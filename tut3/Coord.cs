using System;
using OpenTK;

namespace tut3
{
    class Coord : Volume
    {
        public Coord()
        {
            VertCount = 9;
            IndiceCount = 9;
            ColorDataCount = 9;
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        public override Vector3[] GetColorData()
        {
            return new Vector3[] {
                new Vector3(1f, 0f, 0f),
                new Vector3( 1f, 0f, 0f),
                new Vector3( 1f, 0f, 0f),

                new Vector3( 0f, 1f, 0f),
                new Vector3( 0f, 1f, 0f),
                new Vector3( 0f, 1f, 0f),

                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 0f, 1f)                
            };
        }

        public override int[] GetIndices(int offset = 0)
        {
            int[] inds = new int[] {
                0, 1, 2,
                3,4,5,
                6,7,8
            };

            if (offset != 0)
            {
                for (int i = 0; i < inds.Length; i++)
                {
                    inds[i] += offset;
                }
            }

            return inds;
        }

        public override Vector3[] GetVerts()
        {
            var s = 1.0f;
            var r = 3.0f;
            return new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(s, 0, 0),
                new Vector3(0, s/r, 0),

                new Vector3(0, 0, 0),
                new Vector3(0, s, 0),
                new Vector3(0, 0, s/r),

                new Vector3(0, 0, 0),
                new Vector3(0, 0, s),
                new Vector3(s/r, 0, 0)
            };
        }
    }
}