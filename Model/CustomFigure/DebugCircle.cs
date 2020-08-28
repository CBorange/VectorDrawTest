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
using VectordrawTest.Model.Manager;

namespace VectordrawTest.Model.CustomFigure
{
    public class DebugCircle
    {
        private vdDocument document;
        private vdCircle debugCircle;
        private vdText debugText;
        private string caption;

        public DebugCircle(vdDocument document, gPoint point, double radius, string text)
        {
            this.document = document;
            caption = text;
            InitDebugFigures(point, radius, text);
        }
        private void InitDebugFigures(gPoint circlePoint, double radius, string caption)
        {
            debugCircle = VectorDrawConfigure.Instance.AddCircleToDocument(circlePoint, radius);
            debugText = VectorDrawConfigure.Instance.AddTextToDocument(new gPoint(circlePoint.x, circlePoint.y), caption);
        }

        // Event Handler
        public void UpdateCircle(gPoint newPoint, string text)
        {
            debugCircle.Center = newPoint;
            debugCircle.Update();

            debugText.TextString = $"{caption}: {text}";
            debugText.InsertionPoint = newPoint;
            debugText.Update();

            document.Redraw(true);
        }
    }
}
