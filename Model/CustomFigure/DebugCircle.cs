﻿using System;
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

namespace MathPractice.Model.CustomFigure
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
            debugText = VectorDrawConfigure.Instance.AddTextToDocument(new gPoint(circlePoint.x, circlePoint.y + 10), caption);
        }

        // Event Handler
        public void UpdatePoint(gPoint newPoint)
        {
            debugCircle.Center = newPoint;
            debugCircle.Update();

            debugText.TextString = $"{caption} : {newPoint}";
            debugText.InsertionPoint = newPoint;
            debugText.Update();

            document.Redraw(true);
        }
    }
}
