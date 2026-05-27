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
    public class NEstudiante
    {
        #region "PATRON SINGLETON"
        private static NEstudiante instancia = null;
        private NEstudiante() { }
        public static NEstudiante GetInstance()
        {
            if (instancia == null)
            {
                instancia = new NEstudiante();
            }
            return instancia;
        }
        #endregion

        public Respuesta<List<EEstudiante>> ListarEstIdUndEd(int idUnidadEducativa)
        {
            return DEstudiante.GetInstance().ListarEstIdUndEd(idUnidadEducativa);
        }

        public Respuesta<int> RegistroEstAppNew(EstudianteDTO oModel)
        {
            return DEstudiante.GetInstance().RegistroEstAppNew(oModel);
        }

        public Respuesta<int> RegistroEstApp(EEstudiante oModel)
        {
            return DEstudiante.GetInstance().RegistroEstApp(oModel);
        }

        public Respuesta<EEstudiante> LoginEstudiante(string Correo)
        {
            return DEstudiante.GetInstance().LoginEstudiante(Correo);
        }

        public Respuesta<bool> ActualizarClave(int IdEstudiante, string NuevaClave)
        {
            return DEstudiante.GetInstance().ActualizarClave(IdEstudiante, NuevaClave);
        }
    }
}
