using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;
using VectorDraw.Actions;

using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;

namespace Hicom.BizDraw.Command
{
    public class CmdDimension : ActionBase
    {
        public enum eMethod
        {
            Aligned,
            Vertical,
            Horizontal,
            Angular
        }
        private eMethod Method { get; set; }
        public override vdFigure Entity { get { return Figure; } }
        private vdDimension Figure { get; set; }
        private gPoint First { get; set; }
        private gPoint Second { get; set; }
        private gPoint Intersection { get; set; }
        private vdFigure FirstFigure { get; set; }
        private vdFigure SecondFigure { get; set; }

        // line
        public CmdDimension(gPoint reference, vdLayout layout, eMethod method, DrawDimension.DimensionOptions opt) : base(reference, layout)
        {
            this.Method = method;
            
            this.First = new gPoint(reference);
            
            this.Figure = new vdDimension();
            this.Figure.SetUnRegisterDocument(this.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;
            this.Figure.TextStyle = this.Document.ActiveTextStyle;

            this.Figure.visibility = vdFigure.VisibilityEnum.Invisible;

            this.Figure.TextHeight = opt.TextHeight;
            this.Figure.ArrowSize = opt.ArrowSize;
            this.Figure.ScaleFactor = opt.ScaleFactor;
            this.Figure.DecimalPrecision = opt.DecimalPrecision;
            this.Figure.ExtLineDist1 = opt.ExtLineDist1;
            this.Figure.ExtLineDist2 = opt.ExtLineDist2;
        }

        // Angular
        public CmdDimension(vdFigure fig1, vdFigure fig2, gPoint intersection, vdLayout layout, eMethod method, DrawDimension.DimensionOptions opt) : base(intersection, layout)
        {
            this.Method = method;
            this.Intersection = new gPoint(intersection);
            this.FirstFigure = fig1;
            this.SecondFigure = fig2;
            
            this.Figure = new vdDimension();
            this.Figure.SetUnRegisterDocument(this.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;
            this.Figure.TextStyle = this.Document.ActiveTextStyle;
            this.Figure.visibility = vdFigure.VisibilityEnum.Invisible;

            this.Figure.TextHeight = opt.TextHeight;
            this.Figure.ArrowSize = opt.ArrowSize;
            this.Figure.ScaleFactor = opt.ScaleFactor;
            this.Figure.DecimalPrecision = opt.DecimalPrecision;
        }

        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            this.NewPosition = newPosition;

            switch (this.Method)
            {
                case eMethod.Aligned:
                    if (this.First != null && this.Second != null)
                    {
                        this.Figure.DefPoint1 = this.First;
                        this.Figure.DefPoint2 = this.Second;
                        this.Figure.LinePosition = newPosition;
                        this.Figure.dimType = VdConstDimType.dim_Aligned;
                        this.Figure.AngularType = VdConstDimAngularType.Length;
                        this.Figure.visibility = vdFigure.VisibilityEnum.Visible;
                        this.Figure.Rotation = 0;
                    }
                    break;
                case eMethod.Vertical:
                    if (this.First != null && this.Second != null)
                    {
                        this.Figure.DefPoint1 = this.First;
                        this.Figure.DefPoint2 = this.Second;
                        this.Figure.LinePosition = newPosition;
                        this.Figure.dimType = VdConstDimType.dim_Rotated;
                        this.Figure.AngularType = VdConstDimAngularType.Length;
                        this.Figure.visibility = vdFigure.VisibilityEnum.Visible;
                        this.Figure.Rotation = Globals.HALF_PI;
                    }
                    break;
                case eMethod.Horizontal:
                    if (this.First != null && this.Second != null)
                    {
                        this.Figure.DefPoint1 = this.First;
                        this.Figure.DefPoint2 = this.Second;
                        this.Figure.LinePosition = newPosition;
                        this.Figure.dimType = VdConstDimType.dim_Rotated;
                        this.Figure.AngularType = VdConstDimAngularType.Length;
                        this.Figure.visibility = vdFigure.VisibilityEnum.Visible;
                        this.Figure.Rotation = 0;
                    }
                    break;
                case eMethod.Angular:
                    if (this.FirstFigure is vdArc)
                    {
                        vdArc arc = this.FirstFigure as vdArc;
                        this.Figure.DefPoint1 = arc.Center;
                        this.Figure.DefPoint2 = arc.getStartPoint();
                        this.Figure.DefPoint3 = arc.getStartPoint();
                        this.Figure.DefPoint4 = arc.getEndPoint();
                        this.Figure.LinePosition = newPosition;
                        this.Figure.dimType = VdConstDimType.dim_Angular;
                        this.Figure.AngularType = VdConstDimAngularType.Angle;
                        this.Figure.visibility = vdFigure.VisibilityEnum.Visible;
                        this.Figure.Rotation = 0;
                        this.Figure.DimAunit = AUnits.AUnitType.au_Decdegrees;
                    }
                    else if (this.FirstFigure is vdLine && this.SecondFigure is vdLine)
                    {
                        vdLine vLine1 = this.FirstFigure as vdLine;
                        vdLine vLine2 = this.SecondFigure as vdLine;
                        linesegment lineSeg1 = new linesegment(vLine1.StartPoint, vLine1.EndPoint);
                        linesegment lineSeg2 = new linesegment(vLine2.StartPoint, vLine2.EndPoint);
                        gPoints defPoint = DrawDimension.GetAnglurDimensionLine(this.Intersection, lineSeg1, lineSeg2, newPosition);
                        if (defPoint.Count > 3)
                        {
                            this.Figure.DefPoint1 = defPoint[0];    // 각도를 재는중심점
                            this.Figure.DefPoint2 = defPoint[1];    // 쓸모없는 점
                            this.Figure.DefPoint3 = defPoint[2];    // 첫번째 선택한 Figure의 점
                            this.Figure.DefPoint4 = defPoint[3];    // 두번째 선택한 Figure의 점
                            this.Figure.LinePosition = newPosition;
                            this.Figure.dimType = VdConstDimType.dim_Angular;
                            this.Figure.AngularType = VdConstDimAngularType.Angle;
                            this.Figure.visibility = vdFigure.VisibilityEnum.Visible;
                            this.Figure.Rotation = 0;
                            this.Figure.DimAunit = AUnits.AUnitType.au_Decdegrees;
                        }
                    }
                    break;
            }
        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
        }

        public void SetSecondary()
        {
            this.Second = new gPoint(this.NewPosition);
        }

        public static List<vdFigure> Run(vdDocument doc, eMethod method, DrawDimension.DimensionOptions opt)
        {
            List<vdFigure> result = new List<vdFigure>();
            CmdDimension cmdDim = null;
            switch(method)
            {
                case eMethod.Aligned:
                case eMethod.Vertical:
                case eMethod.Horizontal:
                    cmdDim = DimensionLine(doc, method, opt);
                    break;
                case eMethod.Angular:
                    cmdDim = DimensionAngular(doc, method, opt);
                    break;
            }

            if(cmdDim != null && cmdDim.Entity != null)
                result.Add(cmdDim.Entity);
            
            return result;
        }

        private static CmdDimension DimensionLine(vdDocument doc, eMethod method, DrawDimension.DimensionOptions opt)
        {
            doc.Prompt("Pick dimension first point");
            gPoint reference = doc.ActionUtility.getUserPoint() as gPoint;
            if (reference == null)
                return null;

            CmdDimension cmdDim = new CmdDimension(reference, doc.ActiveLayOut, method, opt);
            doc.ActionAdd(cmdDim);

            doc.Prompt("Pick dimension secondary point");
            StatusCode scode = cmdDim.WaitToFinish();
            if (scode != StatusCode.Success)
                return null;
            cmdDim.SetSecondary();
            
            doc.Prompt("Pick dimension position");
            doc.ActionAdd(cmdDim);
            scode = cmdDim.WaitToFinish();

            if (scode == StatusCode.Success)
            {
                doc.ActionLayout.Entities.AddItem(cmdDim.Entity);
                doc.ActionDrawFigure(cmdDim.Entity);
            }
            return cmdDim;
        }

        private static CmdDimension DimensionAngular(vdDocument doc, eMethod method, DrawDimension.DimensionOptions opt)
        {
            vdFigure figure1 = null;
            vdFigure figure2 = null;
            gPoint retpt1 = null;
            gPoint retpt2 = null;
            gPoint intersection = new gPoint();

            doc.Prompt("Select first line(or arc) entity");
            StatusCode status = doc.ActionUtility.getUserEntity(out figure1, out retpt1);
            if (status != StatusCode.Success)
                return null;

            bool isAcceptEntity = false;
            if (figure1 is vdArc)
            {
                vdArc arc = figure1 as vdArc;
                if (Globals.IntersectionLL2D(arc.Center, arc.getStartPoint(), arc.Center, arc.getEndPoint(), intersection) < 1)
                    return null;

                isAcceptEntity = true;
            }
            else if (figure1 is vdLine)
            {
                figure1.HighLight = true;
                figure1.Update();
                doc.Redraw(true);

                doc.Prompt("Select secondary line entity");
                status = doc.ActionUtility.getUserEntity(out figure2, out retpt2);
                figure1.HighLight = false;
                figure1.Update();
                doc.Redraw(true);

                if (status != StatusCode.Success)
                    return null;
                
                if (figure2 is vdLine)
                {
                    vdLine line1 = figure1 as vdLine;
                    vdLine line2 = figure2 as vdLine;
                    if (Globals.IntersectionLL2D(line1.StartPoint, line1.EndPoint, line2.StartPoint, line2.EndPoint, intersection) < 1)
                        return null; // 두선분이 평행하면 리턴

                    isAcceptEntity = true;
                }
            }

            if (isAcceptEntity)
            {
                CmdDimension cmdDim = new CmdDimension(figure1, figure2, intersection, doc.ActiveLayOut, method, opt);
                doc.ActionAdd(cmdDim);
                doc.Prompt("Pick insert position");
                StatusCode scode = cmdDim.WaitToFinish();
                if (scode != StatusCode.Success)
                    return null;

                doc.ActionLayout.Entities.AddItem(cmdDim.Entity);
                doc.ActionDrawFigure(cmdDim.Entity);
                return cmdDim;
            }
            return null;
        }    
    }
}
