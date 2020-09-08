using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.Actions;
using VectorDraw.Geometry;
using VectorDraw.Actions;

using Hicom.BizDraw.PlanDraw;
using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.Command
{
    public class ActionBase : ActionEntity
    {
        protected vdDocument Document { get { return this.Layout.Document; } }
        protected gPoint NewPosition { get; set; }

        // 치수선 Scale factor - 1000mm에 scalefactor 20을 기준으로 한다.(20 / 1000)
        private const double ScaleFactor = 0.02;
        private const int DecimalPrecision = 2;

        public ActionBase(gPoint reference, vdLayout layout) : base(reference, layout)
        {
            this.ValueTypeProp = valueType.REFPOINT;
            this.OsnapOverrideMode = this.Layout.Document.osnapMode;
        }
        
        /// <summary>
        /// 도면 그리기시 추가 보조선을 그리기위한 기능
        /// </summary>
        /// <param name="orthoPoint"></param>
        /// <param name="render"></param>
        public virtual void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            
        }

        /// <summary>
        /// Action시 화면에 Line 그리기
        /// </summary>
        /// <param name="start">선 시작점</param>
        /// <param name="end">선 끝점</param>
        protected void DrawLine(gPoint start, gPoint end, ActionWrapperRender render)
        {
            vdLine vLine = new vdLine();
            vLine.SetUnRegisterDocument(this.Document);
            vLine.setDocumentDefaults();
            vLine.StartPoint = start;
            vLine.EndPoint = end;
            vLine.Layer = this.Document.Layers.FindName(CommonActionString.ACTION_REF_LINE);
            vLine.Draw(render);
        }

        /// <summary>
        /// Action시 화면에 Text 그리기
        /// </summary>
        /// <param name="text"> 텍스트 내용 </param>
        /// <param name="insertPoint"> 텍스트를 넣을 위치 </param>
        /// <param name="height"> 텍스트의 크기 </param>
        protected void DrawText(gPoint insertPoint, string text, double height, ActionWrapperRender render)
        {
            vdText vText = new vdText();
            vText.SetUnRegisterDocument(this.Document);
            vText.setDocumentDefaults();
            vText.TextString = text;
            vText.InsertionPoint = insertPoint;
            vText.Height = height;
            vText.Layer = this.Document.Layers.FindName(CommonActionString.ACTION_TEXT); // 임시
            vText.Draw(render);
        }
        
        private DrawDimension.DimensionOptions CreateDimensionOption(double distance)
        {
            DrawDimension.DimensionOptions opt = new DrawDimension.DimensionOptions();
            opt.ScaleFactor = distance * ActionBase.ScaleFactor;
            opt.TextHeight = 1.5;
            opt.FirstSpace = opt.FirstSpace * opt.ScaleFactor / 25.0;
            opt.DimSpace = opt.DimSpace * opt.ScaleFactor / 25.0;
            opt.DecimalPrecision = ActionBase.DecimalPrecision;
            return opt;
        }

        protected void DrawAlignedDimension(gPoint start, gPoint end, ActionWrapperRender render)
        {
            double dist = start.Distance2D(end);
            if(Entity != null)
                dist = Math.Max(Entity.BoundingBox.Width, Entity.BoundingBox.Height);
            DrawDimension.DimensionOptions opt = this.CreateDimensionOption(dist);

            vdDimension dimension = new vdDimension();
            dimension.SetUnRegisterDocument(this.Document);
            dimension.setDocumentDefaults();
            dimension.AngularType = VdConstDimAngularType.Length;
            dimension.Rotation = 0;
            dimension.dimType = VdConstDimType.dim_Aligned;
            dimension.DefPoint1 = start;
            dimension.DefPoint2 = end;
            double angle = start.GetAngle(end);
            if (angle > Math.PI)
                dimension.LinePosition = DrawDimension.GetLinePosition(start, end, DrawDimension.eDirection.Right, opt.FirstSpace, opt.DimSpace, 1);
            else
                dimension.LinePosition = DrawDimension.GetLinePosition(start, end, DrawDimension.eDirection.Left, opt.FirstSpace, opt.DimSpace, 1);
            dimension.Layer = this.Document.Layers.FindName(CommonActionString.ACTION_STRAIGHT_DIM);
            dimension.TextStyle = this.Document.TextStyles.FindName(CommonActionString.ACTION_DIM_TEXTSTYLE);

            dimension.ArrowSize = opt.ArrowSize;
            dimension.TextHeight = opt.TextHeight;
            dimension.ScaleFactor = opt.ScaleFactor;
            dimension.DecimalPrecision = opt.DecimalPrecision;
            dimension.Draw(render);
        }

        protected void DrawAngularDimension(gPoint center, gPoint startPoint, gPoint endPoint, ActionWrapperRender render, bool isArc = false)
        {
            double angle = center.GetAngle(endPoint); // 단위 : Radian 
            if (Geometry.Geometry.Equals(angle, 0))
                return;
            Arc arc = null;
            if (angle < Math.PI || isArc)
                arc = new Arc(center, startPoint, center.GetAngle(endPoint));
            else
                arc = new Arc(center, endPoint, center.GetAngle(startPoint));
            
            DrawDimension.DimensionOptions opt = this.CreateDimensionOption(center.Distance2D(startPoint));

            vdDimension dimension = new vdDimension();
            dimension.SetUnRegisterDocument(this.Document);
            dimension.setDocumentDefaults();
            dimension.AngularType = VdConstDimAngularType.Angle;
            dimension.Rotation = 0;
            dimension.dimType = VdConstDimType.dim_Angular;
            dimension.DimAunit = AUnits.AUnitType.au_Decdegrees;
            dimension.DefPoint1 = arc.Center;            
            dimension.DefPoint2 = arc.StartPoint;
            dimension.DefPoint3 = arc.StartPoint;
            dimension.DefPoint4 = arc.EndPoint;
            dimension.LinePosition = arc.MiddlePoint();
            dimension.Layer = this.Document.Layers.FindName(CommonActionString.ACTION_STRAIGHT_DIM);
            dimension.TextStyle = this.Document.TextStyles.FindName(CommonActionString.ACTION_DIM_TEXTSTYLE);

            dimension.ArrowSize = opt.ArrowSize;
            dimension.TextHeight = opt.TextHeight;
            dimension.ScaleFactor = opt.ScaleFactor;
            dimension.DecimalPrecision = opt.DecimalPrecision;
            dimension.Draw(render);
        }

        // AngularDimension과 병합 필요?
        protected void DrawArcLengthDimension(gPoint first, gPoint second, gPoint third, ActionWrapperRender render)
        {
            Arc arc = new Arc(first, second, third);
            if(arc.IsCircle)
            {
                DrawDimension.DimensionOptions opt = this.CreateDimensionOption(arc.Center.Distance2D(arc.StartPoint));

                vdDimension dimension = new vdDimension();
                dimension.SetUnRegisterDocument(this.Document);
                dimension.setDocumentDefaults();
                dimension.AngularType = VdConstDimAngularType.Length;
                dimension.Rotation = 0;
                dimension.dimType = VdConstDimType.dim_Angular;
                dimension.DimAunit = AUnits.AUnitType.au_Decdegrees;
                dimension.DefPoint1 = arc.Center;
                dimension.DefPoint2 = arc.StartPoint;
                dimension.DefPoint3 = arc.StartPoint;
                dimension.DefPoint4 = arc.EndPoint;
                dimension.LinePosition = arc.MiddlePoint();
                dimension.Layer = this.Document.Layers.FindName(CommonActionString.ACTION_STRAIGHT_DIM);
                dimension.TextStyle = this.Document.TextStyles.FindName(CommonActionString.ACTION_DIM_TEXTSTYLE);

                dimension.ArrowSize = opt.ArrowSize;
                dimension.TextHeight = opt.TextHeight;
                dimension.ScaleFactor = opt.ScaleFactor;
                dimension.DecimalPrecision = opt.DecimalPrecision;
                dimension.Draw(render);
            }
        }

        public StatusCode WaitToFinish(string prompt, valueType valtype, bool moveRefPoint = false)
        {
            this.ValueTypeProp = valtype;
            this.Document.ActionAdd(this);
            this.Document.Prompt(prompt);
            StatusCode scode = this.WaitToFinish();
            if(scode == StatusCode.Success && this.Value is gPoint && moveRefPoint)
            {
                if ((this.ValueTypeProp & valueType.REFPOINT) > 0)
                    this.ReferencePoint = this.Value as gPoint;
            }
            return scode;
        }
    }
}