namespace CapaEntidad.Entidades
{
    public class EEstudiante
    {
        public int IdEstudiante { get; set; }
        public int IdUnidadEducativa { get; set; }
        public string NroCi { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string ClaveHash { get; set; }
        public string Photo { get; set; }
        public bool Estado { get; set; }
        // auxiliar
        public string NombreUndEd { get; set; }
        public string FullName => $"{Nombres} {Apellidos}";
    }
}
