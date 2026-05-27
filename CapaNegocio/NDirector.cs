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
    public class NDirector
    {
        #region "PATRON SINGLETON"
        private static NDirector instancia = null;
        private NDirector() { }
        public static NDirector GetInstance()
        {
            if (instancia == null)
            {
                instancia = new NDirector();
            }
            return instancia;
        }
        #endregion
        public Respuesta<int> GuardarOrEditDirector(EDirector oModel)
        {
            return DDirector.GetInstance().GuardarOrEditDirector(oModel);
        }

        public Respuesta<List<EDirector>> ListaDirectores(int idDirector)
        {
            return DDirector.GetInstance().ListaDirectores(idDirector);
        }

        public Respuesta<EDirector> ObtenerDirectorPorId(int idDirector)
        {
            return DDirector.GetInstance().ObtenerDirectorPorId(idDirector);
        }
    }
}
