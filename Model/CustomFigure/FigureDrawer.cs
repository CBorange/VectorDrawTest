using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spatial;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;

namespace MathPractice.Model.CustomFigure
{
    public class FigureDrawer
    {
        private List<vdLine> drawLines;
        public List<vdLine> DrawLines
        {
            get { return drawLines; }
        }

        public FigureDrawer()
        {

        }
    }
}
