using System.Text.Json;
using System.Text.Json.Serialization;

namespace modelov2.Data
{
    public class AnimationData
    {
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = "";

        [JsonPropertyName("eje")]
        public string Eje { get; set; } = "Y";

        [JsonPropertyName("velocidad")]
        public float Velocidad { get; set; } = 1.0f;

        [JsonPropertyName("radio")]
        public float Radio { get; set; } = 0f;

        [JsonPropertyName("offset")]
        public Vector3Data Offset { get; set; } = new();

        [JsonPropertyName("dependeDe")]
        public string DependeDe { get; set; } = "";
    }

    public class Vector3Data
    {
        [JsonPropertyName("x")]
        public float X { get; set; } = 0f;

        [JsonPropertyName("y")]
        public float Y { get; set; } = 0f;

        [JsonPropertyName("z")]
        public float Z { get; set; } = 0f;

        public Vector3Data() { }

        public Vector3Data(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
