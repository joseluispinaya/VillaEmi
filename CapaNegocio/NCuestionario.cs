using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad.Entidades;
using CapaEntidad.Responses;

namespace CapaNegocio
{
    public class NCuestionario
    {
        #region "PATRON SINGLETON"
        private static NCuestionario instancia = null;
        private NCuestionario() { }
        public static NCuestionario GetInstance()
        {
            if (instancia == null)
            {
                instancia = new NCuestionario();
            }
            return instancia;
        }
        #endregion
        public Respuesta<List<ECuestionario>> ListaCuestionarios()
        {
            return DCuestionario.GetInstance().ListaCuestionarios();
        }

        public Respuesta<int> GuardarOrEditCuestionario(ECuestionario oModel)
        {
            return DCuestionario.GetInstance().GuardarOrEditCuestionario(oModel);
        }
    }
}
