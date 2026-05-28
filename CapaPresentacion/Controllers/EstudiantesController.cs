using CapaEntidad.DTOs;
using CapaEntidad.Entidades;
using CapaEntidad.Responses;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CapaPresentacion.Controllers
{
    // 1. Habilitamos CORS para que la App Móvil no tenga bloqueos
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    // 2. Definimos la ruta base para este controlador
    [RoutePrefix("api/estudiantes")]
    public class EstudiantesController : ApiController
    {
        [HttpPost]
        [Route("registro")]
        public IHttpActionResult RegistrarEst([FromBody] EstudianteDTO request)
        {
            // 1. Validación inicial del objeto
            if (request == null)
            {
                return Ok(new Respuesta<int>
                {
                    Estado = false,
                    Valor = "warning",
                    Mensaje = "Debe enviar los datos requeridos."
                });
            }

            // 2. Validación de campos obligatorios críticos (¡NUEVO!)
            if (string.IsNullOrEmpty(request.ClaveHash) || string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.NroCi))
            {
                return Ok(new Respuesta<int>
                {
                    Estado = false,
                    Valor = "warning",
                    Mensaje = "El carnet, correo y contraseña son obligatorios."
                });
            }

            try
            {
                var utilidades = Utilidadesj.GetInstance();
                string imageUrl = string.Empty;

                // 3. Manejo de la foto
                if (!string.IsNullOrEmpty(request.Base64Image))
                {
                    byte[] imageBytes = Convert.FromBase64String(request.Base64Image);
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        string folder = "/ImagenesEst/";
                        imageUrl = utilidades.UploadPhoto(stream, folder);
                    }
                }

                request.Photo = imageUrl;

                // 4. Encriptación segura (ya validamos arriba que no es null)
                request.ClaveHash = utilidades.Hash(request.ClaveHash);

                // 5. Llamada a la capa de negocios
                Respuesta<int> resultadoBD = NEstudiante.GetInstance().RegistroEstAppNew(request);

                // 6. Retornar la respuesta
                return Ok(resultadoBD);
            }
            catch (Exception)
            {
                return Ok(new Respuesta<int>
                {
                    Estado = false,
                    Valor = "error",
                    Mensaje = "Ocurrió un error interno en el servidor"
                });
            }
        }

        [HttpGet]
        [Route("combo")]
        public IHttpActionResult ListaUndEducativas()
        {
            var respuesta = NUnidadEducativa.GetInstance().ListaUndEducativas();
            return Ok(respuesta);
        }

        [HttpPost]
        [Route("loginApp")]
        public IHttpActionResult LoginEstudiante([FromBody] LoginDTO loginDTO)
        {
            try
            {
                // 1. Validación de entrada
                if (loginDTO == null || string.IsNullOrWhiteSpace(loginDTO.Correo) || string.IsNullOrWhiteSpace(loginDTO.Clave))
                {
                    return Ok(RespuestaError("Debe ingresar su correo y una contraseña para iniciar sesión."));
                }

                // 2. Consulta a la base de datos
                var respuesta = NEstudiante.GetInstance().LoginEstudiante(loginDTO.Correo);

                if (!respuesta.Estado || respuesta.Data == null)
                {
                    return Ok(RespuestaError(respuesta.Mensaje ?? "Credenciales incorrectas."));
                }

                var objCliente = respuesta.Data;

                if (!objCliente.Estado)
                {
                    return Ok(RespuestaError("Su cuenta no está activa."));
                }

                // 3. Verificamos la contraseña (BCrypt)
                bool passCorrecta = Utilidadesj.GetInstance().Verify(loginDTO.Clave, objCliente.ClaveHash);

                if (!passCorrecta)
                {
                    return Ok(RespuestaError("Usuario o Contraseña incorrectos."));
                }

                // 5. Limpiamos la contraseña por seguridad antes de mandar el JSON
                objCliente.ClaveHash = "";

                // 6. Éxito
                return Ok(new Respuesta<EEstudiante>
                {
                    Estado = true,
                    Mensaje = "Bienvenido al sistema",
                    Data = objCliente
                });
            }
            catch (Exception)
            {
                // Si algo explota (ej: fallo en BCrypt), el cliente recibe un JSON ordenado
                return Ok(RespuestaError("Ocurrió un error inesperado al iniciar sesión"));
            }
        }

        [HttpPost]
        [Route("editClave")]
        public IHttpActionResult CambiarClave([FromBody] ChangeDTO changeDTO)
        {
            try
            {
                var utilidades = Utilidadesj.GetInstance();

                if (changeDTO == null)
                {
                    return Ok(new Respuesta<bool>
                    {
                        Estado = false,
                        Mensaje = "Debe enviar los datos requeridos.",
                        Data = false
                    });
                }

                if (string.IsNullOrEmpty(changeDTO.Correo) || string.IsNullOrEmpty(changeDTO.ClaveActual) || string.IsNullOrEmpty(changeDTO.ClaveNueva))
                {
                    return Ok(new Respuesta<bool>
                    {
                        Estado = false,
                        Mensaje = "La contraseña actual y la nueva contraseña son obligatorios.",
                        Data = false
                    });
                }

                // 3. Llamada a la capa de datos
                var resp = NEstudiante.GetInstance().LoginEstudiante(changeDTO.Correo);

                // 5. Verificar si el estudiante existe en BD
                if (!resp.Estado || resp.Data == null)
                {
                    return Ok(new Respuesta<bool>
                    {
                        Estado = false,
                        Mensaje = resp.Mensaje,
                        Data = false
                    });
                }

                var objEst = resp.Data;

                // 6. Verificamos la contraseña actual (BCrypt)
                bool passCorrecta = utilidades.Verify(changeDTO.ClaveActual, objEst.ClaveHash);

                if (!passCorrecta)
                {
                    return Ok(new Respuesta<bool>
                    {
                        Estado = false,
                        Mensaje = "La Contraseña actual es incorrecta.",
                        Data = false
                    });
                }

                string claveNewEncrypt = utilidades.Hash(changeDTO.ClaveNueva);

                Respuesta<bool> resultadoBD = NEstudiante.GetInstance().ActualizarClave(objEst.IdEstudiante, claveNewEncrypt);

                return Ok(resultadoBD);
            }
            catch (Exception)
            {
                return Ok(new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "Error interno en el servidor.",
                    Data = false
                });
            }
        }

        private Respuesta<EEstudiante> RespuestaError(string mensaje)
        {
            return new Respuesta<EEstudiante>
            {
                Estado = false,
                Mensaje = mensaje,
                Data = null
            };
        }

    }
}