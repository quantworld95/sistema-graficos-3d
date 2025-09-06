using System.Text.Json;
using System.Text.Json.Serialization;

namespace modelov2.Data
{
    public class SceneData
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = "";

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = "";

        [JsonPropertyName("partes")]
        public List<ParteData> Partes { get; set; } = new();

        [JsonPropertyName("animacionGlobal")]
        public AnimationData AnimacionGlobal { get; set; } = new();

        [JsonPropertyName("camara")]
        public CameraData Camara { get; set; } = new();
    }

    public class CameraData
    {
        [JsonPropertyName("posicion")]
        public Vector3Data Posicion { get; set; } = new(8, 6, 8);

        [JsonPropertyName("objetivo")]
        public Vector3Data Objetivo { get; set; } = new();

        [JsonPropertyName("fov")]
        public float FOV { get; set; } = 60f;

        [JsonPropertyName("near")]
        public float Near { get; set; } = 0.1f;

        [JsonPropertyName("far")]
        public float Far { get; set; } = 100f;
    }
}
