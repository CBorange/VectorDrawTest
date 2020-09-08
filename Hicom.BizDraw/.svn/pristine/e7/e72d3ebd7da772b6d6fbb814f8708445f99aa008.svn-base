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
using VectorDraw.Geometry;
using VectorDraw.Actions;

using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;
using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.Command
{
    public class CmdArc : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private HArc Figure { get; set; }
        public enum eArcMethod
        {
            Pick3PointOnArc = 1,
            PickCenterPoint = 2,
            Pick2PointAngle = 3,
            SelectMethod = 4
        }
        public gPoint Center { get; set; }
        public gPoint First { get; set; }
        public gPoint Second { get; set; }
        public gPoint Third { get; set; }
        public double Angle { get; set; }

        public eArcMethod Method { get; set; }

        public CmdArc(gPoint reference, vdLayout layout, eArcMethod method) : base(reference, layout)
        {
            this.ValueTypeProp = valueType.REFPOINT;

            this.Method = method;
            this.Figure = new HArc();
            this.Figure.SetUnRegisterDocument(layout.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;

            if (method == eArcMethod.Pick3PointOnArc)
                this.First = new gPoint(reference);
            else
                this.Center = new gPoint(reference);
        }

        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            this.NewPosition = new gPoint(newPosition);
            switch (this.Method)
            {
                case eArcMethod.Pick3PointOnArc: // 3점을 지나는 Arc
                    if (this.First != null && this.Second != null)
                    {
                        Arc arc = new Arc(this.First, this.Second, newPosition);
                        if(arc.IsCircle)
                        {
                            this.Figure.Center = arc.Center;
                            this.Figure.Radius = arc.Radius;
                            this.Figure.StartAngle = arc.StartAngle;
                            this.Figure.EndAngle = arc.EndAngle;
                        }
                    }
                    break;
                case eArcMethod.PickCenterPoint: // Center, Start, End Point
                case eArcMethod.Pick2PointAngle:
                    if (this.Center != null && this.First != null)
                    {
                        double radius = this.Center.Distance2D(this.First);
                        double sttAng = this.Center.GetAngle(this.First);
                        double endAng = this.Center.GetAngle(newPosition);

                        Arc arc = new Arc(this.Center, radius, sttAng, endAng);
                        if(arc.IsCircle)
                        {
                            this.Figure.Center = new gPoint(arc.Center);
                            this.Figure.Radius = arc.Radius;
                            this.Figure.StartAngle = arc.StartAngle;
                            this.Figure.EndAngle = arc.EndAngle;
                        }
                    }
                    break;
            }
        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            Arc arc = null;
            switch (this.Method)
            {
                case eArcMethod.Pick3PointOnArc:
                    if (this.First == null)
                        return;

                    if (this.Second == null)
                    {
                        this.DrawLine(this.First, orthoPoint, render);
                        this.DrawAlignedDimension(this.First, orthoPoint, render);
                        return;
                    }

                    arc = new Arc(this.First, this.Second, orthoPoint);
                    if (arc.IsCircle)
                    {
                        this.DrawLine(this.First, this.Second, render);
                        this.DrawLine(this.First, new gPoint(arc.Center), render);
                        this.DrawLine(arc.Center, orthoPoint, render);
                        this.DrawAlignedDimension(this.First, this.Second, render);
                        this.DrawAlignedDimension(this.Second, orthoPoint, render);
                    }
                    break;
                case eArcMethod.PickCenterPoint:
                case eArcMethod.Pick2PointAngle:
                    if (this.Center == null)
                        return;

                    if (this.First == null)
                    {
                        this.DrawLine(this.Center, orthoPoint, render);
                        this.DrawAlignedDimension(this.Center, orthoPoint, render);
                        return;
                    }

                    double radius = this.Center.Distance2D(this.First);
                    double sttAng = this.Center.GetAngle(this.First);
                    double endAng = this.Center.GetAngle(orthoPoint);
                    
                    arc = new Arc(this.Center, radius, sttAng, endAng);
                    if(arc.IsCircle)
                    {
                        // 마우스 위치에 따라 Dimention을 그리기 위한 Point를 다시 잡아주어야 한다.
                        double dist = this.Center.Distance2D(orthoPoint);
                        gPoint first = this.Center + new Vector(this.Center, this.First) * dist;

                        this.DrawLine(this.Center, this.First, render);
                        this.DrawLine(this.Center, orthoPoint, render);
                        this.DrawAlignedDimension(this.Center, this.First, render);
                        this.DrawAngularDimension(this.Center, first, orthoPoint, render, true);
                    }
                    break;
            }
        }

        /// <param name="index">0=Center, 1=first, 2=second, 3=third</param>
        public void PickPostion(int index)
        {
            switch (index)
            {
                case 0:
                    this.Center = new gPoint(this.NewPosition);
                    break;
                case 1:
                    this.First = new gPoint(this.NewPosition);
                    break;
                case 2:
                    this.Second = new gPoint(this.NewPosition);
                    break;
                case 3:
                    this.Third = new gPoint(this.NewPosition);
                    break;
            }
        }

        public static List<vdFigure> Run(vdDocument doc, eArcMethod method)
        {
            List<vdFigure> result = new List<vdFigure>();

            int type = (int)method;
            if (method == eArcMethod.SelectMethod)
            {
                doc.Prompt("1:3P 2:center,start,end, 3:center,start,angle");
                doc.ActionUtility.getUserInt(out type);
            }
            if (type == 0)
                type = 1;
            method = (eArcMethod)type;
            
            if (method == eArcMethod.Pick3PointOnArc)
                doc.Prompt("Pick first point");
            else
                doc.Prompt("Pick center point");

            gPoint reference = doc.ActionUtility.getUserPoint() as gPoint;
            if (reference == null)
                return result;

            CmdArc cmdArc = new CmdArc(reference, doc.ActiveLayOut, method);
            StatusCode sCode = StatusCode.Cancel;
            switch ((eArcMethod)type)
            {
                case eArcMethod.Pick3PointOnArc: // 3점을 지나는 Arc
                    sCode = Arc3PointOnArc(cmdArc, doc);
                    break;
                case eArcMethod.PickCenterPoint: // Center, Start, End Point
                    sCode = Arc3PointCenter(cmdArc, doc);
                    break;
                case eArcMethod.Pick2PointAngle:
                    sCode = Arc2PointAngle(cmdArc, doc);
                    break;
            }

            if (sCode == StatusCode.Success)
            {
                doc.ActionLayout.Entities.AddItem(cmdArc.Entity);
                doc.ActionDrawFigure(cmdArc.Entity);
                result.Add(cmdArc.Entity);
            }
            
            return result;
        }

        /// <summary>
        /// 3점을 지나는 Arc
        /// </summary>
        /// <returns></returns>
        private static StatusCode Arc3PointOnArc(CmdArc cmdArc, vdDocument doc)
        {
            StatusCode scode = cmdArc.WaitToFinish("Pick secondary point", valueType.REFPOINT, true);
            if (scode != StatusCode.Success)
                return scode;
            cmdArc.PickPostion(2);
            
            scode = cmdArc.WaitToFinish("Pick third point", valueType.REFPOINT, true);
            return scode;
        }

        /// <summary>
        /// Center Point, Start Point, EndPoint
        /// </summary>
        /// <returns></returns>
        private static StatusCode Arc3PointCenter(CmdArc cmdArc, vdDocument doc)
        {
            StatusCode scode = cmdArc.WaitToFinish("Pick start point", valueType.REFPOINT);
            if (scode != StatusCode.Success)
                return scode;
            cmdArc.PickPostion(1);
            
            scode = cmdArc.WaitToFinish("Pick end point", valueType.REFPOINT);
            return scode;
        }

        /// <summary>
        /// 2점과 각도를 가지는 Arc
        /// </summary>
        /// <returns></returns>
        private static StatusCode Arc2PointAngle(CmdArc cmdArc, vdDocument doc)
        {
            StatusCode scode = cmdArc.WaitToFinish("Pick start point", valueType.REFPOINT);
            if (scode != StatusCode.Success)
                return scode;
            cmdArc.PickPostion(1);
            
            scode = cmdArc.WaitToFinish("Angle", valueType.REFPOINTANGLE);
            double sttang = cmdArc.Figure.StartAngle;
            double endang = cmdArc.Figure.EndAngle;
            double newEndAng = (double)cmdArc.Value;

            // 마우스위치 각도보다 사용자가 입력한값이 우선
            if (Geometry.Geometry.Equals(endang, newEndAng) == false)
                cmdArc.Figure.EndAngle = newEndAng + sttang;
            
            return scode;
        }
    }
}
