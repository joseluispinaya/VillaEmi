namespace CapaEntidad.DTOs
{
    public class EstudianteDTO
    {
        public int IdUnidadEducativa { get; set; }
        public string NroCi { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string ClaveHash { get; set; }
        public string Photo { get; set; }
        public string Base64Image { get; set; }
    }
}
