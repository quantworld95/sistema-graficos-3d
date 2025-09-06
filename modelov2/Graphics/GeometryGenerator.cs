using System.Collections.Generic;
using modelov2.Models;
using OpenTK.Mathematics;

namespace modelov2.Graphics
{
    public static class GeometryGenerator
    {
        // Crea una Parte cúbica genérica usando polígonos (6 caras, cada cara = polígonos de 4 vértices CCW)
        public static Parte CrearCubo(string nombre, float lado)
        {
            float h = lado * 0.5f;

            var V = new[]
            {
                new Vertice(-h,-h,-h), new Vertice(+h,-h,-h),
                new Vertice(+h,+h,-h), new Vertice(-h,+h,-h),
                new Vertice(-h,-h,+h), new Vertice(+h,-h,+h),
                new Vertice(+h,+h,+h), new Vertice(-h,+h,+h),
            };

            int[][] caras = new[]
            {
                new[]{0,1,2,3}, // -Z (frente)
                new[]{4,7,6,5}, // +Z (atrás) - orden invertido para CCW
                new[]{0,3,7,4}, // -X (izquierda) - orden invertido para CCW
                new[]{1,5,6,2}, // +X (derecha)
                new[]{3,2,6,7}, // +Y (arriba)
                new[]{0,4,5,1}, // -Y (abajo) - orden invertido para CCW
            };

            var parte = new Parte { Nombre = nombre };
            foreach (var c in caras)
            {
                var poly = new Poligono();
                foreach (var idx in c) poly.Vertices.Add(V[idx]);
                parte.Poligonos.Add(poly);
            }
            parte.RecalcularCentroMasa();
            return parte;
        }

        // Crea una Parte rectangular (cubo con dimensiones diferentes)
        public static Parte CrearRectangulo(string nombre, float ancho, float alto, float profundidad)
        {
            float w = ancho * 0.5f;
            float h = alto * 0.5f;
            float d = profundidad * 0.5f;

            var V = new[]
            {
                new Vertice(-w,-h,-d), new Vertice(+w,-h,-d),
                new Vertice(+w,+h,-d), new Vertice(-w,+h,-d),
                new Vertice(-w,-h,+d), new Vertice(+w,-h,+d),
                new Vertice(+w,+h,+d), new Vertice(-w,+h,+d),
            };

            int[][] caras = new[]
            {
                new[]{0,1,2,3}, // -Z (frente)
                new[]{4,7,6,5}, // +Z (atrás)
                new[]{0,3,7,4}, // -X (izquierda)
                new[]{1,5,6,2}, // +X (derecha)
                new[]{3,2,6,7}, // +Y (arriba)
                new[]{0,4,5,1}, // -Y (abajo)
            };

            var parte = new Parte { Nombre = nombre };
            foreach (var c in caras)
            {
                var poly = new Poligono();
                foreach (var idx in c) poly.Vertices.Add(V[idx]);
                parte.Poligonos.Add(poly);
            }
            parte.RecalcularCentroMasa();
            return parte;
        }

