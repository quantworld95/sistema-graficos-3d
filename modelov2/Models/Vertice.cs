using OpenTK.Mathematics;

namespace modelov2.Models
{
    public struct Vertice
    {
        public Vector3 Pos; // local a la Parte
        public Vertice(float x, float y, float z) { Pos = new Vector3(x, y, z); }
    }
}
