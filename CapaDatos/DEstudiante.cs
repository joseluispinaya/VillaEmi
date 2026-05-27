using CapaEntidad.DTOs;
using CapaEntidad.Entidades;
using CapaEntidad.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DEstudiante
    {
        #region "PATRON SINGLETON"
        private static DEstudiante instancia = null;
        private DEstudiante() { }
        public static DEstudiante GetInstance()
        {
            if (instancia == null)
            {
                instancia = new DEstudiante();
            }
            return instancia;
        }
        #endregion

        public Respuesta<List<EEstudiante>> ListarEstIdUndEd(int idUnidadEducativa)
        {
            try
            {
                List<EEstudiante> rptLista = new List<EEstudiante>();
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ListarEstudiantesIdUnd", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdUnidadEducativa", idUnidadEducativa);
                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EEstudiante
                                {
                                    IdEstudiante = Convert.ToInt32(dr["IdEstudiante"]),
                                    IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                                    NroCi = dr["NroCi"].ToString(),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"]),
                                    Photo = dr["Photo"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EEstudiante>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Lista obtenida correctamente"
                };
            }
            catch (Exception)
            {
                return new Respuesta<List<EEstudiante>>()
                {
                    Estado = false,
                    Data = null,
                    Mensaje = "Error al obtener la lista"
                };
            }
        }

        public Respuesta<int> RegistroEstAppNew(EstudianteDTO oModel)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;
            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarEstudiante", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUnidadEducativa", oModel.IdUnidadEducativa);
                        cmd.Parameters.AddWithValue("@NroCi", oModel.NroCi);
                        cmd.Parameters.AddWithValue("@Nombres", oModel.Nombres);
                        cmd.Parameters.AddWithValue("@Apellidos", oModel.Apellidos);
                        cmd.Parameters.AddWithValue("@Correo", oModel.Correo);
                        cmd.Parameters.AddWithValue("@ClaveHash", oModel.ClaveHash);
                        cmd.Parameters.AddWithValue("@Photo", oModel.Photo);
                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        resultadoCodigo = Convert.ToInt32(outputParam.Value);
                    }
                }
                // Asignamos el código numérico a Data (por si se necesita lógica extra)
                response.Data = resultadoCodigo;
                switch (resultadoCodigo)
                {
                    case 1: // Duplicado
                        response.Estado = false;
                        response.Valor = "warning"; // <--- Usamos Valor para el icono AMARILLO
                        response.Mensaje = "El nro de ci o el correo ya existe.";
                        break;

                    case 2: // Registro Nuevo
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Registrado correctamente.";
                        break;

                    case 0: // Error
                    default:
                        response.Estado = false;
                        response.Valor = "error"; // <--- Usamos Valor para el icono ROJO
                        response.Mensaje = "No se pudo completar la operación.";
                        break;
                }
            }
            catch (Exception)
            {
                response.Data = 0;
                response.Estado = false;
                response.Valor = "error";
                response.Mensaje = "Error al registrar intente mas tarde.";
            }
            return response;
        }

        public Respuesta<int> RegistroEstApp(EEstudiante oModel)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;
            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarEstudiante", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUnidadEducativa", oModel.IdUnidadEducativa);
                        cmd.Parameters.AddWithValue("@NroCi", oModel.NroCi);
                        cmd.Parameters.AddWithValue("@Nombres", oModel.Nombres);
                        cmd.Parameters.AddWithValue("@Apellidos", oModel.Apellidos);
                        cmd.Parameters.AddWithValue("@Correo", oModel.Correo);
                        cmd.Parameters.AddWithValue("@ClaveHash", oModel.ClaveHash);
                        cmd.Parameters.AddWithValue("@Photo", oModel.Photo);
                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        resultadoCodigo = Convert.ToInt32(outputParam.Value);
                    }
                }
                // Asignamos el código numérico a Data (por si se necesita lógica extra)
                response.Data = resultadoCodigo;
                switch (resultadoCodigo)
                {
                    case 1: // Duplicado
                        response.Estado = false;
                        response.Valor = "warning"; // <--- Usamos Valor para el icono AMARILLO
                        response.Mensaje = "El nro de ci o el correo ya existe.";
                        break;

                    case 2: // Registro Nuevo
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Registrado correctamente.";
                        break;

                    case 0: // Error
                    default:
                        response.Estado = false;
                        response.Valor = "error"; // <--- Usamos Valor para el icono ROJO
                        response.Mensaje = "No se pudo completar la operación.";
                        break;
                }
            }
            catch (Exception)
            {
                response.Data = 0;
                response.Estado = false;
                response.Valor = "error";
                response.Mensaje = "Error al registrar intente mas tarde.";
            }
            return response;
        }

        public Respuesta<EEstudiante> LoginEstudiante(string Correo)
        {
            try
            {
                EEstudiante obj = null;

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_LoginEstudiante", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Correo", Correo);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new EEstudiante
                                {
                                    IdEstudiante = Convert.ToInt32(dr["IdEstudiante"]),
                                    IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                                    NroCi = dr["NroCi"].ToString(),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    ClaveHash = dr["ClaveHash"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"]),
                                    Photo = dr["Photo"].ToString(),
                                    NombreUndEd = dr["NombreUndEd"].ToString()
                                };
                            }
                        }
                    }
                }

                // Si obj es null, es que el correo no existe
                return new Respuesta<EEstudiante>
                {
                    Estado = obj != null,
                    Data = obj,
                    // Es buena práctica de seguridad decir "Credenciales incorrectas" y no "Correo no existe"
                    Mensaje = obj != null ? "Estudiante encontrado" : "Credenciales incorrectas"
                };
            }
            catch (Exception)
            {
                return new Respuesta<EEstudiante>
                {
                    Estado = false,
                    Mensaje = "Error servidor",
                    Data = null
                };
            }
        }

        public Respuesta<bool> ActualizarClave(int IdEstudiante, string NuevaClave)
        {
            try
            {
                bool respuesta = false;
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ActualizarClave", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdEstudiante", IdEstudiante);
                        cmd.Parameters.AddWithValue("@NuevaClave", NuevaClave);

                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        respuesta = Convert.ToBoolean(outputParam.Value);
                    }
                }
                return new Respuesta<bool>
                {
                    Estado = respuesta,
                    Mensaje = respuesta ? "Contraseña actualizada" : "Error al actualizar intente mas tarde"
                };
            }
            catch (Exception)
            {
                return new Respuesta<bool> { Estado = false, Mensaje = "Ocurrió un error en la BD" };
            }
        }

    }
}
