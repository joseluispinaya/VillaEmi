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
    public class DCuestionario
    {
        #region "PATRON SINGLETON"
        private static DCuestionario instancia = null;
        private DCuestionario() { }
        public static DCuestionario GetInstance()
        {
            if (instancia == null)
            {
                instancia = new DCuestionario();
            }
            return instancia;
        }
        #endregion

        public Respuesta<List<ECuestionario>> ListaCuestionarios()
        {
            try
            {
                List<ECuestionario> rptLista = new List<ECuestionario>();
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand("usp_ListarCuestionario", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rptLista.Add(new ECuestionario
                                {
                                    IdCuestionario = Convert.ToInt32(dr["IdCuestionario"]),
                                    Titulo = dr["Titulo"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    NroPreguntas = Convert.ToInt32(dr["NroPreguntas"])
                                });
                            }
                        }
                    }
                }
                return new Respuesta<List<ECuestionario>>()
                {
                    Estado = true,
                    Data = rptLista,
                    Mensaje = "Lista obtenida correctamente"
                };
            }
            catch (Exception ex)
            {
                return new Respuesta<List<ECuestionario>>()
                {
                    Estado = false,
                    Data = null,
                    Mensaje = $"Error al obtener la lista: {ex.Message}"
                };
            }
        }

        public Respuesta<int> GuardarOrEditCuestionario(ECuestionario oModel)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;

            try
            {
                using (SqlConnection con = ConexionBD.GetInstance().ConexionDB())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GuardarOrEditCuestionario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCuestionario", oModel.IdCuestionario);
                        cmd.Parameters.AddWithValue("@Titulo", oModel.Titulo);
                        cmd.Parameters.AddWithValue("@Descripcion", oModel.Descripcion);

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
                        response.Mensaje = "El Titulo ingresado ya existe.";
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
    }
}
