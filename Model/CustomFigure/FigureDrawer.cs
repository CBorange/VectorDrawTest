using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Render;
using MathPractice.Model.Manager;
using System.Drawing;

namespace MathPractice.Model.CustomFigure
{
    public class FigureDrawer
    {
        private gPoint[] points;
        public gPoint[] Points
        {
            get { return points; }
        }
        private vdLine[] lines;
        public vdLine[] Lines
        {
            get { return lines; }
        }
        private vdDocument document;
        public vdDocument Document
        {
            get { return document; }
        }
        private Color figureColor;
        public Color FigureColor
        {
            get { return figureColor; }
        }
        private float figureWidth;
        public float FigureWidth
        {
            get { return figureWidth; }
        }
        public FigureDrawer(gPoint[] points, vdDocument document, Color figureColor, float figureWidth)
        {
            this.points = points;
            this.document = document;
            this.figureColor = figureColor;
            this.figureWidth = figureWidth;
            InitLines();
        }
        private void InitLines()
        {
            lines = new vdLine[points.Length];
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = new vdLine();
                lines[i].SetUnRegisterDocument(document);

                int nextPointIDX = i + 1;
                if (nextPointIDX > lines.Length - 1)
                    nextPointIDX = 0;

                lines[i].StartPoint = points[i];
                lines[i].PenWidth = figureWidth;
                lines[i].EndPoint = points[nextPointIDX];
                lines[i].PenColor.SystemColor = figureColor;
            }
        }
        public void DrawLines(vdRender render)
        {
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i].Update();
                lines[i].Draw(render);
            }
        }
    }
}
