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

using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;
using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.Command
{
    public class CmdCircle : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private HCircle Figure { get; set; }
        public enum eMethod
        {
            PickCenterPoint = 1,
            Pick3Point = 2,
            Pick2Point = 3,
            SelectMethod
        }

        public gPoint First { get; set; }
        public gPoint Secondary { get; set; }        
        public eMethod Method { get; set; }

        public CmdCircle(gPoint reference, vdLayout layout, eMethod method) : base(reference, layout)
        {
            this.Method = method;
            this.Figure = new HCircle();
            this.Figure.SetUnRegisterDocument(layout.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;
            this.First = new gPoint(reference);
        }

        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            this.NewPosition = new gPoint(newPosition);
            Circle circle = null;
            switch (this.Method)
            {
                case eMethod.PickCenterPoint:
                    circle = new Circle(this.First, this.First.Distance2D(newPosition));
                    break;
                case eMethod.Pick3Point:
                    if (this.First != null && this.Secondary != null)
                        circle = new Circle(this.First, this.Secondary, newPosition);
                    break;
                case eMethod.Pick2Point:
                    if (this.First != null)
                        circle = new Circle(this.First, newPosition);
                    break;
            }

            if (circle != null && circle.IsCircle)
            {
                this.Figure.Center = circle.Center;
                this.Figure.Radius = circle.Radius;
            }
        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            switch (this.Method)
            {
                case CmdCircle.eMethod.PickCenterPoint:
                    this.DrawAlignedDimension(this.Figure.Center, orthoPoint, render);
                    break;
                case CmdCircle.eMethod.Pick3Point: // 세점을 지나는 원 생성
                    if (this.First == null)
                        return;

                    double dist = 0;
                    gPoint point = null;
                    if (this.Secondary == null)
                    {
                        dist = this.First.Distance2D(orthoPoint);
                        point = this.First + Geometry.Geometry.Xaxis * dist;

                        this.DrawLine(this.First, point, render);
                        this.DrawAlignedDimension(this.First, orthoPoint, render);
                        this.DrawAngularDimension(this.First, point, orthoPoint, render);
                        return;
                    }

                    dist = this.Secondary.Distance2D(orthoPoint);
                    point = this.Secondary + Geometry.Geometry.Xaxis * dist;

                    this.DrawLine(this.First, this.Secondary, render);
                    this.DrawLine(this.Secondary, orthoPoint, render);
                    this.DrawLine(this.Secondary, point, render);

                    //this.DrawAlignedDimension(this.First, this.Secondary, render);
                    this.DrawAlignedDimension(this.Secondary, orthoPoint, render);
                    this.DrawAngularDimension(this.Secondary, point, orthoPoint, render);
                    break;
                case CmdCircle.eMethod.Pick2Point: // 두점을 지나는 원 생성                    
                    if (this.First == null)
                        return;
                    dist = this.First.Distance2D(orthoPoint);
                    point = this.First + Geometry.Geometry.Xaxis * dist;

                    this.DrawLine(this.First, orthoPoint, render);
                    this.DrawLine(this.First, point, render);
                    this.DrawAlignedDimension(this.First, orthoPoint, render);
                    this.DrawAngularDimension(this.First, point, orthoPoint, render);
                    break;
            }
        }

        public void PickSecondary()
        {
            this.Secondary = new gPoint(this.NewPosition);
        }

        public static List<vdFigure> Run(vdDocument doc, eMethod method)
        {
            List<vdFigure> result = new List<vdFigure>();
            int type = (int)method;
            if (method == eMethod.SelectMethod)
            {
                doc.Prompt("1:Center,Radius 2:3Point 3:2Point");
                doc.ActionUtility.getUserInt(out type);
            }
            if (type == 0)
                type = 1;
            method = (eMethod)type;
            if (method == eMethod.PickCenterPoint)
                doc.Prompt("Pick center point");
            else
                doc.Prompt("Pick first point");

            gPoint reference = doc.ActionUtility.getUserPoint() as gPoint;
            if (reference == null)
                return result;

            CmdCircle cmdCircle = new CmdCircle(reference, doc.ActiveLayOut, method);
            StatusCode sCode = StatusCode.Cancel;
            switch (method)
            {
                case eMethod.PickCenterPoint:
                    sCode = CirclePickCenter(cmdCircle, doc);
                    break;
                case eMethod.Pick3Point:
                    sCode = CirclePick3Point(cmdCircle, doc);
                    break;
                case eMethod.Pick2Point:
                    sCode = CirclePick2Point(cmdCircle, doc);
                    break;
            }

            if(sCode == StatusCode.Success)
            {
                doc.ActionLayout.Entities.AddItem(cmdCircle.Entity);
                doc.ActionDrawFigure(cmdCircle.Entity);
                result.Add(cmdCircle.Entity);
            }
            return result;
        }

        public static StatusCode CirclePickCenter(CmdCircle cmdCircle, vdDocument doc)
        {
            StatusCode scode = cmdCircle.WaitToFinish("Pick radius point or input", valueType.REFPOINT, true);
            return scode;
        }

        // 세점을 지나는 원 생성
        public static StatusCode CirclePick3Point(CmdCircle cmdCircle, vdDocument doc)
        {
            StatusCode scode = cmdCircle.WaitToFinish("Pick secondary point", valueType.REFPOINT, true);
            if (scode != StatusCode.Success)
                return scode;
            cmdCircle.PickSecondary();
            
            scode = cmdCircle.WaitToFinish("Pick third point", valueType.REFPOINT, true);
            return scode;
        }

        public static StatusCode CirclePick2Point(CmdCircle cmdCircle, vdDocument doc)
        {
            StatusCode scode = cmdCircle.WaitToFinish("Pick second point", valueType.REFPOINT, true);
            return scode;
        }

    }
}
