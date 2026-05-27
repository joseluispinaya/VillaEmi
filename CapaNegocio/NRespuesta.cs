using CapaDatos;
using CapaEntidad.DTOs;
using CapaEntidad.Entidades;
using CapaEntidad.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NRespuesta
    {
        #region "PATRON SINGLETON"
        private static NRespuesta instancia = null;
        private NRespuesta() { }
        public static NRespuesta GetInstance()
        {
            if (instancia == null)
            {
                instancia = new NRespuesta();
            }
            return instancia;
        }
        #endregion
        public Respuesta<List<ResultTestDTO>> ObtenerTestResp(int idTest)
        {
            return DRespuesta.GetInstance().ObtenerTestResp(idTest);
        }

        public Respuesta<int> RegistrarTest(int idEstudiante, string respuestasJson)
        {
            return DRespuesta.GetInstance().RegistrarTest(idEstudiante, respuestasJson);
        }

        public Respuesta<int> RegistrarResultIa(int idTest, string observacionGeneralIA, string recomendacionesJSON)
        {
            return DRespuesta.GetInstance().RegistrarResultIa(idTest, observacionGeneralIA, recomendacionesJSON);
        }

        public Respuesta<List<HistorialTestDTO>> HistorialTestEst(int IdEstudiante)
        {
            return DRespuesta.GetInstance().HistorialTestEst(IdEstudiante);
        }

        public Respuesta<List<DetalleHistDTO>> DetalleHistorialTest(int IdTest)
        {
            return DRespuesta.GetInstance().DetalleHistorialTest(IdTest);
        }
    }
}
