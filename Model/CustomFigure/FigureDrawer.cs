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
        private gPoint[] points;
        public gPoint[] Points
        {
            get { return points; }
        }
        private vdLine[] drawLines;
        public vdLine[] DrawLines
        {
            get { return drawLines; }
        }
        private vdDocument document;
        public vdDocument Document
        {
            get { return document; }
        }
        public FigureDrawer(gPoint[] points, vdDocument document)
        {
            this.points = points;
            this.document = document;
            InitLines();
        }
        private void InitLines()
        {
            drawLines = new vdLine[points.Length];
            for (int i = 0; i < drawLines.Length; ++i)
            {
                drawLines[i].SetUnRegisterDocument(document);

                int nextPointIDX = i + 1;
                if (nextPointIDX > drawLines.Length - 1)
                    nextPointIDX = 0;

                drawLines[i].StartPoint = points[i];
                drawLines[i].EndPoint = points[nextPointIDX];
            }
        }
    }
}
