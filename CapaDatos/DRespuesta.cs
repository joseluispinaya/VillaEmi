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
    public class DRespuesta
    {
        #region "PATRON SINGLETON"
        private static DRespuesta instancia = null;
        private DRespuesta() { }
        public static DRespuesta GetInstance()
        {
            if (instancia == null)
            {
                instancia = new DRespuesta();
            }
            return instancia;
        }
        #endregion

        public Respuesta<int> RegistrarTest(int idEstudiante, string respuestasJson)
        {
            var respuesta = new Respuesta<int>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                using (SqlCommand cmd = new SqlCommand("usp_RegistrarTestYRespuestas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.Add("@IdEstudiante", SqlDbType.Int).Value = idEstudiante;
                    cmd.Parameters.Add("@RespuestasJSON", SqlDbType.NVarChar).Value = respuestasJson;

                    // Parámetro de salida (OUTPUT)
                    var paramIdGenerado = new SqlParameter("@IdTestGenerado", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramIdGenerado);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    // Obtenemos el valor retornado por el procedimiento
                    int idGenerado = Convert.ToInt32(paramIdGenerado.Value);

                    if (idGenerado > 0)
                    {
                        respuesta.Estado = true;
                        respuesta.Data = idGenerado; // Guardamos el ID para usarlo luego
                        respuesta.Mensaje = "Test y respuestas registrados correctamente.";
                    }
                    else
                    {
                        respuesta.Estado = false;
                        respuesta.Data = 0;
                        respuesta.Mensaje = "Error interno en la base de datos al registrar el test.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = false;
                respuesta.Data = 0;
                respuesta.Mensaje = $"Error de BD: {ex.Message}";
            }

            return respuesta;
        }

        public Respuesta<List<ResultTestDTO>> ObtenerTestResp(int idTest)
        {
            List<ResultTestDTO> lista = new List<ResultTestDTO>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ObtenerDatosParaIA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdTest", idTest);

                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new ResultTestDTO
                                {
                                    Pregunta = dr["Pregunta"].ToString(),
                                    RespuestaTest = dr["Respuesta"].ToString()
                                });
                            }
                        }
                    }
                }

                // Validación: Si no hay resultados
                if (lista.Count == 0)
                {
                    return new Respuesta<List<ResultTestDTO>>
                    {
                        Estado = false,
                        Valor = "warning",
                        Mensaje = "No existen resultados disponibles.",
                        Data = lista // o null, dependiendo de tu preferencia
                    };
                }

                // Éxito: Retornamos las preguntas aleatorias
                return new Respuesta<List<ResultTestDTO>>
                {
                    Estado = true,
                    Valor = "success",
                    Mensaje = "Resultados obtenidos",
                    Data = lista
                };
            }
            catch (Exception)
            {
                return new Respuesta<List<ResultTestDTO>>
                {
                    Estado = false,
                    Valor = "error",
                    Mensaje = "Ocurrió un error en el servidor",
                    Data = null
                };
            }
        }

        public Respuesta<int> RegistrarResultIa(int idTest, string observacionGeneralIA, string recomendacionesJSON)
        {
            var respuesta = new Respuesta<int>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                using (SqlCommand cmd = new SqlCommand("usp_GuardarResultadosIA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.Add("@IdTest", SqlDbType.Int).Value = idTest;
                    cmd.Parameters.Add("@ObservacionGeneralIA", SqlDbType.NVarChar).Value = observacionGeneralIA;
                    cmd.Parameters.Add("@RecomendacionesJSON", SqlDbType.NVarChar).Value = recomendacionesJSON;

                    // Parámetro de salida (OUTPUT)
                    var paramResult = new SqlParameter("@Resultado", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramResult);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    int resultInt = Convert.ToInt32(paramResult.Value);

                    if (resultInt > 0)
                    {
                        respuesta.Estado = true;
                        respuesta.Data = resultInt;
                        respuesta.Mensaje = "Test y respuestas registrados correctamente.";
                    }
                    else
                    {
                        respuesta.Estado = false;
                        respuesta.Data = 0;
                        respuesta.Mensaje = "Error interno en la base de datos al registrar el resultado.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = false;
                respuesta.Data = 0;
                respuesta.Mensaje = $"Error de BD: {ex.Message}";
            }

            return respuesta;
        }

        public Respuesta<List<HistorialTestDTO>> HistorialTestEst(int IdEstudiante)
        {
            List<HistorialTestDTO> lista = new List<HistorialTestDTO>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_HistorialTest", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdEstudiante", IdEstudiante);

                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new HistorialTestDTO
                                {
                                    IdTest = Convert.ToInt32(dr["IdTest"]),
                                    ObservacionGeneralIA = dr["ObservacionGeneralIA"].ToString(),
                                    FechaTest = Convert.ToDateTime(dr["FechaTest"]).ToString("dd/MM/yyyy")
                                });
                            }
                        }
                    }
                }

                // Validación: Si no hay preguntas
                if (lista.Count == 0)
                {
                    return new Respuesta<List<HistorialTestDTO>>
                    {
                        Estado = false,
                        Valor = "warning",
                        Mensaje = "No existen historiales de test disponibles.",
                        Data = lista // o null, dependiendo de tu preferencia
                    };
                }

                // Éxito: Retornamos las preguntas aleatorias
                return new Respuesta<List<HistorialTestDTO>>
                {
                    Estado = true,
                    Valor = "success",
                    Mensaje = "Historial obtenido correctamente.",
                    Data = lista
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<List<HistorialTestDTO>>
                {
                    Estado = false,
                    Valor = "error",
                    Mensaje = "Ocurrió un error al obtener el historial: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<List<DetalleHistDTO>> DetalleHistorialTest(int IdTest)
        {
            List<DetalleHistDTO> lista = new List<DetalleHistDTO>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_DetalleIdTest", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdTest", IdTest);

                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new DetalleHistDTO
                                {
                                    Carrera = dr["Nombre"].ToString(),
                                    Puntaje = Convert.ToDecimal(dr["Puntaje"]),
                                    Justificacion = dr["Justificacion"].ToString()
                                });
                            }
                        }
                    }
                }

                // Validación: Si no hay preguntas
                if (lista.Count == 0)
                {
                    return new Respuesta<List<DetalleHistDTO>>
                    {
                        Estado = false,
                        Valor = "warning",
                        Mensaje = "No existen detalles de test disponibles.",
                        Data = lista // o null, dependiendo de tu preferencia
                    };
                }

                // Éxito: Retornamos las preguntas aleatorias
                return new Respuesta<List<DetalleHistDTO>>
                {
                    Estado = true,
                    Valor = "success",
                    Mensaje = "Detalles obtenido correctamente.",
                    Data = lista
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<List<DetalleHistDTO>>
                {
                    Estado = false,
                    Valor = "error",
                    Mensaje = "Ocurrió un error al obtener el detalle: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}
