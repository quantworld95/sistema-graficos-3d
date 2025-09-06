using System.Text.Json;
using modelov2.Data;
using modelov2.Examples;
using modelov2.Graphics;
using modelov2.Models;
using OpenTK.Mathematics;

namespace modelov2.Serialization
{
    public static class SceneLoader
    {
        public static IExample LoadFromFile(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException($"No se encontró el archivo: {rutaArchivo}");
            }

            var json = File.ReadAllText(rutaArchivo);
            var sceneData = JsonSerializer.Deserialize<SceneData>(json);

            if (sceneData == null)
            {
                throw new InvalidOperationException("Error al deserializar el archivo de escena");
            }

            return new FileBasedExample(sceneData);
        }

        public static void SaveToFile(SceneData sceneData, string rutaArchivo)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var json = JsonSerializer.Serialize(sceneData, options);
            File.WriteAllText(rutaArchivo, json);
        }
    }
}
