using CapaEntidad.DTOs;
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
    public partial class PageEstudiantes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static Respuesta<List<HistorialTestDTO>> HistorialTestEst(int IdEstudiante)
        {
            return NRespuesta.GetInstance().HistorialTestEst(IdEstudiante);
        }

        [WebMethod]
        public static Respuesta<List<DetalleHistDTO>> DetalleHistorialTest(int IdTest)
        {
            return NRespuesta.GetInstance().DetalleHistorialTest(IdTest);
        }
    }
}