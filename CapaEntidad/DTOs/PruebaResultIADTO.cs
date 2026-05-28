using System.Collections.Generic;

namespace CapaEntidad.DTOs
{
    public class PruebaResultIADTO
    {
        // Nuevos campos para validación
        public bool TestValido { get; set; }
        public string MensajeError { get; set; }

        public string ObservacionGeneralIA { get; set; }
        public List<RecomendacionesDTO> Recomendaciones { get; set; }
    }
}
