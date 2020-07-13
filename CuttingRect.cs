﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spatial;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;

namespace MathPractice
{
    public class CuttingRect
    {
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                vdFigure.VisibilityEnum visibility;
                if (visible)
                    visibility = vdFigure.VisibilityEnum.Visible;
                else
                    visibility = vdFigure.VisibilityEnum.Invisible;

                if (lines != null)
                {
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        lines[i].visibility = visibility;
                        lines[i].Update();
                    }
                    document.Redraw(true);
                }
            }
        }
        private gPoint[] points;
        public gPoint[] Points
        {
            get { return points; }
        }
        private vdCircle[] circles;
        public vdCircle[] Circles
        {
            get { return circles; }
        }
        private vdLine[] lines;
        public vdLine[] Lines
        {
            get { return lines; }
        }

        // Draw Variable
        private vdDocument document;

        public CuttingRect(gPoint[] points, vdDocument document)
        {
            this.points = points;
            this.document = document;

            ClearFigures();
            Visible = true;
        }
        public CuttingRect(vdDocument document)
        {
            this.document = document;
            Visible = false;
        }
        private void ClearFigures()
        {
            // Line
            if (lines != null)
            {
                for (int i = 0; i < lines.Length; ++i)
                    document.Model.Entities.RemoveItem(lines[i]);
            }
            lines = new vdLine[(points.Length / 2) + 1];
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = new vdLine();
                AddLineToDocument(lines[i]);
            }

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i].StartPoint = points[i];
                lines[i].EndPoint = points[i + 1];
                lines[i].Update();
            }

            // Circle
            if (circles != null)
            {
                for (int i = 0; i < circles.Length; ++i)
                    document.Model.Entities.RemoveItem(circles[i]);
            }
            circles = new vdCircle[points.Length];
            for (int i = 0; i < circles.Length; ++i)
            {
                circles[i] = new vdCircle();
                AddCircleToDocument(circles[i]);
            }
            for (int i = 0; i < circles.Length; ++i)
            {
                circles[i].Radius = 3;
                circles[i].Center = points[i];
                circles[i].Update();
            }
            
        }
        private void AddLineToDocument(vdLine newLine)
        {
            newLine.SetUnRegisterDocument(document);
            newLine.setDocumentDefaults();
            document.Model.Entities.AddItem(newLine);
        }
        private void AddCircleToDocument(vdCircle newCircle)
        {
            newCircle.SetUnRegisterDocument(document);
            newCircle.setDocumentDefaults();
            document.Model.Entities.AddItem(newCircle);
        }
        public void SetPoints(gPoint[] points)
        {
            this.points = points;
            ClearFigures();
        }
        public void DrawCuttingRect()
        {
            if (!visible)
                return;

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i].PenColor.SystemColor = Color.Green;
                lines[i].Update();
            }
            for (int i = 0; i < circles.Length; ++i)
                circles[i].Update();
            document.Redraw(true);
        }
    }
}
