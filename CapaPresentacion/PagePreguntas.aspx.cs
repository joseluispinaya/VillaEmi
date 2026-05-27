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
    public partial class PagePreguntas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static Respuesta<List<EPregunta>> ListaPreguntasId(int IdCuestionario)
        {
            return NPregunta.GetInstance().ListarPreguntas(IdCuestionario);
        }


        [WebMethod]
        public static Respuesta<int> GuardarOrEditPregunta(EPregunta objeto)
        {
            return NPregunta.GetInstance().GuardarOrEditPregunta(objeto);
        }
    }
}