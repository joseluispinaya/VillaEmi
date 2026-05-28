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
    public class ModeloTestIA
    {
        private static readonly string OpenAIApiKey = ConfigurationManager.AppSettings["OpenAIApiKey"];

        #region "PATRON SINGLETON"
        private static ModeloTestIA _instancia = null;

        private ModeloTestIA()
        {

        }

        public static ModeloTestIA GetInstance()
        {
            if (_instancia == null)
            {
                _instancia = new ModeloTestIA();
            }
            return _instancia;
        }
        #endregion

        public Respuesta<PruebaResultIADTO> AnalizarTestPruebas(List<ResultTestDTO> resultTest)
        {
            // 1. Envolvemos TODO en un try-catch
            try
            {
                if (string.IsNullOrWhiteSpace(OpenAIApiKey))
                {
                    return new Respuesta<PruebaResultIADTO>
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
                        return new Respuesta<PruebaResultIADTO>
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

                    // ¡AQUÍ AGREGAMOS LA VALIDACIÓN!
                    if (!resultado.TestValido)
                    {
                        // Si la IA detectó respuestas sin sentido, cortamos el flujo y retornamos el error
                        return new Respuesta<PruebaResultIADTO>
                        {
                            Estado = false,
                            Data = null,
                            Mensaje = resultado.MensajeError // Ej: "No se pudo generar una recomendación porque las respuestas ingresadas no tienen relación con las preguntas."
                        };
                    }

                    return new Respuesta<PruebaResultIADTO>
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
                return new Respuesta<PruebaResultIADTO>
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

            sb.AppendLine("Actúa como un orientador vocacional profesional y estricto.");
            sb.AppendLine("Analiza las siguientes respuestas del estudiante y devuelve el resultado evaluando primero la coherencia.");
            sb.AppendLine();

            sb.AppendLine("=== LISTA DE LAS CARRERAS DISPONIBLES QUE PUEDES SELECCIONAR ===");
            foreach (var c in carreras)
            {
                sb.AppendLine($"- [ID: {c.IdCarrera}] Carrera: {c.Nombre}");
            }
            sb.AppendLine();

            sb.AppendLine("=== RESPUESTAS DEL ESTUDIANTE ===");
            foreach (var r in resultTest)
            {
                sb.AppendLine($"Pregunta: {r.Pregunta}");
                sb.AppendLine($"Respuesta: {r.RespuestaTest}");
                sb.AppendLine();
            }

            sb.AppendLine("=== INSTRUCCIONES ESTRICTAS ===");
            sb.AppendLine("1. EVALUACIÓN DE COHERENCIA: Primero revisa si las respuestas tienen sentido lógico respecto a las preguntas. Si el estudiante responde con letras al azar, números sin contexto, groserías o frases ilógicas (ej: 'me duele' a una pregunta sobre investigar), marca 'TestValido' como false y llena 'MensajeError' explicando de forma educada que las respuestas no son válidas.");
            sb.AppendLine("2. Si 'TestValido' es false, puedes dejar 'ObservacionGeneralIA' vacío y 'Recomendaciones' como un arreglo vacío [].");
            sb.AppendLine("3. Si el test SI es válido, pon 'TestValido' en true, 'MensajeError' vacío, y genera un 'ObservacionGeneralIA' (máximo 2 párrafos).");
            sb.AppendLine("4. Selecciona a las mejores carreras (máximo 3) que tengan afinidad REAL.");
            sb.AppendLine("5. Asigna a cada carrera un 'Puntaje' numérico de 0 a 100 y una breve 'Justificacion'.");
            sb.AppendLine("6. Devuelve el 'IdCarrera' numérico exacto que te proporcioné y un campo 'Orden' (1, 2, o 3).");
            sb.AppendLine();

            sb.AppendLine("Responde ÚNICAMENTE con un JSON válido que siga EXACTAMENTE la siguiente estructura:");
            sb.AppendLine("{");
            sb.AppendLine("  \"TestValido\": true,");
            sb.AppendLine("  \"MensajeError\": \"\",");
            sb.AppendLine("  \"ObservacionGeneralIA\": \"texto de la observacion aquí (si aplica)\",");
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

        private PruebaResultIADTO ExtraerResultado(string jsonResponse)
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
                    return JsonSerializer.Deserialize<PruebaResultIADTO>(contentString, options);
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