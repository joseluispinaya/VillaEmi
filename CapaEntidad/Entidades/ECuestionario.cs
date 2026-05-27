namespace CapaEntidad.Entidades
{
    public class ECuestionario
    {
        public int IdCuestionario { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        //public string FechaCreado { get; set; }
        public int NroPreguntas { get; set; }
        public string CantiPreg =>
            NroPreguntas == 0
            ? "0 Preguntas"
            : NroPreguntas == 1
                ? "1 Pregunta"
                : $"{NroPreguntas} Preguntas";
    }
}
