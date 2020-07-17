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
using MathPractice.Model.Manager;

namespace MathPractice.Model.Lagacy
{
    public class Beam_Lagacy
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
        private double rotation;
        public double Rotation
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

        private gPoint top;
        public gPoint Top { get { return top; } }

        private gPoint bottom;
        public gPoint Bottom { get { return bottom; } }

        private gPoint center;
        public gPoint Center { get { return center; } }

        private Beam_Lagacy attachedBeam;
        public Beam_Lagacy AttachedBeam { get { return attachedBeam; } }

        private List<Beam_Lagacy> calcTargetBeams;
        public List<Beam_Lagacy> CalcTargetBeams { get { return calcTargetBeams; } }

        // Figures 
        private List<FigureDrawer_Lagacy> cuttingFigures;
        public List<FigureDrawer_Lagacy> CuttingFigures { get { return cuttingFigures; } }

        private List<FigureDrawer_Lagacy> expandFigures;
        public List<FigureDrawer_Lagacy> ExpandFigures { get { return expandFigures; } }

        // Draw Variable
        private vdDocument document;
        private vdLine line_lt2rt;
        private vdLine line_rt2rb;
        private vdLine line_rb2lb;
        private vdLine line_lb2lt;
        private vdLine line_left2right;

        public Color DrawColor;
        public Color CenterColor;
        public Beam_Lagacy(gPoint centerPos, vdDocument document, int width, int height, Color drawColor,
            Color centerColor, double rot, string beamName)
        {
            beamWidth = width;
            beamHeight = height;
            center = centerPos;
            rotation = rot;
            this.beamName = beamName;
            DrawColor = drawColor;
            CenterColor = centerColor;
            this.document = document;
            attachedBeam = null;

            calcTargetBeams = new List<Beam_Lagacy>();
            cuttingFigures = new List<FigureDrawer_Lagacy>();
            expandFigures = new List<FigureDrawer_Lagacy>();

            ConvertRotation();
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

            if (attachedBeam != null)
            {
                Vector2 temp = new Vector2(BeamWidth * Math.Cos(Globals.DegreesToRadians(rotation)),
                    BeamWidth * Math.Sin(Globals.DegreesToRadians(rotation)));
                temp *= 0.5f;
                center = MathSupporter.Instance.GetExpandPoint(attachedBeam.center, temp);
            }

            leftTop = new gPoint(center.x - halfWidth, center.y + halfHeight);
            rightTop = new gPoint(center.x + halfWidth, center.y + halfHeight);
            rightBottom = new gPoint(center.x + halfWidth, center.y - halfHeight);
            leftBottom = new gPoint(center.x - halfWidth, center.y - halfHeight);
            left = new gPoint(center.x - halfWidth, center.y);
            right = new gPoint(center.x + halfWidth, center.y);
            top = new gPoint(center.x, center.y + halfHeight);
            bottom = new gPoint(center.x, center.y - halfHeight);

            leftTop = MathSupporter.Instance.GetRotatedPoint(rotation, leftTop, center);
            rightTop = MathSupporter.Instance.GetRotatedPoint(rotation, rightTop, center);
            rightBottom = MathSupporter.Instance.GetRotatedPoint(rotation, rightBottom, center);
            leftBottom = MathSupporter.Instance.GetRotatedPoint(rotation, leftBottom, center);
            left = MathSupporter.Instance.GetRotatedPoint(rotation, left, center);
            right = MathSupporter.Instance.GetRotatedPoint(rotation, right, center);
            top = MathSupporter.Instance.GetRotatedPoint(rotation, top, center);
            bottom = MathSupporter.Instance.GetRotatedPoint(rotation, bottom, center);

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
        public void DrawBeam()
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

            for (int i = 0; i < cuttingFigures.Count; ++i)
            {
                cuttingFigures[i].DrawCuttingRect();
            }
            for (int i = 0; i < expandFigures.Count; ++i)
            {
                expandFigures[i].DrawCuttingRect();
            }

            document.Redraw(true);
        }
        public void RemoveAllFigures()
        {
            int cutCount = cuttingFigures.Count;
            for (int i = 0; i < cutCount; ++i)
            {
                cuttingFigures[0].Release();
                cuttingFigures.RemoveAt(0);
            }

            int exCount = expandFigures.Count;
            for (int i = 0; i < exCount; ++i)
            {
                expandFigures[0].Release();
                expandFigures.RemoveAt(0);
            }
        }
        public void RemoveAllCalcTarget()
        {
            calcTargetBeams.RemoveRange(0, calcTargetBeams.Count);
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
        public void RotateBeam(double degreeAngle)
        {
            rotation += degreeAngle;
            ConvertRotation();
            CalcRectData();
        }
        private void ConvertRotation()
        {
            if (rotation > 360)
                rotation = rotation - 360;
            else if (rotation < 0)
                rotation = rotation + 360;
        }
        public void SetRotation(int degreeAngle)
        {
            rotation = degreeAngle;
            CalcRectData();
        }
        public void AttachToBeam(Beam_Lagacy verBeam)
        {
            attachedBeam = verBeam;
            CalcRectData();
        }
        #endregion
    }
}
