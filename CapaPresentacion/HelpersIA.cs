using CapaEntidad.DTOs;
using CapaEntidad.Entidades;
using CapaEntidad.Responses;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;

namespace CapaPresentacion
{
    public class HelpersIA
    {
        private static readonly string OpenAIApiKey = ConfigurationManager.AppSettings["OpenAIApiKey"];

        #region "PATRON SINGLETON"
        private static HelpersIA _instancia = null;

        private HelpersIA()
        {

        }

        public static HelpersIA GetInstance()
        {
            if (_instancia == null)
            {
                _instancia = new HelpersIA();
            }
            return _instancia;
        }
        #endregion

        public Respuesta<ResultadoIADTO> AnalizarTestConIA(List<ResultTestDTO> resultTest)
        {
            // 1. Envolvemos TODO en un try-catch
            try
            {
                if (string.IsNullOrWhiteSpace(OpenAIApiKey))
                {
                    return new Respuesta<ResultadoIADTO>
                    {
                        Estado = false,
                        Data = null,
                        Mensaje = "Modelo inteligente no disponible verifique la configuracion."
                    };
                }

                var listaCarreras = NPregunta.GetInstance().ListaCarreras();
                //var lista = listaCarreras.Data;

                var prompt = ConstruirPrompt(listaCarreras.Data, resultTest);

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "Eres un orientador vocacional profesional." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.3,
                    // Exigimos JSON puro por parte de la API
                    response_format = new { type = "json_object" }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAIApiKey);

                    // Llamada SIN async
                    var response = http
                        .PostAsync("https://api.openai.com/v1/chat/completions", content)
                        .GetAwaiter()
                        .GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        // Opcional: Leer el error exacto que devuelve OpenAI para saber por qué falló
                        var errorDetails = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        return new Respuesta<ResultadoIADTO>
                        {
                            Estado = false,
                            Data = null,
                            Mensaje = "Error al comunicarse con el modelo inteligente: " + errorDetails
                        };
                    }

                    var responseJson = response
                        .Content
                        .ReadAsStringAsync()
                        .GetAwaiter()
                        .GetResult();

                    var resultado = ExtraerResultado(responseJson);

                    return new Respuesta<ResultadoIADTO>
                    {
                        Estado = true,
                        Data = resultado,
                        Mensaje = "Recomendación generada correctamente"
                    };
                }
            }
            catch (Exception ex)
            {
                // CORRECCIÓN 3: Si ExtraerResultado falla, o si no hay internet, caemos aquí
                return new Respuesta<ResultadoIADTO>
                {
                    Estado = false,
                    Data = null,
                    Mensaje = "Ocurrió un error inesperado al generar la recomendación: " + ex.Message
                };
            }
        }

        private string ConstruirPrompt(List<ECarrera> carreras, List<ResultTestDTO> resultTest)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Analiza las siguientes respuestas del estudiante y devuelve:");
            sb.AppendLine();

            sb.AppendLine("=== LISTA DE LAS CARRERAS DISPONIBLES QUE PUEDES SELECCIONAR ===");
            foreach (var c in carreras)
            {
                // Se le envía explícitamente el ID para que lo devuelva intacto
                sb.AppendLine($"- [ID: {c.IdCarrera}] Carrera: {c.Nombre}");
                sb.AppendLine();
            }

            sb.AppendLine("=== RESPUESTAS DEL ESTUDIANTE ===");
            foreach (var r in resultTest)
            {
                sb.AppendLine($"Pregunta: {r.Pregunta}");
                sb.AppendLine($"Respuesta: {r.RespuestaTest}");
                sb.AppendLine();
            }

            sb.AppendLine("=== INSTRUCCIONES ESTRICTAS ===");
            sb.AppendLine("1. Genera un 'ObservacionGeneralIA' (máximo 2 párrafos) explicando las cualidades y el resultado.");
            sb.AppendLine("2. Selecciona a las mejores carreras (máximo 3) que tengan afinidad REAL.");
            sb.AppendLine("3. Asigna a cada carrera un 'Puntaje' numérico de 0 a 100.");
            sb.AppendLine("4. Escribe una breve 'Justificacion' de por qué seleccionaste a esa carrera.");
            sb.AppendLine("5. Devuelve el 'IdCarrera' numérico exacto que te proporcioné.");
            sb.AppendLine("6. Asigna un campo 'Orden' (1, 2, o 3) donde 1 es la carrera más recomendada (mayor puntaje) y 3 la menos recomendada de las seleccionadas.");
            sb.AppendLine();

            // Forzamos la estructura JSON exacta
            sb.AppendLine("Responde ÚNICAMENTE con un JSON válido que siga EXACTAMENTE la siguiente estructura, sin texto adicional ni explicaciones fuera del JSON:");
            sb.AppendLine("{");
            sb.AppendLine("  \"ObservacionGeneralIA\": \"texto de la observacion aquí\",");
            sb.AppendLine("  \"Recomendaciones\": [");
            sb.AppendLine("    {");
            sb.AppendLine("      \"IdCarrera\": 0,");
            sb.AppendLine("      \"Carrera\": \"nombre de la carrera\",");
            sb.AppendLine("      \"Puntaje\": 95.5,");
            sb.AppendLine("      \"Justificacion\": \"razón de la elección\",");
            sb.AppendLine("      \"Orden\": 1");
            sb.AppendLine("    }");
            sb.AppendLine("  ]");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private ResultadoIADTO ExtraerResultado(string jsonResponse)
        {
            try
            {
                // 1. Navegamos por el JSON crudo que devuelve la API de OpenAI
                using (var doc = JsonDocument.Parse(jsonResponse))
                {
                    var contentString = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    // 2. Limpieza de seguridad: Quitamos las etiquetas Markdown si la IA las incluyó
                    if (!string.IsNullOrWhiteSpace(contentString))
                    {
                        contentString = contentString.Trim();

                        // Si empieza con ```json y termina con ```, se lo quitamos
                        if (contentString.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
                        {
                            contentString = contentString.Substring(7); // Quitamos el ```json
                        }
                        else if (contentString.StartsWith("```"))
                        {
                            contentString = contentString.Substring(3); // Quitamos solo los ``` si no dice json
                        }

                        if (contentString.EndsWith("```"))
                        {
                            contentString = contentString.Substring(0, contentString.Length - 3); // Quitamos los ``` del final
                        }

                        contentString = contentString.Trim(); // Limpiamos espacios residuales
                    }

                    // 3. Opciones de deserialización flexibles
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignora si la IA mandó "observacionGeneralIA" en minúscula
                    };

                    // 4. Convertimos el string JSON limpio a nuestra clase DTO
                    return JsonSerializer.Deserialize<ResultadoIADTO>(contentString, options);
                }
            }
            catch (Exception ex)
            {
                // Lanzamos una excepción controlada para que tu catch superior la capture
                throw new Exception($"Error al procesar el JSON de la IA. Detalle: {ex.Message}");
            }
        }
    }
}