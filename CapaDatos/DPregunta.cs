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
    public class DPregunta
    {
        #region "PATRON SINGLETON"
        private static DPregunta instancia = null;
        private DPregunta() { }
        public static DPregunta GetInstance()
        {
            if (instancia == null)
            {
                instancia = new DPregunta();
            }
            return instancia;
        }
        #endregion

        public Respuesta<List<EPregunta>> ListarPreguntas(int idCuestionario)
        {
            try
            {
                List<EPregunta> rptLista = new List<EPregunta>();
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ListarPregunta", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdCuestionario", idCuestionario);
                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EPregunta
                                {
                                    IdPregunta = Convert.ToInt32(dr["IdPregunta"]),
                                    IdCuestionario = Convert.ToInt32(dr["IdCuestionario"]),
                                    TituloCuestionario = dr["TituloCuestionario"].ToString(),
                                    Texto = dr["Texto"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EPregunta>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Lista obtenida correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<List<EPregunta>>()
                {
                    Estado = false,
                    Data = null,
                    Mensaje = $"Error al obtener la lista: {ex.Message}"
                };
            }
        }

        public Respuesta<int> GuardarOrEditPregunta(EPregunta oModel)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GuardarOrEditPregunta", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdPregunta", oModel.IdPregunta);
                        cmd.Parameters.AddWithValue("@IdCuestionario", oModel.IdCuestionario);
                        cmd.Parameters.AddWithValue("@Texto", oModel.Texto);

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
                        response.Mensaje = "La pregunta ingresada ya existe.";
                        break;

                    case 2: // Registro Nuevo
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Registrado correctamente.";
                        break;

                    case 3: // Actualización
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Actualizado correctamente.";
                        break;

                    case 0: // Error
                    default:
                        response.Estado = false;
                        response.Valor = "error"; // <--- Usamos Valor para el icono ROJO
                        response.Mensaje = "No se pudo completar la operación.";
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Data = 0;
                response.Estado = false;
                response.Valor = "error";
                response.Mensaje = "Error interno: " + ex.Message;
            }

            return response;
        }

        public Respuesta<List<PreguntaDTO>> ObtenerPreguntasAleatorias(int cantidad = 6)
        {
            List<PreguntaDTO> lista = new List<PreguntaDTO>();

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ObtenerPreguntasAleatorias", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new PreguntaDTO
                                {
                                    IdPregunta = Convert.ToInt32(dr["IdPregunta"]),
                                    IdCuestionario = Convert.ToInt32(dr["IdCuestionario"]),
                                    Texto = dr["Texto"].ToString()
                                });
                            }
                        }
                    }
                }

                // Validación: Si no hay preguntas
                if (lista.Count == 0)
                {
                    return new Respuesta<List<PreguntaDTO>>
                    {
                        Estado = false,
                        Valor = "warning",
                        Mensaje = "No existen preguntas disponibles.",
                        Data = lista // o null, dependiendo de tu preferencia
                    };
                }

                // Éxito: Retornamos las preguntas aleatorias
                return new Respuesta<List<PreguntaDTO>>
                {
                    Estado = true,
                    Valor = "success",
                    Mensaje = "Preguntas obtenidas",
                    Data = lista
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<List<PreguntaDTO>>
                {
                    Estado = false,
                    Valor = "error",
                    Mensaje = "Ocurrió un error al obtener las preguntas: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<int> GuardarOrEditCarreras(ECarrera oModel)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GuardarOrEditCarrera", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCarrera", oModel.IdCarrera);
                        cmd.Parameters.AddWithValue("@Nombre", oModel.Nombre);
                        cmd.Parameters.AddWithValue("@Estado", oModel.Estado);

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
                        response.Mensaje = "La Carrera ya existe.";
                        break;

                    case 2: // Registro Nuevo
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Registrado correctamente.";
                        break;

                    case 3: // Actualización
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Actualizado correctamente.";
                        break;

                    case 0: // Error
                    default:
                        response.Estado = false;
                        response.Valor = "error"; // <--- Usamos Valor para el icono ROJO
                        response.Mensaje = "No se pudo completar la operación.";
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Valor = "error";
                response.Mensaje = "Error interno: " + ex.Message;
            }

            return response;
        }

        public Respuesta<List<ECarrera>> ListaCarreras()
        {
            try
            {
                List<ECarrera> rptLista = new List<ECarrera>();
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ListarCarrera", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new ECarrera()
                                {
                                    IdCarrera = Convert.ToInt32(dr["IdCarrera"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"])
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<ECarrera>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Lista obtenida correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<List<ECarrera>>()
                {
                    Estado = false,
                    Data = null,
                    Mensaje = $"Error al obtener la lista de carreras: {ex.Message}"
                };
            }
        }

    }
}
