namespace CapaEntidad.Entidades
{
    public class EDirector
    {
        public int IdDirector { get; set; }
        public int IdUnidadEducativa { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NroCi { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string ClaveHash { get; set; }
        public string Photo { get; set; }
        public bool Estado { get; set; }
        public string NombreUnidadEducativa { get; set; }
        public string FullName => $"{Nombres} {Apellidos}";
    }
}
