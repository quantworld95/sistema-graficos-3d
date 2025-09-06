using System.Collections.Generic;
using OpenTK.Mathematics;

namespace modelov2.Models
{
    public class Parte
    {
        public string Nombre = "";
        public List<Poligono> Poligonos = new();
        public Vector3 CentroMasa = Vector3.Zero;
        public Matrix4 Local = Matrix4.Identity; // TRS local respecto al Objeto

        // GPU buffers (generales para cualquier parte)
        public int VAO, VBO, EBO, IndexCount;

        public void RecalcularCentroMasa()
        {
            if (Poligonos.Count == 0) { CentroMasa = Vector3.Zero; return; }
            var acc = Vector3.Zero;
            foreach (var p in Poligonos) acc += p.Centroide();
            CentroMasa = acc / Poligonos.Count;
        }
    }
}
