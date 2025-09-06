using System;
using modelov2.Models;
using modelov2.Graphics;
using modelov2.Examples;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace modelov2.Graphics
{
    public class DemoWindow : GameWindow
    {
        // Escena
        Objeto _objeto = new Objeto { Nombre = "Sistema" };
        IExample _currentExample = null!;

        // Render
        int _prog;
        Matrix4 _V, _P;
        float _t;

        public DemoWindow(GameWindowSettings gws, NativeWindowSettings nws, IExample example) : base(gws, nws) 
        {
            _currentExample = example;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            // Temporalmente deshabilitamos cull face para ver todas las caras
            // GL.Enable(EnableCap.CullFace);
            // GL.CullFace(TriangleFace.Back);

            // Shaders
            _prog = ShaderManager.CompileProgram(VS, FS);

            // Cámara - mejor ángulo para ver los cubos
            var eye = new Vector3(8, 6, 8);
            var center = Vector3.Zero;
            _V = Matrix4.LookAt(eye, center, Vector3.UnitY);
            _P = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f),
                                                      ClientSize.X / (float)ClientSize.Y, 0.1f, 100f);

            // Configurar ejemplo actual
            _currentExample.Setup(_objeto);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            _t += (float)args.Time;

            // Actualizar ejemplo actual
            _currentExample.Update(_objeto, _t);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.ClearColor(0.08f, 0.09f, 0.11f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_prog);
            ShaderManager.SetMat4(_prog, "uV", _V);
            ShaderManager.SetMat4(_prog, "uP", _P);
            ShaderManager.SetVec3(_prog, "uLightDir", Vector3.Normalize(new Vector3(0.6f, 1.0f, 0.3f)));
            ShaderManager.SetVec3(_prog, "uBaseColor", new Vector3(0.85f, 0.85f, 0.90f));

            // Dibujar todas las partes del objeto (independiente del ejemplo)
            foreach (var parte in _objeto.Partes)
            {
                var M = _objeto.Global * parte.Local;
                ShaderManager.SetMat4(_prog, "uM", M);
                
                // Colores específicos del ejemplo actual
                _currentExample.SetColors(parte);
                
                GL.BindVertexArray(parte.VAO);
                GL.DrawElements(PrimitiveType.Triangles, parte.IndexCount, DrawElementsType.UnsignedInt, 0);
            }

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.DeleteProgram(_prog);
            foreach (var p in _objeto.Partes)
            {
                if (p.VAO != 0) GL.DeleteVertexArray(p.VAO);
                if (p.VBO != 0) GL.DeleteBuffer(p.VBO);
                if (p.EBO != 0) GL.DeleteBuffer(p.EBO);
            }
        }

        /* ---------------------- Shaders ---------------------- */

        const string VS = @"
#version 330 core
layout(location=0) in vec3 aPos;
layout(location=1) in vec3 aNor;
uniform mat4 uM, uV, uP;
out vec3 vN;
out vec3 vWPos;
void main(){
    vec4 wpos = uM * vec4(aPos,1.0);
    gl_Position = uP * uV * wpos;
    // normal en mundo (aprox: sin escala no uniforme)
    vN = mat3(transpose(inverse(uM))) * aNor;
    vWPos = wpos.xyz;
}";

        const string FS = @"
#version 330 core
in vec3 vN;
in vec3 vWPos;
out vec4 FragColor;
uniform vec3 uLightDir;
uniform vec3 uBaseColor;
void main(){
    vec3 N = normalize(vN);
    float diff = max(dot(N, normalize(uLightDir)), 0.0);
    vec3 color = uBaseColor * (0.2 + 0.8 * diff);
    FragColor = vec4(color, 1.0);
}";
    }
}
