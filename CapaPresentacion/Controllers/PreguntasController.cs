using CapaEntidad.DTOs;
using CapaEntidad.Responses;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CapaPresentacion.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    // 2. Definimos la ruta base para este controlador
    [RoutePrefix("api/preguntas")]
    public class PreguntasController : ApiController
    {
        [HttpGet]
        [Route("preguntasTest")]
        public IHttpActionResult PreguntasRandon()
        {
            var respuesta = NPregunta.GetInstance().ObtenerPreguntasAleatorias(7);
            return Ok(respuesta);
        }

        [HttpPost]
        [Route("prueba")]
        public IHttpActionResult PruebaTest([FromBody] List<ResultTestDTO> request)
        {
            if (request == null || request.Count == 0)
            {
                return Ok(new Respuesta<ResultadoIADTO>
                {
                    Estado = false,
                    Data = null,
                    Mensaje = "Datos incompletos. Asegúrese de enviar preguntas y respuestas."
                });
            }

            try
            {

                // PASO 3: ENVIAR A LA IA (Usando tu clase HelpersIA)
                var resultadoIA = HelpersIA.GetInstance().AnalizarTestConIA(request);

                return Ok(resultadoIA);

            }
            catch (Exception ex)
            {
                return Ok(new Respuesta<ResultadoIADTO>
                {
                    Estado = false,
                    Data = null,
                    Mensaje = "Ocurrió un error en el servidor: " + ex.Message
                });
            }

        }

        [HttpPost]
        [Route("testvalidador")]
        public IHttpActionResult PruebaTestValidador([FromBody] List<ResultTestDTO> request)
        {
            if (request == null || request.Count == 0)
            {
                return Ok(new Respuesta<PruebaResultIADTO>
                {
                    Estado = false,
                    Data = null,
                    Mensaje = "Datos incompletos. Asegúrese de enviar preguntas y respuestas."
                });
            }

            try
            {

                // PASO 3: ENVIAR A LA IA (Usando tu clase HelpersIA)
                var resultadoIA = ModeloTestIA.GetInstance().AnalizarTestPruebas(request);

                return Ok(resultadoIA);

            }
            catch (Exception ex)
            {
                return Ok(new Respuesta<PruebaResultIADTO>
                {
                    Estado = false,
                    Data = null,
                    Mensaje = "Ocurrió un error en el servidor: " + ex.Message
                });
            }

        }
    }
}