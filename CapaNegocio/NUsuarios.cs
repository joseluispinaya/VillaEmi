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
    public class NUsuarios
    {
        #region "PATRON SINGLETON"
        private static NUsuarios instancia = null;
        private NUsuarios() { }
        public static NUsuarios GetInstance()
        {
            if (instancia == null)
            {
                instancia = new NUsuarios();
            }
            return instancia;
        }
        #endregion

        public Respuesta<EUsuarios> LoginUsuario(string Correo)
        {
            var correoPrueba = "admindev@yopmail.com";

            if (correoPrueba.ToLower() != Correo.Trim().ToLower())
            {
                return new Respuesta<EUsuarios>
                {
                    Estado = false,
                    Data = null,
                    Mensaje = "Usuario o Contraseña incorrectos."
                };
            }


            EUsuarios obj = new EUsuarios
            {
                IdUsuario = 1,
                Nombres = "Juan Carlos",
                Apellidos = "Luna Paz",
                NroCi = "32547854",
                Correo = "admindev@yopmail.com",
                Celular = "73999748",
                ClaveHash = "123456789",
                ImagenUser = "/images/logodefault.png",
                Cargo = "Administrador",
                IdRol = 2,
                Estado = true
            };

            return new Respuesta<EUsuarios>
            {
                Estado = true,
                Data = obj,
                Mensaje = "Bienvenido al sistema"
            };

        }
    }
}
