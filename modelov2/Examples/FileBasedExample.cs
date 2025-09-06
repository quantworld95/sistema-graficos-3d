using modelov2.Data;
using modelov2.Examples;
using modelov2.Graphics;
using modelov2.Models;
using OpenTK.Mathematics;

namespace modelov2.Examples
{
    public class FileBasedExample : IExample
    {
        public string Name => _sceneData.Nombre;
        
        private readonly SceneData _sceneData;
        private readonly Dictionary<string, Parte> _partes = new();

        public FileBasedExample(SceneData sceneData)
        {
            _sceneData = sceneData;
        }

        public void Setup(Objeto objeto)
        {
            foreach (var parteData in _sceneData.Partes)
            {
                var parte = CrearParteDesdeData(parteData);
                _partes[parteData.Nombre] = parte;
                objeto.Partes.Add(parte);
            }

            foreach (var p in objeto.Partes) { 
                p.RecalcularCentroMasa(); 
                ShaderManager.SubirParteAGPU(p); 
            }
            objeto.RecalcularCentroMasa();
        }

        public void Update(Objeto objeto, float tiempo)
        {
            // Aplicar animación global si existe
            if (!string.IsNullOrEmpty(_sceneData.AnimacionGlobal.Tipo))
            {
                AplicarAnimacionGlobal(objeto, tiempo);
            }

            // Aplicar animaciones individuales
            foreach (var parteData in _sceneData.Partes)
            {
                if (_partes.TryGetValue(parteData.Nombre, out var parte))
                {
                    AplicarAnimacionParte(parte, parteData, tiempo);
                }
            }
        }

        public void SetColors(Parte parte)
        {
            var parteData = _sceneData.Partes.FirstOrDefault(p => p.Nombre == parte.Nombre);
            if (parteData != null)
            {
                var color = new Vector3(parteData.Color.R, parteData.Color.G, parteData.Color.B);
                ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", color);
            }
        }

        private Parte CrearParteDesdeData(ParteData parteData)
        {
            var geometria = parteData.Geometria;
            
            return geometria.Tipo.ToLower() switch
            {
                "cubo" => GeometryGenerator.CrearCubo(parteData.Nombre, geometria.GetFloat("lado", 1.0f)),
                "rectangulo" => GeometryGenerator.CrearRectangulo(parteData.Nombre, 
                    geometria.GetFloat("ancho", 1.0f), 
                    geometria.GetFloat("alto", 1.0f), 
                    geometria.GetFloat("profundidad", 1.0f)),
                "cilindro" => GeometryGenerator.CrearCilindro(parteData.Nombre, 
                    geometria.GetFloat("radio", 0.5f), 
                    geometria.GetFloat("altura", 1.0f), 
                    geometria.GetInt("segmentos", 8)),
                "esfera" => GeometryGenerator.CrearEsfera(parteData.Nombre, 
                    geometria.GetFloat("radio", 0.5f), 
                    geometria.GetInt("segmentos", 8)),
                _ => throw new NotSupportedException($"Tipo de geometría no soportado: {geometria.Tipo}")
            };
        }

        private void AplicarAnimacionGlobal(Objeto objeto, float tiempo)
        {
            var animacion = _sceneData.AnimacionGlobal;
            
            switch (animacion.Tipo.ToLower())
            {
                case "traslacion":
                    var pos = new Vector3(
                        animacion.Offset.X * tiempo,
                        animacion.Offset.Y * tiempo,
                        animacion.Offset.Z * tiempo
                    );
                    objeto.Global = Matrix4.CreateTranslation(pos);
                    break;
                case "rotacion":
                    var rotacion = CrearRotacion(animacion.Eje, animacion.Velocidad * tiempo);
                    objeto.Global = rotacion;
                    break;
            }
        }

        private void AplicarAnimacionParte(Parte parte, ParteData parteData, float tiempo)
        {
            var animacion = parteData.Animacion;
            Matrix4 transformacion = Matrix4.Identity;

            // Aplicar posición inicial (siempre, incluso sin animación)
            if (animacion.Offset.X != 0 || animacion.Offset.Y != 0 || animacion.Offset.Z != 0)
            {
                transformacion *= Matrix4.CreateTranslation(new Vector3(animacion.Offset.X, animacion.Offset.Y, animacion.Offset.Z));
            }

            // Aplicar animación solo si hay tipo definido
            if (!string.IsNullOrEmpty(animacion.Tipo))
            {
                switch (animacion.Tipo.ToLower())
                {
                    case "rotacion":
                        transformacion *= CrearRotacion(animacion.Eje, animacion.Velocidad * tiempo);
                        break;
                    case "orbita":
                        if (!string.IsNullOrEmpty(animacion.DependeDe) && _partes.TryGetValue(animacion.DependeDe, out var partePadre))
                        {
                            // Implementar órbita alrededor de otra parte
                            var angulo = animacion.Velocidad * tiempo;
                            var radio = animacion.Radio;
                            var posicionOrbital = new Vector3(
                                MathF.Cos(angulo) * radio,
                                0,
                                MathF.Sin(angulo) * radio
                            );
                            transformacion = Matrix4.CreateTranslation(posicionOrbital) * transformacion;
                        }
                        break;
                }
            }

            parte.Local = transformacion;
        }

        private Matrix4 CrearRotacion(string eje, float angulo)
        {
            return eje.ToUpper() switch
            {
                "X" => Matrix4.CreateRotationX(angulo),
                "Y" => Matrix4.CreateRotationY(angulo),
                "Z" => Matrix4.CreateRotationZ(angulo),
                _ => Matrix4.CreateRotationY(angulo)
            };
        }
    }
}
