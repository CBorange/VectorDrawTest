using System;
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
    public class Beam
    {
        private int beamWidth;
        public int BeamWidth
        {
            get { return beamWidth; }
        }
        private int beamHeight;
        public int BeamHeight
        {
            get { return beamHeight; }
        }
        private string beamName;

        // Rect Point
        private int rotation;
        public int Rotation
        {
            get { return rotation; }
        }
        private gPoint leftTop;
        public gPoint LeftTop { get { return leftTop; } }

        private gPoint rightTop;
        public gPoint RightTop { get { return rightTop; } }

        private gPoint leftBottom;
        public gPoint LeftBottom { get { return leftBottom; } }

        private gPoint rightBottom;
        public gPoint RightBottom { get { return rightBottom; } }

        private gPoint bottom;
        public gPoint Bottom { get { return bottom; } }

        private gPoint top;
        public gPoint Top { get { return top; } }

        private gPoint center;
        public gPoint Center { get { return center; } }

        // Draw Variable
        private vdDocument document;
        private vdLine line_lt2rt;
        private vdLine line_rt2rb;
        private vdLine line_rb2lb;
        private vdLine line_lb2lt;
        private vdLine line_top2bottom;

        public short DrawColorIndex;
        public Beam(gPoint centerPos,vdDocument document, int width, int height, short colorIndex,int rot,
            string beamName)
        {
            beamWidth = width;
            beamHeight = height;
            center = centerPos;
            rotation = rot;
            this.beamName = beamName;
            DrawColorIndex = colorIndex;
            this.document = document;

            InitDrawLine();
            CalcRectData();
        }
        private void InitDrawLine()
        {
            line_lt2rt = new vdLine();
            AddBarLineToDocument(line_lt2rt);

            line_rt2rb = new vdLine();
            AddBarLineToDocument(line_rt2rb);

            line_rb2lb = new vdLine();
            AddBarLineToDocument(line_rb2lb);

            line_lb2lt = new vdLine();
            AddBarLineToDocument(line_lb2lt);

            line_top2bottom = new vdLine();
            AddBarLineToDocument(line_top2bottom);
        }
        private void AddBarLineToDocument(vdLine newLine)
        {
            newLine.SetUnRegisterDocument(document);
            newLine.setDocumentDefaults();
            document.Model.Entities.AddItem(newLine);
        }
        private void CalcRectData()
        {
            int halfWidth = (int)(beamWidth * 0.5f);
            int halfHeight = (int)(beamHeight * 0.5f);
            
            leftTop = new gPoint(center.x - halfWidth, center.y + halfHeight);
            rightTop = new gPoint(center.x + halfWidth, center.y + halfHeight);
            rightBottom = new gPoint(center.x + halfWidth, center.y - halfHeight);
            leftBottom = new gPoint(center.x - halfWidth, center.y - halfHeight);
            top = new gPoint(center.x, center.y + halfHeight);
            bottom = new gPoint(center.x, center.y - halfHeight);

            double[,] rotMatrix = new double[2, 2];
            rotMatrix[0, 0] = Math.Cos(Globals.DegreesToRadians(rotation));
            rotMatrix[0, 1] = -Math.Sin(Globals.DegreesToRadians(rotation));
            rotMatrix[1, 0] = Math.Sin(Globals.DegreesToRadians(rotation));
            rotMatrix[1, 1] = Math.Cos(Globals.DegreesToRadians(rotation));

            leftTop = CalcMatrixMultiply(rotMatrix, leftTop);
            rightTop = CalcMatrixMultiply(rotMatrix, rightTop);
            rightBottom = CalcMatrixMultiply(rotMatrix, rightBottom);
            leftBottom = CalcMatrixMultiply(rotMatrix, leftBottom);
            top = CalcMatrixMultiply(rotMatrix, top);
            bottom = CalcMatrixMultiply(rotMatrix, bottom);

            line_lt2rt.StartPoint = leftTop;
            line_lt2rt.EndPoint = rightTop;

            line_rt2rb.StartPoint = rightTop;
            line_rt2rb.EndPoint = rightBottom;

            line_rb2lb.StartPoint = rightBottom;
            line_rb2lb.EndPoint = leftBottom;

            line_lb2lt.StartPoint = leftBottom;
            line_lb2lt.EndPoint = leftTop;

            line_top2bottom.StartPoint = top;
            line_top2bottom.EndPoint = bottom;
        }
        private gPoint CalcMatrixMultiply(double[,] mat, gPoint vec)
        {
            gPoint result = new gPoint();
            result.x = ((mat[0, 0] * (vec.x - center.x)) + (mat[0, 1] * (vec.y - center.y)) + center.x);
            result.y = ((mat[1, 0] * (vec.x - center.x)) + (mat[1, 1] * (vec.y - center.y)) + center.y);
            return result;
        }
        public void DrawBeam(vdDocument document)
        {
            Debug.WriteLine($"{beamName} End : {line_lt2rt.EndPoint} ");
            line_lt2rt.SetUnRegisterDocument(document);
            line_lt2rt.setDocumentDefaults();
            line_lt2rt.Update();

            line_rt2rb.SetUnRegisterDocument(document);
            line_rt2rb.setDocumentDefaults();
            line_rt2rb.Update();

            line_rb2lb.SetUnRegisterDocument(document);
            line_rb2lb.setDocumentDefaults();
            line_rb2lb.Update();

            line_lb2lt.SetUnRegisterDocument(document);
            line_lb2lt.setDocumentDefaults();
            line_lb2lt.Update();

            line_top2bottom.SetUnRegisterDocument(document);
            line_top2bottom.setDocumentDefaults();
            line_top2bottom.Update();

            document.Redraw(true);
        }


        #region Translate Beam Transform Method
        public void SetPosition(gPoint newPos)
        {
            center = newPos;
            CalcRectData();
        }
        public void Translate(gPoint delta)
        {
            center += delta;
            CalcRectData();
        }
        public void RotateBeam(int degreeAngle)
        {
            rotation += degreeAngle;
            CalcRectData();
        }
        public void SetRotation(int degreeAngle)
        {
            rotation = degreeAngle;
            CalcRectData();
        }
        #endregion
    }
}
