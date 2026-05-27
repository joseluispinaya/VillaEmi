using CapaEntidad.Entidades;
using CapaEntidad.Responses;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapaPresentacion
{
    public partial class PageDirectores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static Respuesta<EDirector> ObtenerDirectorPorId(int IdDirector)
        {
            return NDirector.GetInstance().ObtenerDirectorPorId(IdDirector);
        }

        [WebMethod]
        public static Respuesta<List<EDirector>> ListaDirectores()
        {
            return NDirector.GetInstance().ListaDirectores(0);
        }

        [WebMethod]
        public static Respuesta<int> GuardarOrEditDirector(EDirector objeto, string base64Image)
        {
            try
            {
                var utilidades = Utilidadesj.GetInstance();

                // 1. Manejo de la foto
                if (!string.IsNullOrEmpty(base64Image))
                {
                    byte[] imageBytes = Convert.FromBase64String(base64Image);
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        string folder = "/Imagenes/";
                        objeto.Photo = utilidades.UploadPhoto(stream, folder);
                    }
                }
                else
                {
                    // Si no hay foto nueva, enviamos vacío.
                    // Si es nuevo registro, guardará "". Si es edición, el SP conservará la foto antigua.
                    objeto.Photo = "";
                }

                // 2. Manejo de la clave
                if (objeto.IdDirector == 0)
                {
                    string clavePlana = objeto.NroCi;
                    objeto.ClaveHash = utilidades.Hash(clavePlana);
                }
                else
                {
                    objeto.ClaveHash = ""; // El SP conservará la clave antigua
                }

                return NDirector.GetInstance().GuardarOrEditDirector(objeto);
            }
            catch (Exception ex)
            {
                return new Respuesta<int> { Estado = false, Valor = "error", Mensaje = "Error en el servidor: " + ex.Message };
            }
        }
    }
}