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
using VectorDraw.Render;
using MathPractice.Model.Manager;

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

        private List<Beam> calcTargetBeams;
        public List<Beam> CalcTargetBeams { get { return calcTargetBeams; } }

        // Figures 
        private List<FigureDrawer> cuttingFigures;
        public List<FigureDrawer> CuttingFigures { get { return cuttingFigures; } }

        private List<FigureDrawer> expandFigures;
        public List<FigureDrawer> ExpandFigures { get { return expandFigures; } }

        public Beam(gPoint point, vdDocument document, double width, double height,double rotation, string beamName)
        {
            center = point;
            beamWidth = width;
            beamHeight = height;
            halfWidth = width * 0.5f;
            halfHeight = height * 0.5f;
            this.rotation = rotation;
            this.beamName = beamName;
            this.document = document;

            calcTargetBeams = new List<Beam>();
            cuttingFigures = new List<FigureDrawer>();
            expandFigures = new List<FigureDrawer>();

            InitLines();
            RefreshRectData();
            AddBarLineToDocument(baseLine);
        }
        private void InitLines()
        {
            baseLine = new vdLine();

            line_lt2rt = new vdLine();
            line_lt2rt.SetUnRegisterDocument(document);

            line_rt2rb = new vdLine();
            line_rt2rb.SetUnRegisterDocument(document);

            line_rb2lb = new vdLine();
            line_rb2lb.SetUnRegisterDocument(document);

            line_lb2lt = new vdLine();
            line_lb2lt.SetUnRegisterDocument(document);
        }
        private void RefreshRectData()
        {
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

            baseLine.StartPoint = left;
            baseLine.EndPoint = right;

            line_lt2rt.StartPoint = leftTop;
            line_lt2rt.EndPoint = rightTop;

            line_rt2rb.StartPoint = rightTop;
            line_rt2rb.EndPoint = rightBottom;

            line_rb2lb.StartPoint = rightBottom;
            line_rb2lb.EndPoint = leftBottom;

            line_lb2lt.StartPoint = leftBottom;
            line_lb2lt.EndPoint = leftTop;
        }
        private void AddBarLineToDocument(vdLine newLine)
        {
            newLine.SetUnRegisterDocument(document);
            newLine.setDocumentDefaults();
            document.Model.Entities.AddItem(newLine);
        }
        public void UpdateBaseLine()
        {
            RefreshRectData();
            baseLine.Update();
        }
        public void DrawOutLines(vdRender render)
        {
            line_lt2rt.Update();
            line_rt2rb.Update();
            line_rb2lb.Update();
            line_lb2lt.Update();

            line_lt2rt.Draw(render);
            line_rt2rb.Draw(render);
            line_rb2lb.Draw(render);
            line_lb2lt.Draw(render);
        }
        public void RemoveAllFigures()
        {
            cuttingFigures.Clear();
            expandFigures.Clear();
        }
        public void RemoveAllCalcTarget()
        {
            calcTargetBeams.Clear();
        }
    }
}
