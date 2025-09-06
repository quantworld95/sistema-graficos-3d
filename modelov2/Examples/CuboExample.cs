using modelov2.Models;
using modelov2.Graphics;
using OpenTK.Mathematics;

namespace modelov2.Examples
{
    public class CuboExample : IExample
    {
        public string Name => "Ejemplo de Cubos";
        
        private Parte _cuboGrande = null!;
        private Parte _cuboPeq = null!;

        public void Setup(Objeto objeto)
        {
            // Construir partes genéricas (cualquier polígono sirve; aquí uso cubos por simplicidad):
            _cuboGrande = GeometryGenerator.CrearCubo("Grande", 2.0f);
            _cuboPeq = GeometryGenerator.CrearCubo("Pequenio", 0.6f);

            objeto.Partes.Add(_cuboGrande);
            objeto.Partes.Add(_cuboPeq);

            foreach (var p in objeto.Partes) { 
                p.RecalcularCentroMasa(); 
                ShaderManager.SubirParteAGPU(p); 
            }
            objeto.RecalcularCentroMasa();
        }

        public void Update(Objeto objeto, float tiempo)
        {
            // 1) Traslación lineal del Objeto (válido para cualquier entidad)
            // El objeto completo (ambos cubos) se mueve como una unidad
            var pos = new Vector3(tiempo * 2.0f, 0, 0); // v = (2.0,0,0) - velocidad aumentada para mejor visibilidad
            objeto.Global = Matrix4.CreateTranslation(pos);

            // 2) Parte grande: en el centro del Objeto
            _cuboGrande.Local = Matrix4.Identity;

            // 3) Parte pequeña: simplemente sobre el cubo grande (sin rotación)
            // Posicionar el cubo pequeño sobre el cubo grande
            // El cubo grande tiene lado 2.0f, así que su altura es de -1.0f a +1.0f
            // El cubo pequeño tiene lado 0.6f, así que su altura es de -0.3f a +0.3f
            // Para que esté sobre el grande: 1.0f (top del grande) + 0.3f (radio del pequeño) = 1.3f
            var offset = new Vector3(0, 1.3f, 0);
            _cuboPeq.Local = Matrix4.CreateTranslation(offset);
        }

        public void SetColors(Parte parte)
        {
            // Colores específicos para el ejemplo de cubos
            if (parte.Nombre == "Grande")
                ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.8f, 0.3f, 0.3f)); // Rojo
            else
                ShaderManager.SetVec3(ShaderManager.CurrentProgram, "uBaseColor", new Vector3(0.3f, 0.3f, 0.8f)); // Azul
        }
    }
}
