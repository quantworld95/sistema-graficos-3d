using modelov2.Models;
using modelov2.Graphics;
using OpenTK.Mathematics;

namespace modelov2.Examples
{
    public class BrazoMecanicoExample : IExample
    {
        public string Name => "Brazo Mecánico";

        private Parte _base = null!;
        private Parte _brazoInferior = null!;
        private Parte _brazoSuperior = null!;
        private Parte _pinza = null!;

        public void Setup(Objeto objeto)
        {
            // Crear las partes del brazo mecánico con diferentes geometrías
            _base = GeometryGenerator.CrearCilindro("Base", 0.5f, 0.3f, 8);
            _brazoInferior = GeometryGenerator.CrearRectangulo("BrazoInferior", 0.2f, 1.0f, 0.2f);
            _brazoSuperior = GeometryGenerator.CrearRectangulo("BrazoSuperior", 0.15f, 0.8f, 0.15f);
            _pinza = GeometryGenerator.CrearEsfera("Pinza", 0.1f, 6);

            objeto.Partes.Add(_base);
            objeto.Partes.Add(_brazoInferior);
            objeto.Partes.Add(_brazoSuperior);
            objeto.Partes.Add(_pinza);

            foreach (var p in objeto.Partes) { 
                p.RecalcularCentroMasa(); 
                ShaderManager.SubirParteAGPU(p); 
            }
            objeto.RecalcularCentroMasa();
        }

        public void Update(Objeto objeto, float tiempo)
        {
            // 1) Traslación global del objeto (opcional)
            var pos = new Vector3(tiempo * 0.5f, 0, 0);
            objeto.Global = Matrix4.CreateTranslation(pos);

            // 2) Base: estática
            _base.Local = Matrix4.Identity;

            // 3) Brazo Inferior: rota sobre su base
            float angulo1 = tiempo * 0.5f; // Rotación lenta
            _brazoInferior.Local = Matrix4.CreateRotationY(angulo1);

            // 4) Brazo Superior: posicionado al final del brazo inferior y rota
            float angulo2 = tiempo * 0.8f; // Rotación más rápida
            var offsetBrazoSuperior = new Vector3(0, 0.5f, 0); // Posición relativa
            _brazoSuperior.Local = Matrix4.CreateTranslation(offsetBrazoSuperior) * Matrix4.CreateRotationY(angulo2);

            // 5) Pinza: posicionada al final del brazo superior y rota
            float angulo3 = tiempo * 1.2f; // Rotación más rápida
            var offsetPinza = new Vector3(0, 0.4f, 0); // Posición relativa
            _pinza.Local = Matrix4.CreateTranslation(offsetPinza) * Matrix4.CreateRotationY(angulo3);
        }

        public void SetColors(Parte parte)
        {
            // Colores específicos para el brazo mecánico
            switch (parte.Nombre)
            {
                case "Base":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.5f, 0.5f, 0.5f)); // Gris
                    break;
                case "BrazoInferior":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.8f, 0.2f, 0.2f)); // Rojo
                    break;
                case "BrazoSuperior":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.2f, 0.8f, 0.2f)); // Verde
                    break;
                case "Pinza":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.2f, 0.2f, 0.8f)); // Azul
                    break;
                default:
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.7f, 0.7f, 0.7f)); // Gris claro
                    break;
            }
        }
    }
}
