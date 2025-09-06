using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using modelov2.Models;

namespace modelov2.Graphics
{
    public static class ShaderManager
    {
        public static int CurrentProgram { get; private set; }
        public static int CompileProgram(string vs, string fs)
        {
            int v = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(v, vs); GL.CompileShader(v); CheckShader(v, "VS");

            int f = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(f, fs); GL.CompileShader(f); CheckShader(f, "FS");

            int p = GL.CreateProgram();
            GL.AttachShader(p, v); GL.AttachShader(p, f);
            GL.LinkProgram(p);
            GL.GetProgram(p, GetProgramParameterName.LinkStatus, out int ok);
            if (ok == 0) throw new Exception("Program link error: " + GL.GetProgramInfoLog(p));
            GL.DeleteShader(v); GL.DeleteShader(f);
            CurrentProgram = p;
            return p;
        }

        public static void CheckShader(int s, string label)
        {
            GL.GetShader(s, ShaderParameter.CompileStatus, out int ok);
            if (ok == 0) throw new Exception($"{label} compile error: " + GL.GetShaderInfoLog(s));
        }

        public static void SetMat4(int prog, string name, Matrix4 m)
        {
            int loc = GL.GetUniformLocation(prog, name);
            GL.UniformMatrix4(loc, false, ref m);
        }

        public static void SetVec3(int prog, string name, Vector3 v)
        {
            int loc = GL.GetUniformLocation(prog, name);
            GL.Uniform3(loc, v);
        }

        // Convierte una Parte (lista de Polígonos con N vértices) en buffers GPU triangulados (fan triangulation)
        public static void SubirParteAGPU(Parte parte)
        {
            // Aplanar a posición + normal por vértice y triangulamos polígonos convexos (fan)
            var positions = new List<Vector3>();
            var normals   = new List<Vector3>();
            var indices   = new List<uint>();

            foreach (var poly in parte.Poligonos)
            {
                // Posiciones del polígono
                int baseIndex = positions.Count;
                foreach (var v in poly.Vertices) positions.Add(v.Pos);

                // Normal plana del polígono (si el polígono tiene al menos 3 vértices)
                Vector3 n = Vector3.UnitY;
                if (poly.Vertices.Count >= 3)
                {
                    var a = poly.Vertices[0].Pos;
                    var b = poly.Vertices[1].Pos;
                    var c = poly.Vertices[2].Pos;
                    n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
                }
                for (int i = 0; i < poly.Vertices.Count; ++i) normals.Add(n);

                // Triangulación por "fan" (válida para convexos)
                for (int i = 1; i < poly.Vertices.Count - 1; ++i)
                {
                    indices.Add((uint)(baseIndex + 0));
                    indices.Add((uint)(baseIndex + i));
                    indices.Add((uint)(baseIndex + i + 1));
                }
            }

            parte.IndexCount = indices.Count;

            // Subir a GPU (interleaved: pos(3), nor(3))
            var interleaved = new float[positions.Count * 6];
            for (int i = 0; i < positions.Count; ++i)
            {
                interleaved[i * 6 + 0] = positions[i].X;
                interleaved[i * 6 + 1] = positions[i].Y;
                interleaved[i * 6 + 2] = positions[i].Z;
                interleaved[i * 6 + 3] = normals[i].X;
                interleaved[i * 6 + 4] = normals[i].Y;
                interleaved[i * 6 + 5] = normals[i].Z;
            }

            GL.GenVertexArrays(1, out parte.VAO);
            GL.GenBuffers(1, out parte.VBO);
            GL.GenBuffers(1, out parte.EBO);

            GL.BindVertexArray(parte.VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, parte.VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, interleaved.Length * sizeof(float), interleaved, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, parte.EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);

            int stride = 6 * sizeof(float);
            GL.EnableVertexAttribArray(0); // aPos
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(1); // aNor
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));

            GL.BindVertexArray(0);
        }
    }
}
