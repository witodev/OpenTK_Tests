using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOP_OTK
{
    public abstract class Volume
    {
        int ibo_elements;
        public uint BuffvPosition;
        public int AttrvPosition;
        public int AttrvColor;
        public uint BuffvColor;
        public int ProgramID;
        public int Uniformmodelview;
        
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Matrix4 ModelMatrix { get; set; }
        public Matrix4 ViewProjectionMatrix { get; set; }
        public Matrix4 ModelViewProjectionMatrix;

        public Volume()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;

            ModelMatrix = Matrix4.Identity;
            ViewProjectionMatrix = Matrix4.Identity;
            ModelViewProjectionMatrix = Matrix4.Identity;

        }

        public virtual void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        public abstract Vector3[] Vertices();
        public abstract Vector3[] Colors();
        public abstract int[] Indices();

        public virtual void Init()
        {
            GL.GenBuffers(1, out ibo_elements);
        }
        public virtual void Draw()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);

            GL.UniformMatrix4(Uniformmodelview, false, ref ModelViewProjectionMatrix);

            GL.DrawElements(PrimitiveType.Triangles, Indices().Length, DrawElementsType.UnsignedInt, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);   
        }

        public virtual void BindData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, BuffvPosition);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Vertices().Length * Vector3.SizeInBytes), Vertices(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttrvPosition, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (AttrvColor != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, BuffvColor);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Colors().Length * Vector3.SizeInBytes), Colors(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(AttrvColor, 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            GL.UseProgram(ProgramID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indices().Length * sizeof(int)), Indices(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);        
        }

        /// <summary>
        /// old
        /// </summary>

        //public Vector3 Position = Vector3.Zero;
        //public Vector3 Rotation = Vector3.Zero;
        //public Vector3 Scale = Vector3.One;

        //public virtual int VertCount { get; set; }
        //public virtual int IndiceCount { get; set; }
        //public virtual int ColorDataCount { get; set; }

        //public Matrix4 ModelMatrix = Matrix4.Identity;
        //public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        //public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

    //    public abstract Vector3[] GetVerts();
    //    public abstract int[] GetIndices(int offset = 0);
    //    public abstract Vector3[] GetColorData();
    //    public abstract void CalculateModelMatrix();

    //    public bool IsTextured = false;
    //    public int TextureID;
    //    public int TextureCoordsCount;
    //    public abstract Vector2[] GetTextureCoords();
    }
}
