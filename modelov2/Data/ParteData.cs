using System.Text.Json;
using System.Text.Json.Serialization;

namespace modelov2.Data
{
    public class ParteData
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = "";

        [JsonPropertyName("geometria")]
        public GeometryData Geometria { get; set; } = new();

        [JsonPropertyName("color")]
        public ColorData Color { get; set; } = new();

        [JsonPropertyName("animacion")]
        public AnimationData Animacion { get; set; } = new();

        [JsonPropertyName("posicionInicial")]
        public Vector3Data PosicionInicial { get; set; } = new();

        [JsonPropertyName("rotacionInicial")]
        public Vector3Data RotacionInicial { get; set; } = new();

        [JsonPropertyName("escalaInicial")]
        public Vector3Data EscalaInicial { get; set; } = new(1, 1, 1);
    }
}
