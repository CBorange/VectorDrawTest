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
using System.Diagnostics;

namespace MathPractice.Model.CustomFigure
{
    public class Beam
    {
        private double beamWidth;
        public double BeamWidth
        {
            get { return beamWidth; }
        }
        private double beamHeight;
        public double BeamHeight
        {
            get { return beamHeight; }
        }
        private double halfWidth;
        public double HalfWidth
        {
            get { return halfWidth; }
        }
        private double halfHeight;
        public double HalfHeight
        {
            get { return halfHeight; }
        }
        private string beamName;
        private vdDocument document;
        public vdDocument Document
        {
            get { return document; }
        }

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

        private vdLine baseLine;
        private vdLine line_lt2rt;
        private vdLine line_rt2rb;
        private vdLine line_rb2lb;
        private vdLine line_lb2lt;

        private const double CIRCLE_SIZE = 0.5f;
        private DebugCircle ltCircle;
        private DebugCircle rtCircle;
        private DebugCircle lbCircle;
        private DebugCircle rbCircle;
        private DebugCircle rotCircle;
        private DebugCircle lCircle;
        private DebugCircle rCircle;
        private DebugCircle tCircle;
        private DebugCircle bCircle;

        private Color drawColor;

        private List<Beam> calcTargetBeams;
        public List<Beam> CalcTargetBeams { get { return calcTargetBeams; } }

        // Figures 
        private List<FigureDrawer> cuttingFigures;
        public List<FigureDrawer> CuttingFigures { get { return cuttingFigures; } }

        private List<FigureDrawer> expandFigures;
        public List<FigureDrawer> ExpandFigures { get { return expandFigures; } }

        public Beam(gPoint point, vdDocument document, Color drawColor, double width, double height,double rotation, string beamName)
        {
            this.rotation = rotation;
            this.beamName = beamName;
            this.document = document;
            this.drawColor = drawColor;

            center = point;
            beamWidth = width;
            beamHeight = height;
            halfWidth = width * 0.5f;
            halfHeight = height * 0.5f;

            leftTop = new gPoint();
            rightTop = new gPoint();
            leftBottom = new gPoint();
            rightBottom = new gPoint();
            left = new gPoint();
            right = new gPoint();
            top = new gPoint();
            bottom = new gPoint();

            calcTargetBeams = new List<Beam>();
            cuttingFigures = new List<FigureDrawer>();
            expandFigures = new List<FigureDrawer>();

            InitLines();
            InitDebugCircles();
            VectorDrawConfigure.Instance.AddLineToDocument(baseLine);
        }
        private void InitLines()
        {
            baseLine = new vdLine();
            baseLine.StartPoint = new gPoint(center.x - halfWidth, center.y);
            baseLine.EndPoint = new gPoint(center.x + halfWidth, center.y);
            baseLine.StartPoint = MathSupporter.Instance.GetRotatedPoint(rotation, baseLine.StartPoint, center);
            baseLine.EndPoint = MathSupporter.Instance.GetRotatedPoint(rotation, baseLine.EndPoint, center);

            line_lt2rt = new vdLine();
            line_lt2rt.SetUnRegisterDocument(document);

            line_rt2rb = new vdLine();
            line_rt2rb.SetUnRegisterDocument(document);

            line_rb2lb = new vdLine();
            line_rb2lb.SetUnRegisterDocument(document);

            line_lb2lt = new vdLine();
            line_lb2lt.SetUnRegisterDocument(document);
        }
        private void InitDebugCircles()
        {
            ltCircle = new DebugCircle(document, leftTop, CIRCLE_SIZE, "LT");
            rtCircle = new DebugCircle(document, rightTop, CIRCLE_SIZE, "RT");
            lbCircle = new DebugCircle(document, leftBottom, CIRCLE_SIZE, "LB");
            rbCircle = new DebugCircle(document, rightBottom, CIRCLE_SIZE, "RB");
            rotCircle = new DebugCircle(document, center, CIRCLE_SIZE, "ROT");
            lCircle = new DebugCircle(document, left, CIRCLE_SIZE, "LEFT");
            rCircle = new DebugCircle(document, right, CIRCLE_SIZE, "RIGHT");
            tCircle = new DebugCircle(document, top, CIRCLE_SIZE, "TOP");
            bCircle = new DebugCircle(document, bottom, CIRCLE_SIZE, "BOTTOM");
        }
        private void RefreshRectData()
        {
            // center refresh
            center = MathSupporter.Instance.GetCenterBy2Points(baseLine.EndPoint, baseLine.StartPoint);

            // rotate calc
            Vector baseS2E = new Vector(baseLine.EndPoint.x - baseLine.StartPoint.x, baseLine.EndPoint.y - baseLine.StartPoint.y, 0);
            Vector centerLineUnit = new Vector(1, 0, 0);
            if (baseS2E.x < 0)
                centerLineUnit = new Vector(-1, 0, 0);

            double x = baseS2E.Dot(centerLineUnit);
            rotation = Math.Atan2(baseS2E.y, x);

            rotation = Globals.RadiansToDegrees(rotation);
            if (baseS2E.x < 0 && baseS2E.y >= 0)
                rotation *= -1;
            else if (baseS2E.x < 0 && baseS2E.y <= 0)
                rotation *= -1;

            // calc vertex point
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

            ltCircle.UpdateCircle(leftTop, $"{string.Format("{0:0.00}",leftTop.x)}, {string.Format("{0:0.00}", leftTop.y)}");
            rtCircle.UpdateCircle(rightTop, $"{string.Format("{0:0.00}", rightTop.x)}, {string.Format("{0:0.00}", rightTop.y)}");
            rbCircle.UpdateCircle(rightBottom, $"{string.Format("{0:0.00}", rightBottom.x)}, {string.Format("{0:0.00}", rightBottom.y)}");
            lbCircle.UpdateCircle(leftBottom, $"{string.Format("{0:0.00}", leftBottom.x)}, {string.Format("{0:0.00}", leftBottom.y)}");
            lCircle.UpdateCircle(left, $"{string.Format("{0:0.00}", left.x)}, {string.Format("{0:0.00}", left.y)}");
            rCircle.UpdateCircle(right, $"{string.Format("{0:0.00}", right.x)}, {string.Format("{0:0.00}", right.y)}");
            tCircle.UpdateCircle(top, $"{string.Format("{0:0.00}", top.x)}, {string.Format("{0:0.00}", top.y)}");
            bCircle.UpdateCircle(bottom, $"{string.Format("{0:0.00}", bottom.x)}, {string.Format("{0:0.00}", bottom.y)}");
            rotCircle.UpdateCircle(center, $"{string.Format("{0:0.00}", rotation)}");

            // refresh line vertex
            line_lt2rt.StartPoint = leftTop;
            line_lt2rt.EndPoint = rightTop;

            line_rt2rb.StartPoint = rightTop;
            line_rt2rb.EndPoint = rightBottom;

            line_rb2lb.StartPoint = rightBottom;
            line_rb2lb.EndPoint = leftBottom;

            line_lb2lt.StartPoint = leftBottom;
            line_lb2lt.EndPoint = leftTop;
        }
        
