using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace SkyImagesAnalyzerLibraries
{
    public class Graph
    {
        public List<PointD> verticesList;
        public List<GraphEdge> edgesList;
    }


    public struct GraphEdge
    {
        public PointD pt1;
        public PointD pt2;

        public double length
        {
            get { return pt1.Distance(pt2); }
        }

        public double weight;
    }


}
