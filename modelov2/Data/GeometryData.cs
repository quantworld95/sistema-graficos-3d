using System.Text.Json;
using System.Text.Json.Serialization;

namespace modelov2.Data
{
    public class GeometryData
    {
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = "";

        [JsonPropertyName("parametros")]
        public Dictionary<string, object> Parametros { get; set; } = new();

        // Métodos de conveniencia para obtener parámetros
        public float GetFloat(string key, float defaultValue = 0f)
        {
            if (Parametros.TryGetValue(key, out var value) && value is JsonElement element)
            {
                return element.GetSingle();
            }
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (Parametros.TryGetValue(key, out var value) && value is JsonElement element)
            {
                return element.GetInt32();
            }
            return defaultValue;
        }
    }
}
