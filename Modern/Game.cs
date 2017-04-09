using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern
{
    class Game : GameWindow
    {
        Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        string activeShader = "normal";
        
        public Game()
            : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {

        }

        void initProgram()
        {
            GL.GenBuffers(1, out ibo_elements);

            // Load shaders from file
            shaders.Add("default", new ShaderProgram("vs.glsl", "fs.glsl", true));
            shaders.Add("textured", new ShaderProgram("vs_tex.glsl", "fs_tex.glsl", true));
            shaders.Add("normal", new ShaderProgram("vs_norm.glsl", "fs_norm.glsl", true));
            shaders.Add("lit", new ShaderProgram("vs_lit.glsl", "fs_lit.glsl", true));

            activeShader = "normal";

            loadMaterials("opentk.mtl");

            // Create our objects
            TexturedCube tc = new TexturedCube();
            tc.TextureID = textures[materials["opentk1"].DiffuseMap];
            tc.CalculateNormals();
            tc.Material = materials["opentk1"];
            objects.Add(tc);

            TexturedCube tc2 = new TexturedCube();
            tc2.Position += new Vector3(1f, 1f, 1f);
            tc2.TextureID = textures[materials["opentk2"].DiffuseMap];
            tc2.CalculateNormals();
            tc2.Material = materials["opentk2"];
            objects.Add(tc2);

            // Move camera away from origin
            cam.Position += new Vector3(0f, 0f, 3f);

            textures.Add("earth.png", loadImage("earth.png"));
            ObjVolume earth = ObjVolume.LoadFromFile("earth.obj");
            earth.TextureID = textures["earth.png"];
            earth.Position += new Vector3(1f, 1f, -2f);
            earth.Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5);
            objects.Add(earth);
        }
    }
}
