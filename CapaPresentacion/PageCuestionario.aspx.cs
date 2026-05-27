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
    public partial class PageCuestionario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static Respuesta<List<ECuestionario>> ListaCuestionarios()
        {
            return NCuestionario.GetInstance().ListaCuestionarios();
        }


        [WebMethod]
        public static Respuesta<int> GuardarOrEditCuestionario(ECuestionario objeto)
        {
            return NCuestionario.GetInstance().GuardarOrEditCuestionario(objeto);
        }
    }
}