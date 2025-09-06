using System.Collections.Generic;
using OpenTK.Mathematics;

namespace modelov2.Models
{
    public class Poligono
    {
        public List<Vertice> Vertices = new();
        public Vector3 Centroide()
        {
            if (Vertices.Count == 0) return Vector3.Zero;
            var acc = Vector3.Zero;
            foreach (var v in Vertices) acc += v.Pos;
            return acc / Vertices.Count;
        }
    }
}
