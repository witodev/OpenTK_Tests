
using System;
using System.Collections.Generic;
using System.Linq;

namespace tut3
{
    class CDB
    {
        static IEnumerable<string> Split(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        private Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        private Dictionary<int, Element> elems = new Dictionary<int, Element>();
        private string[] cdb;

        public void readFile(string file)
        {
            if (System.IO.File.Exists(file))
                cdb = System.IO.File.ReadAllLines(file);
            else
                throw new Exception("Plik nie istnieje");
        }
                
        internal Dictionary<int, Node> getNodes()
        {
            if (nodes.Count==0)
            {
                var i = 0;
                var line = cdb[i];
                while(!line.StartsWith("NBLOCK"))
                {
                    line = cdb[++i];
                }
                var vals = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                var NUMFIELD = long.Parse(vals[1]);
                var NDMAX = long.Parse(vals[3]);
                var NDSEL = long.Parse(vals[4]);
                
                for (var j=2; j < NDSEL+2; ++j)
                {
                    line = cdb[j+i];
                    var id = line.Substring(0, 9);
                    var pos = line.Substring(27);
                    var v = Split(pos, 20).Select(double.Parse).ToList();

                    var n = new Node();
                    n.id = int.Parse(id);

                    switch (v.Count)
                    {
                        case 6:
                            n.zx = v[5];
                            goto case 5;
                        case 5:
                            n.yz = v[4];
                            goto case 4;
                        case 4:
                            n.xy = v[3];
                            goto case 3;
                        case 3:
                            n.z = v[2];
                            goto case 2;
                        case 2:
                            n.y = v[1];
                            goto case 1;
                        case 1:
                            n.x = v[0];
                            break;
                    }

                    nodes.Add(n.id, n);
                }
            }
            
            return nodes;
        }

        internal Dictionary<int, Element> getElements()
        {
            if (elems.Count == 0)
            {
                var i = 0;
                var line = cdb[i];
                while (!line.StartsWith("EBLOCK"))
                {
                    line = cdb[++i];
                }
                var vals = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                var NUMFIELD = long.Parse(vals[1]);
                var NDMAX = long.Parse(vals[3]);
                var NDSEL = long.Parse(vals[4]);

                for (var j = 2; j < NDSEL + 2; ++j)
                {
                    line = cdb[j + i];
                    var v = Split(line, 9).Select(int.Parse).ToList();
                    var id = v[10];
                    var ncount = v[8];

                    if (ncount > 8)
                    {
                        var lplus = cdb[j + (++i)];
                        var vplus = Split(lplus, 9).Select(int.Parse).ToList();
                        v.AddRange(vplus);
                    }

                    var e = new Element();
                    e.id = id;
                    e.nodesIds = v.GetRange(11, (int)ncount);

                    e.nodes = makeNodesList(e.nodesIds);

                    elems.Add(id, e);
                }
            }

            return elems;
        }

        private List<Node> makeNodesList(List<int> list)
        {
            var res = new List<Node>();

            for (var i=0; i<list.Count; ++i)
            {
                res.Add(nodes[list[i]]);
            }

            return res;
        }
    }
}
