using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tut3
{
    class Element
    {
        public int id { get; set; }
        public List<int> nodesIds { get; set; }
        public List<Node> nodes { get; set; }        
    }
}