        // Crea una Parte cilíndrica (aproximación con polígonos)
        public static Parte CrearCilindro(string nombre, float radio, float altura, int segmentos = 8)
        {
            var parte = new Parte { Nombre = nombre };
            
            // Crear vértices del cilindro
            var vertices = new List<Vertice>();
            
            // Vértices de la base inferior
            for (int i = 0; i < segmentos; i++)
            {
                float angle = (float)(2.0 * Math.PI * i / segmentos);
                float x = radio * (float)Math.Cos(angle);
                float z = radio * (float)Math.Sin(angle);
                vertices.Add(new Vertice(x, -altura * 0.5f, z));
            }
            
            // Vértices de la base superior
            for (int i = 0; i < segmentos; i++)
            {
                float angle = (float)(2.0 * Math.PI * i / segmentos);
                float x = radio * (float)Math.Cos(angle);
                float z = radio * (float)Math.Sin(angle);
                vertices.Add(new Vertice(x, altura * 0.5f, z));
            }
            
            // Centro de las bases
            vertices.Add(new Vertice(0, -altura * 0.5f, 0)); // Centro base inferior
            vertices.Add(new Vertice(0, altura * 0.5f, 0));  // Centro base superior
            
            // Crear polígonos
            int centroInferior = vertices.Count - 2;
            int centroSuperior = vertices.Count - 1;
            
            // Base inferior
            for (int i = 0; i < segmentos; i++)
            {
                var poly = new Poligono();
                poly.Vertices.Add(vertices[centroInferior]);
                poly.Vertices.Add(vertices[i]);
                poly.Vertices.Add(vertices[(i + 1) % segmentos]);
                parte.Poligonos.Add(poly);
            }
            
            // Base superior
            for (int i = 0; i < segmentos; i++)
            {
                var poly = new Poligono();
                poly.Vertices.Add(vertices[centroSuperior]);
                poly.Vertices.Add(vertices[segmentos + (i + 1) % segmentos]);
                poly.Vertices.Add(vertices[segmentos + i]);
                parte.Poligonos.Add(poly);
            }
            
            // Lados del cilindro
            for (int i = 0; i < segmentos; i++)
            {
                int next = (i + 1) % segmentos;
                var poly = new Poligono();
                poly.Vertices.Add(vertices[i]);
                poly.Vertices.Add(vertices[segmentos + i]);
                poly.Vertices.Add(vertices[segmentos + next]);
                poly.Vertices.Add(vertices[next]);
                parte.Poligonos.Add(poly);
            }
            
            parte.RecalcularCentroMasa();
            return parte;
        }

        // Crea una Parte esférica (aproximación con polígonos)
        public static Parte CrearEsfera(string nombre, float radio, int segmentos = 8)
        {
            var parte = new Parte { Nombre = nombre };
            var vertices = new List<Vertice>();
            
            // Generar vértices de la esfera
            for (int i = 0; i <= segmentos; i++)
            {
                float lat = (float)(Math.PI * i / segmentos - Math.PI / 2);
                for (int j = 0; j <= segmentos; j++)
                {
                    float lon = (float)(2.0 * Math.PI * j / segmentos);
                    float x = radio * (float)(Math.Cos(lat) * Math.Cos(lon));
                    float y = radio * (float)Math.Sin(lat);
                    float z = radio * (float)(Math.Cos(lat) * Math.Sin(lon));
                    vertices.Add(new Vertice(x, y, z));
                }
            }
            
            // Crear polígonos
            for (int i = 0; i < segmentos; i++)
            {
                for (int j = 0; j < segmentos; j++)
                {
                    int current = i * (segmentos + 1) + j;
                    int next = current + segmentos + 1;
                    
                    var poly = new Poligono();
                    poly.Vertices.Add(vertices[current]);
                    poly.Vertices.Add(vertices[next]);
                    poly.Vertices.Add(vertices[next + 1]);
                    poly.Vertices.Add(vertices[current + 1]);
                    parte.Poligonos.Add(poly);
                }
            }
            
            parte.RecalcularCentroMasa();
            return parte;
        }

        // Crea una Parte personalizada desde una lista de vértices y caras
        public static Parte CrearPartePersonalizada(string nombre, Vertice[] vertices, int[][] caras)
        {
            var parte = new Parte { Nombre = nombre };
            
            foreach (var cara in caras)
            {
                var poly = new Poligono();
                foreach (var indice in cara)
                {
                    if (indice >= 0 && indice < vertices.Length)
                        poly.Vertices.Add(vertices[indice]);
                }
                if (poly.Vertices.Count >= 3) // Solo agregar si tiene al menos 3 vértices
                    parte.Poligonos.Add(poly);
            }
            
            parte.RecalcularCentroMasa();
            return parte;
        }
    }
}
