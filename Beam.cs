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

        private gPoint right;
        public gPoint Right { get { return right; } }

        private gPoint left;
        public gPoint Left { get { return left; } }

        private gPoint center;
        public gPoint Center { get { return center; } }

        // Draw Variable
        private vdDocument document;
        private vdLine line_lt2rt;
        private vdLine line_rt2rb;
        private vdLine line_rb2lb;
        private vdLine line_lb2lt;
        private vdLine line_left2right;

        public Color DrawColor;
        public Color CenterColor;
        public Beam(gPoint centerPos, vdDocument document, int width, int height, Color drawColor,
            Color centerColor, int rot, string beamName)
        {
            beamWidth = width;
            beamHeight = height;
            center = centerPos;
            rotation = rot;
            this.beamName = beamName;
            DrawColor = drawColor;
            CenterColor = centerColor;
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

            line_left2right = new vdLine();
            AddBarLineToDocument(line_left2right);
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
            left = new gPoint(center.x - halfWidth, center.y);
            right = new gPoint(center.x + halfWidth, center.y);

            leftTop = MathSupporter.Instance.GetRotatedPoint(rotation, leftTop, center);
            rightTop = MathSupporter.Instance.GetRotatedPoint(rotation, rightTop, center);
            rightBottom = MathSupporter.Instance.GetRotatedPoint(rotation, rightBottom, center);
            leftBottom = MathSupporter.Instance.GetRotatedPoint(rotation, leftBottom, center);
            left = MathSupporter.Instance.GetRotatedPoint(rotation, left, center);
            right = MathSupporter.Instance.GetRotatedPoint(rotation, right, center);

            line_lt2rt.StartPoint = leftTop;
            line_lt2rt.EndPoint = rightTop;

            line_rt2rb.StartPoint = rightTop;
            line_rt2rb.EndPoint = rightBottom;

            line_rb2lb.StartPoint = rightBottom;
            line_rb2lb.EndPoint = leftBottom;

            line_lb2lt.StartPoint = leftBottom;
            line_lb2lt.EndPoint = leftTop;

            line_left2right.StartPoint = left;
            line_left2right.EndPoint = right;
        }
        public void DrawBeam(vdDocument document)
        {
            line_lt2rt.Update();
            line_lt2rt.PenColor.SystemColor = DrawColor;

            line_rt2rb.Update();
            line_rt2rb.PenColor.SystemColor = DrawColor;

            line_rb2lb.Update();
            line_rb2lb.PenColor.SystemColor = DrawColor;

            line_lb2lt.Update();
            line_lb2lt.PenColor.SystemColor = DrawColor;

            line_left2right.Update();
            line_left2right.PenColor.SystemColor = CenterColor;

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
            if (rotation > 360)
                rotation = rotation - 360;
            else if (rotation < 0)
                rotation = rotation + 360;
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
