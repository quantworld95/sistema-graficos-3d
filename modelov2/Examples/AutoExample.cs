using modelov2.Models;
using modelov2.Graphics;
using OpenTK.Mathematics;

namespace modelov2.Examples
{
    public class AutoExample : IExample
    {
        public string Name => "Auto en Movimiento";

        private Parte _chasis = null!;
        private Parte _ruedaDelanteraIzq = null!;
        private Parte _ruedaDelanteraDer = null!;
        private Parte _ruedaTraseraIzq = null!;
        private Parte _ruedaTraseraDer = null!;
        private Parte _cabina = null!;

        public void Setup(Objeto objeto)
        {
            // Crear las partes del auto con diferentes geometrías
            _chasis = GeometryGenerator.CrearRectangulo("Chasis", 3.0f, 0.3f, 1.5f);
            _cabina = GeometryGenerator.CrearRectangulo("Cabina", 1.5f, 0.8f, 1.2f);
            
            // Ruedas como cilindros
            _ruedaDelanteraIzq = GeometryGenerator.CrearCilindro("RuedaDelanteraIzq", 0.3f, 0.2f, 8);
            _ruedaDelanteraDer = GeometryGenerator.CrearCilindro("RuedaDelanteraDer", 0.3f, 0.2f, 8);
            _ruedaTraseraIzq = GeometryGenerator.CrearCilindro("RuedaTraseraIzq", 0.3f, 0.2f, 8);
            _ruedaTraseraDer = GeometryGenerator.CrearCilindro("RuedaTraseraDer", 0.3f, 0.2f, 8);

            objeto.Partes.Add(_chasis);
            objeto.Partes.Add(_cabina);
            objeto.Partes.Add(_ruedaDelanteraIzq);
            objeto.Partes.Add(_ruedaDelanteraDer);
            objeto.Partes.Add(_ruedaTraseraIzq);
            objeto.Partes.Add(_ruedaTraseraDer);

            foreach (var p in objeto.Partes) { 
                p.RecalcularCentroMasa(); 
                ShaderManager.SubirParteAGPU(p); 
            }
            objeto.RecalcularCentroMasa();
        }

        public void Update(Objeto objeto, float tiempo)
        {
            // 1) Movimiento lineal del auto completo
            var pos = new Vector3(tiempo * 3.0f, 0, 0); // Velocidad del auto
            objeto.Global = Matrix4.CreateTranslation(pos);

            // 2) Chasis: estático respecto al auto
            _chasis.Local = Matrix4.Identity;

            // 3) Cabina: sobre el chasis
            var offsetCabina = new Vector3(0, 0.3f, 0);
            _cabina.Local = Matrix4.CreateTranslation(offsetCabina);

            // 4) Ruedas: rotan mientras el auto se mueve
            float velocidadRuedas = tiempo * 10.0f; // Rotación de las ruedas
            
            // Rueda delantera izquierda
            var offsetRuedaDelIzq = new Vector3(1.0f, -0.3f, 0.6f);
            _ruedaDelanteraIzq.Local = Matrix4.CreateTranslation(offsetRuedaDelIzq) * 
                                      Matrix4.CreateRotationZ(velocidadRuedas);

            // Rueda delantera derecha
            var offsetRuedaDelDer = new Vector3(1.0f, -0.3f, -0.6f);
            _ruedaDelanteraDer.Local = Matrix4.CreateTranslation(offsetRuedaDelDer) * 
                                      Matrix4.CreateRotationZ(velocidadRuedas);

            // Rueda trasera izquierda
            var offsetRuedaTrasIzq = new Vector3(-1.0f, -0.3f, 0.6f);
            _ruedaTraseraIzq.Local = Matrix4.CreateTranslation(offsetRuedaTrasIzq) * 
                                    Matrix4.CreateRotationZ(velocidadRuedas);

            // Rueda trasera derecha
            var offsetRuedaTrasDer = new Vector3(-1.0f, -0.3f, -0.6f);
            _ruedaTraseraDer.Local = Matrix4.CreateTranslation(offsetRuedaTrasDer) * 
                                    Matrix4.CreateRotationZ(velocidadRuedas);
        }

        public void SetColors(Parte parte)
        {
            // Colores específicos para el auto
            switch (parte.Nombre)
            {
                case "Chasis":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.2f, 0.2f, 0.2f)); // Negro
                    break;
                case "Cabina":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.8f, 0.1f, 0.1f)); // Rojo
                    break;
                case "RuedaDelanteraIzq":
                case "RuedaDelanteraDer":
                case "RuedaTraseraIzq":
                case "RuedaTraseraDer":
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.1f, 0.1f, 0.1f)); // Negro oscuro
                    break;
                default:
                    ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.5f, 0.5f, 0.5f)); // Gris
                    break;
            }
        }
    }
}
