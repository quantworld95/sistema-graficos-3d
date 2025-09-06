using System.Collections.Generic;
using OpenTK.Mathematics;

namespace modelov2.Models
{
    public class Objeto
    {
        public string Nombre = "";
        public List<Parte> Partes = new();
        public Vector3 CentroMasa = Vector3.Zero;
        public Matrix4 Global = Matrix4.Identity; // TRS global respecto al mundo

        public void RecalcularCentroMasa()
        {
            if (Partes.Count == 0) { CentroMasa = Vector3.Zero; return; }
            var acc = Vector3.Zero;
            foreach (var p in Partes) acc += p.CentroMasa;
            CentroMasa = acc / Partes.Count;
        }
    }
}
