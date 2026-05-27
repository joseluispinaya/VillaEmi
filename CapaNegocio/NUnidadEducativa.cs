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
    public class NUnidadEducativa
    {
        #region "PATRON SINGLETON"
        private static NUnidadEducativa instancia = null;
        private NUnidadEducativa() { }
        public static NUnidadEducativa GetInstance()
        {
            if (instancia == null)
            {
                instancia = new NUnidadEducativa();
            }
            return instancia;
        }
        #endregion
        public Respuesta<int> GuardarOrEditUnidadesEdu(EUnidadEducativa oModel)
        {
            return DUnidadEducativa.GetInstance().GuardarOrEditUnidadesEdu(oModel);
        }

        public Respuesta<List<EUnidadEducativa>> ListaUndEducativas()
        {
            return DUnidadEducativa.GetInstance().ListaUndEducativas();
        }

        public Respuesta<List<EUnidadEducativa>> ListaUndEducativasDisponibles(int idUnidadEducativaActual)
        {
            return DUnidadEducativa.GetInstance().ListaUndEducativasDisponibles(idUnidadEducativaActual);
        }
    }
}
