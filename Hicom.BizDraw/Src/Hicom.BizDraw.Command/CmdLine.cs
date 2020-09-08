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

using Hicom.BizDraw.Entity;
using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;

namespace Hicom.BizDraw.Command
{
    public class CmdLine : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private HLine Figure { get; set; }

        public CmdLine(gPoint reference, vdLayout layout) : base(reference, layout)
        {
            this.Figure = new HLine();
            this.Figure.SetUnRegisterDocument(layout.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;
            this.Figure.StartPoint = reference;
        }
        
        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            if(this.mUserValue != null)
            {
                Vector dir = new Vector(newPosition - this.Figure.StartPoint);
                if (dir.Normalize())
                {
                    this.Figure.EndPoint = this.Figure.StartPoint + (double)this.mUserValue * dir;
                }
            }
            else
            {
                this.Figure.EndPoint = newPosition;
            }
        }
        
        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            if(this.Figure.StartPoint != null)
            {
                double dist = this.Figure.StartPoint.Distance2D(orthoPoint);
                gPoint point = this.Figure.StartPoint + Geometry.Geometry.Xaxis * dist;

                this.DrawLine(this.Figure.StartPoint, point, render);
                this.DrawAlignedDimension(this.Figure.StartPoint, orthoPoint, render);
                this.DrawAngularDimension(this.Figure.StartPoint, point, orthoPoint, render);
            }
        }
        
        public static List<vdFigure> Run(vdDocument doc, bool repeat)
        {
            List<vdFigure> result = new List<vdFigure>();
            
            doc.Prompt("Pick start point");
            gPoint start = doc.ActionUtility.getUserPoint() as gPoint;
            if (start == null)
                return result;
            
            CmdLine cmdLine = new CmdLine(start, doc.ActiveLayOut);
            StatusCode sCode = cmdLine.WaitToFinish("Pick end point", valueType.REFPOINT);
            if (sCode == StatusCode.Success)
            {
                doc.ActionLayout.Entities.AddItem(cmdLine.Entity);
                doc.ActionDrawFigure(cmdLine.Entity);
                result.Add(cmdLine.Entity);
            }
            
            return result;
        }
    }
}
