using System.Collections.Generic;

namespace CapaEntidad.DTOs
{
    public class ResultadoIADTO
    {
        public string ObservacionGeneralIA { get; set; }
        public List<RecomendacionesDTO> Recomendaciones { get; set; }
    }
}
