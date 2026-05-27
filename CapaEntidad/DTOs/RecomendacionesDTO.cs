namespace CapaEntidad.DTOs
{
    public class RecomendacionesDTO
    {
        public int IdCarrera { get; set; }
        public string Carrera { get; set; }
        public decimal Puntaje { get; set; }
        public string Justificacion { get; set; }
        public int Orden { get; set; }
    }
}
