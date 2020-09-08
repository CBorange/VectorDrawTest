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
    public class CmdRectang : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private HRect Figure { get; set; }
        
        public CmdRectang(gPoint reference, vdLayout layout) : base(reference, layout)
        {
            this.Figure = new HRect();
            this.Figure.SetUnRegisterDocument(layout.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;
            this.Figure.InsertionPoint = reference;
        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            gPoint pointx = new gPoint(orthoPoint.x, this.Figure.InsertionPoint.y);
            gPoint pointy = new gPoint(this.Figure.InsertionPoint.x, orthoPoint.y);

            this.DrawAlignedDimension(this.Figure.InsertionPoint, pointx, render);
            this.DrawAlignedDimension(this.Figure.InsertionPoint, pointy, render);
        }

        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            double width = newPosition.x - this.Figure.InsertionPoint.x;
            double height = newPosition.y - this.Figure.InsertionPoint.y;

            this.Figure.Width = width;
            this.Figure.Height = height;
        }
        
        public static List<vdFigure> Run(vdDocument doc)
        {
            List<vdFigure> result = new List<vdFigure>();

            doc.Prompt("Pick start point");
            gPoint reference = doc.ActionUtility.getUserPoint() as gPoint;
            if (reference == null)
                return result;

            CmdRectang cmdRect = new CmdRectang(reference, doc.ActiveLayOut);
            StatusCode scode = cmdRect.WaitToFinish("Pick end point", valueType.REFPOINT, true);
            if (scode == StatusCode.Success)
            {
                cmdRect.Entity.Transformby(doc.User2WorldMatrix);
                doc.ActionLayout.Entities.AddItem(cmdRect.Entity);
                doc.ActionDrawFigure(cmdRect.Entity);
                result.Add(cmdRect.Entity);
            }
            
            return result;
        }
    }
}
 