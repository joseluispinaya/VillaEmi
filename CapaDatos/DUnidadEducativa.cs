using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad.Entidades;
using CapaEntidad.Responses;

namespace CapaDatos
{
    public class DUnidadEducativa
    {
        #region "PATRON SINGLETON"
        private static DUnidadEducativa instancia = null;
        private DUnidadEducativa() { }
        public static DUnidadEducativa GetInstance()
        {
            if (instancia == null)
            {
                instancia = new DUnidadEducativa();
            }
            return instancia;
        }
        #endregion

        public Respuesta<int> GuardarOrEditUnidadesEdu(EUnidadEducativa oModel)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GuardarOrEditUnidadEducativa", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUnidadEducativa", oModel.IdUnidadEducativa);
                        cmd.Parameters.AddWithValue("@Nombre", oModel.Nombre);
                        cmd.Parameters.AddWithValue("@NroContacto", oModel.NroContacto);
                        cmd.Parameters.AddWithValue("@Ubicacion", oModel.Ubicacion);

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
                        response.Mensaje = "El nombre de la unidad educativa ya existe.";
                        break;

                    case 2: // Registro Nuevo
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Unidad educativa registrada correctamente.";
                        break;

                    case 3: // Actualización
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Unidad educativa actualizada correctamente.";
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

        public Respuesta<List<EUnidadEducativa>> ListaUndEducativas()
        {
            try
            {
                List<EUnidadEducativa> rptLista = new List<EUnidadEducativa>();
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ListarUnidadEducativaFull", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EUnidadEducativa
                                {
                                    IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    NroContacto = dr["NroContacto"].ToString(),
                                    Ubicacion = dr["Ubicacion"].ToString(),
                                    FechaCreado = Convert.ToDateTime(dr["FechaCreado"]).ToString("dd/MM/yyyy"),
                                    Responsable = dr["Responsable"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EUnidadEducativa>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Lista obtenida correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<List<EUnidadEducativa>>()
                {
                    Estado = false,
                    Data = null,
                    Mensaje = $"Error al obtener la lista: {ex.Message}"
                };
            }
        }

        public Respuesta<List<EUnidadEducativa>> ListaUndEducativasDisponibles(int idUnidadEducativaActual)
        {
            try
            {
                List<EUnidadEducativa> rptLista = new List<EUnidadEducativa>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("sp_ListarUnidadesEducativasDisponibles", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdUnidadEducativaActual", idUnidadEducativaActual);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EUnidadEducativa
                                {
                                    IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                                    Nombre = dr["Nombre"].ToString()
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EUnidadEducativa>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Lista obtenidas correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EUnidadEducativa>>()
                {
                    Estado = false,
                    Mensaje = $"Error al obtener la lista: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
