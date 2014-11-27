using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using System.ComponentModel;

namespace SkyImagesAnalyzerLibraries
{
    class InteractiveImage
    {
        public Size representingPictureBoxSize = new Size(800, 600);
        public int scaleOrder = 0;
        public Image<Bgr, byte> wholeImage = null;
        public Image<Bgr, byte> partialImage = null;
        public Rectangle rectContainingPartialImage = new Rectangle(0, 0, 800, 600);

    }
}
