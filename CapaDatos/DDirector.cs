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
    public class DDirector
    {
        #region "PATRON SINGLETON"
        private static DDirector instancia = null;
        private DDirector() { }
        public static DDirector GetInstance()
        {
            if (instancia == null)
            {
                instancia = new DDirector();
            }
            return instancia;
        }
        #endregion
        public Respuesta<int> GuardarOrEditDirector(EDirector oModel)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;
            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GuardarOEditarDirector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdDirector", oModel.IdDirector);
                        cmd.Parameters.AddWithValue("@IdUnidadEducativa", oModel.IdUnidadEducativa);
                        cmd.Parameters.AddWithValue("@Nombres", oModel.Nombres);
                        cmd.Parameters.AddWithValue("@Apellidos", oModel.Apellidos);
                        cmd.Parameters.AddWithValue("@NroCi", oModel.NroCi);
                        cmd.Parameters.AddWithValue("@Celular", oModel.Celular);
                        cmd.Parameters.AddWithValue("@Correo", oModel.Correo);
                        cmd.Parameters.AddWithValue("@ClaveHash", string.IsNullOrEmpty(oModel.ClaveHash) ? "" : oModel.ClaveHash);
                        cmd.Parameters.AddWithValue("@Photo", string.IsNullOrEmpty(oModel.Photo) ? "" : oModel.Photo);
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

                    case 3: // Actualización
                        response.Estado = true;
                        response.Valor = "success"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "Actualizado correctamente.";
                        break;
                    case 4: // ya tiene un director asignado
                        response.Estado = false;
                        response.Valor = "warning"; // <--- Usamos Valor para el icono VERDE
                        response.Mensaje = "La unidad educativa seleccionada ya esta asignada a un director.";
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
                response.Mensaje = $"Error al guardar o editar el director: {ex.Message}";
            }
            return response;
        }

        public Respuesta<List<EDirector>> ListaDirectores(int idDirector)
        {
            try
            {
                List<EDirector> rptLista = new List<EDirector>();

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerDirectores", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdDirector", idDirector);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new EDirector
                                {
                                    IdDirector = Convert.ToInt32(dr["IdDirector"]),
                                    IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                                    NombreUnidadEducativa = dr["NombreUnidadEducativa"].ToString(),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    NroCi = dr["NroCi"].ToString(),
                                    Celular = dr["Celular"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Photo = dr["Photo"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"])
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<EDirector>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Lista obtenidos correctamente"
                };
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return new Respuesta<List<EDirector>>()
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

        public Respuesta<EDirector> ObtenerDirectorPorId(int idDirector)
        {
            try
            {
                EDirector obj = null;

                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerDirectores", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdDirector", idDirector);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new EDirector
                                {
                                    IdDirector = Convert.ToInt32(dr["IdDirector"]),
                                    IdUnidadEducativa = Convert.ToInt32(dr["IdUnidadEducativa"]),
                                    NombreUnidadEducativa = dr["NombreUnidadEducativa"].ToString(),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    NroCi = dr["NroCi"].ToString(),
                                    Celular = dr["Celular"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Photo = dr["Photo"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Estado"])
                                };
                            }
                        }
                    }
                }

                // Si obj es null, es que el correo no existe
                return new Respuesta<EDirector>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Datos Obtenidas" : "No se tiene registros del director"
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<EDirector>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}
