namespace CapaEntidad.Entidades
{
    public class EUsuarios
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NroCi { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string ClaveHash { get; set; }
        public string ImagenUser { get; set; }
        public string Cargo { get; set; }
        public int IdRol { get; set; }
        public bool Estado { get; set; }
    }
}