        public void UpdateBaseLine()
        {
            RefreshRectData();
            baseLine.PenColor.SystemColor = drawColor;
            baseLine.Update();
        }
        public void DrawOutLines(vdRender render)
        {
            // Beam OutLines Draw
            line_lt2rt.PenColor.SystemColor = drawColor;
            line_lt2rt.Update();

            line_rt2rb.PenColor.SystemColor = drawColor;
            line_rt2rb.Update();

            line_rb2lb.PenColor.SystemColor = drawColor;
            line_rb2lb.Update();

            line_lb2lt.PenColor.SystemColor = drawColor;
            line_lb2lt.Update();

            line_lt2rt.Draw(render);
            line_rt2rb.Draw(render);
            line_rb2lb.Draw(render);
            line_lb2lt.Draw(render);

            // Cutting Rect Draw
            for (int i = 0; i < cuttingFigures.Count; ++i)
                cuttingFigures[i].DrawLines(render);

            // Expand Rect Draw
            for (int i = 0; i < expandFigures.Count; ++i)
                expandFigures[i].DrawLines(render);

        }
        public void RemoveAllFigures()
        {
            cuttingFigures.Clear();
            expandFigures.Clear();
        }
        public void AddCuttingFigure(List<gPoint> points, Color figureColor)
        {
            cuttingFigures.Add(new FigureDrawer(points.ToArray(), document, figureColor));
        }
        public void AddExpandFigure(List<gPoint> points, Color figureColor)
        {
            expandFigures.Add(new FigureDrawer(points.ToArray(), document, figureColor));
        }

        public void RemoveAllCalcTarget()
        {
            calcTargetBeams.Clear();
        }
    }
}
