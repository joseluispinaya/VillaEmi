using System.Collections.Generic;

namespace CapaEntidad.DTOs
{
    public class SolicitudTestDTO
    {
        public int IdEstudiante { get; set; }
        public List<RespuestaItemDTO> Respuestas { get; set; }
    }
}
