using modelov2.Models;
using modelov2.Graphics;
using OpenTK.Mathematics;

namespace modelov2.Examples
{
    public class SistemaSolarExample : IExample
    {
        public string Name => "Sistema Solar - Tierra Orbitando el Sol";

        private Parte _sol = null!;
        private Parte _tierra = null!;
        private Parte _luna = null!;

        public void Setup(Objeto objeto)
        {
            // Crear las partes del sistema solar
            _sol = GeometryGenerator.CrearEsfera("Sol", 1.0f, 12);
            _tierra = GeometryGenerator.CrearEsfera("Tierra", 0.4f, 10);
            _luna = GeometryGenerator.CrearEsfera("Luna", 0.15f, 8);

            objeto.Partes.Add(_sol);
            objeto.Partes.Add(_tierra);
            objeto.Partes.Add(_luna);

            foreach (var p in objeto.Partes) { 
                p.RecalcularCentroMasa(); 
                ShaderManager.SubirParteAGPU(p); 
            }
            objeto.RecalcularCentroMasa();
        }

        public void Update(Objeto objeto, float tiempo)
        {
            // 1) El sistema solar completo se mueve ligeramente (opcional)
            var pos = new Vector3(tiempo * 0.2f, 0, 0);
            objeto.Global = Matrix4.CreateTranslation(pos);

            // 2) Sol: rota sobre su propio eje
            float rotacionSol = tiempo * 0.5f;
            _sol.Local = Matrix4.CreateRotationY(rotacionSol);

            // 3) Tierra: orbita alrededor del sol
            float radioOrbitaTierra = 3.0f;
            float velocidadOrbitaTierra = tiempo * 0.3f; // Velocidad orbital
            float rotacionTierra = tiempo * 2.0f; // Rotación sobre su propio eje
            
            // Posición orbital de la Tierra
            var posicionTierra = new Vector3(
                MathF.Cos(velocidadOrbitaTierra) * radioOrbitaTierra,
                0,
                MathF.Sin(velocidadOrbitaTierra) * radioOrbitaTierra
            );
            
            // Tierra: rotación sobre su eje + posición orbital
            _tierra.Local = Matrix4.CreateTranslation(posicionTierra) * 
                           Matrix4.CreateRotationY(rotacionTierra);

            // 4) Luna: orbita alrededor de la Tierra
            float radioOrbitaLuna = 0.8f;
            float velocidadOrbitaLuna = tiempo * 1.5f; // Velocidad orbital más rápida
            
            // Posición orbital de la Luna relativa a la Tierra
            var posicionLunaRelativa = new Vector3(
                MathF.Cos(velocidadOrbitaLuna) * radioOrbitaLuna,
                0,
                MathF.Sin(velocidadOrbitaLuna) * radioOrbitaLuna
            );
            
            // Luna: posición orbital de la Tierra + posición orbital de la Luna
            var posicionLunaFinal = posicionTierra + posicionLunaRelativa;
            _luna.Local = Matrix4.CreateTranslation(posicionLunaFinal);
        }

        public void SetColors(Parte parte)
        {
            // Colores específicos para el sistema solar
            switch (parte.Nombre)
            {
                case "Sol":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(1.0f, 0.8f, 0.2f)); // Amarillo dorado
                    break;
                case "Tierra":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.2f, 0.4f, 0.8f)); // Azul
                    break;
                case "Luna":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.7f, 0.7f, 0.7f)); // Gris claro
                    break;
                default:
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.5f, 0.5f, 0.5f)); // Gris
                    break;
            }
        }
    }
}
