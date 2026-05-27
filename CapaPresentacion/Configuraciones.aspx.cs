using CapaEntidad.Entidades;
using CapaEntidad.Responses;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapaPresentacion
{
    public partial class Configuraciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static Respuesta<List<ECarrera>> ListaCarreras()
        {
            return NPregunta.GetInstance().ListaCarreras();
        }


        [WebMethod]
        public static Respuesta<int> GuardarOrEditCarreras(ECarrera objeto)
        {
            return NPregunta.GetInstance().GuardarOrEditCarreras(objeto);
        }
    }
}