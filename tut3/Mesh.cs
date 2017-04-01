using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.IO;

using OpenTK.Graphics.OpenGL;
namespace tut3
{
    class Mesh : Volume
    {
        private Dictionary<int, Element> Elems;
        private List<int> Indices;
        private Dictionary<int, Node> Nodes;
        private Vector3[] Verts;

        public Mesh(string filePath)
        {
            if (File.Exists(filePath))
            {
                var cdb = new CDB();
                cdb.readFile(filePath);
                Nodes = cdb.getNodes();
                Elems = cdb.getElements();

                Verts = new Vector3[Nodes.Keys.Count];

                var ids = new Dictionary<int, int>();
                for (int i = 0; i < Nodes.Keys.Count; i++)
                {
                    var id = Nodes.ElementAt(i);
                    ids.Add(id.Key, i);

                    var n = id.Value;
                    Verts[i] = new Vector3((float)n.x, (float)n.y, (float)n.z);
                }

                Indices = new List<int>();
                for (int e = 0; e < Elems.Keys.Count; e++)
                {
                    var elem = Elems.ElementAt(e).Value;
                    var nodes = elem.nodesIds;
                    var i = nodes[0];
                    var j = nodes[1];
                    var k = nodes[2];
                    //var m = nodes[4];

                    Indices.Add(ids[i]);
                    Indices.Add(ids[j]);
                    Indices.Add(ids[k]);

                    //Indices.Add(ids[i]);
                    //Indices.Add(ids[j]);
                    //Indices.Add(ids[m]);
                    
                    //Indices.Add(ids[j]);
                    //Indices.Add(ids[k]);
                    //Indices.Add(ids[m]);

                    //Indices.Add(ids[i]);
                    //Indices.Add(ids[k]);
                    //Indices.Add(ids[m]);
                }

                //var verts = new List<Vector3>();
                //Indices = new List<int>();
                //for (int i = 0; i < Elems.Keys.Count; i++)
                //{
                //    var e = Elems.ElementAt(i).Value;
                //    var nodes = e.nodes;
                //    for (int j = 0; j < nodes.Count; j++)
                //    {
                //        verts.Add(new Vector3((float)nodes[j].x, (float)nodes[j].y, (float)nodes[j].z));
                //        Indices.Add(verts.Count-1);
                //    }
                //}
                //Verts = verts.ToArray();

                VertCount = Verts.Length;
                IndiceCount = Indices.Count;
                ColorDataCount = Verts.Length;
            }
            else
            {
                throw new FileNotFoundException(filePath);
            }
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        public override Vector3[] GetColorData()
        {
            var colors = new Vector3[Verts.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Vector3(1f, 0f, 1f);
            }
            return colors;
        }

        public override int[] GetIndices(int offset = 0)
        {
            var inds = Indices.ToArray();

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
            return Verts;
        }
    }
}
