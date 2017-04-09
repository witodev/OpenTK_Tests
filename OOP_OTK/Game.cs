using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OOP_OTK
{
    class Game : GameWindow
    {
        public List<Volume> objects = new List<Volume>();
        public Camera cam = new Camera();
        private ShaderProgram SP;

        public Game()
            : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            Title = "Hello OpenTK!";
            GL.ClearColor(Color.Black);
            GL.PointSize(5f);
            
            // Move camera away from origin
            cam.Position += new Vector3(0f, 0f, 3f);

            var cube1 = new Cube();
            objects.Add(cube1);

            var cube2 = new ColorCube();
            cube2.Position += new Vector3(1f, 0, 0);
            objects.Add(cube2);

            var cube3 = new Cube();
            cube3.Position += new Vector3(0.5f, 0.5f, 0);
            objects.Add(cube3);

            SP = new ShaderProgram("vs.glsl", "fs.glsl", true);

            foreach(var obj in objects)
            {
                obj.BuffvPosition = SP.GetBuffer("vPosition");
                obj.AttrvPosition = SP.GetAttribute("vPosition");
                obj.AttrvColor = SP.GetAttribute("vColor");
                obj.BuffvColor = SP.GetBuffer("vColor");
                obj.ProgramID = SP.ProgramID;
                obj.Uniformmodelview = SP.GetUniform("modelview");
                obj.Init();
            }

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            foreach (Volume v in objects)
            {
                v.CalculateModelMatrix();
                v.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            SP.EnableVertexAttribArrays();

            foreach (var obj in objects)
            {
                obj.BindData();
                obj.Draw();
            }

            SP.DisableVertexAttribArrays();

            GL.Flush();
            SwapBuffers();
        }
    }
}