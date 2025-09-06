using System.Text.Json;
using System.Text.Json.Serialization;

namespace modelov2.Data
{
    public class ColorData
    {
        [JsonPropertyName("r")]
        public float R { get; set; } = 0.5f;

        [JsonPropertyName("g")]
        public float G { get; set; } = 0.5f;

        [JsonPropertyName("b")]
        public float B { get; set; } = 0.5f;

        public ColorData() { }

        public ColorData(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
