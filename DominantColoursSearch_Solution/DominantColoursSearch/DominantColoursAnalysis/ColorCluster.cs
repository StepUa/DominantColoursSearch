using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;

namespace DominantColoursSearch.DominantColoursAnalysis
{
    public struct ColorCluster
    {
        public MCvScalar Color { get; set; }
        public MCvScalar NewColor { get; set; }
        public int Count { get; set; }
    }
}
