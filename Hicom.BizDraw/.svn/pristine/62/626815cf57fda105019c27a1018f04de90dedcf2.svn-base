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
    public class CmdPolyline : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private HPolyline Figure { get; set; }  

        public CmdPolyline(gPoint reference, vdLayout layout) : base(reference, layout)
        {
            this.Figure = new HPolyline();
            this.Figure.SetUnRegisterDocument(layout.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;
            this.Figure.VertexList.Add(new gPoint(reference));
        }
        
        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            
        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            gPoint sttPt = this.Figure.VertexList.Last().AsgPoint();
            double dist = sttPt.Distance2D(orthoPoint);
            gPoint point = sttPt + Geometry.Geometry.Xaxis * dist;

            this.DrawAlignedDimension(sttPt, orthoPoint, render);
            this.DrawAngularDimension(sttPt, point, orthoPoint, render);
            this.DrawLine(sttPt, point, render);
        }
        
        public static List<vdFigure> Run(vdDocument doc)
        {
            List<vdFigure> result = new List<vdFigure>();
            
            doc.Prompt("Pick start point");
            gPoint start = doc.ActionUtility.getUserPoint() as gPoint;
            if (start == null)
                return result;

            CmdPolyline cmdPolyLine = new CmdPolyline(start, doc.ActiveLayOut);
            StatusCode sCode = cmdPolyLine.WaitToFinish("Pick Next point", valueType.REFPOINT, true);
            if (sCode == StatusCode.Success && cmdPolyLine.Value is gPoint)
            {
                gPoint point = cmdPolyLine.Value as gPoint;
                cmdPolyLine.Figure.VertexList.Add(point);
                
                while (sCode == StatusCode.Success)
                {
                    string close = string.Format("{0};c;", Command.POLYLINE_CLOSE);
                    cmdPolyLine.SetAcceptedStringValues(new string[] { close }, null);
                    sCode = cmdPolyLine.WaitToFinish("Pick Next point[Close(c)]", valueType.REFPOINT | valueType.STRING, true);
                    if (sCode == StatusCode.Success)
                    {
                        if (cmdPolyLine.Value is string)
                        {
                            string command = cmdPolyLine.Value.ToString().ToLower();
                            if (command.Equals(Command.POLYLINE_CLOSE))
                            {
                                cmdPolyLine.Figure.Flag = VdConstPlineFlag.PlFlagCLOSE;
                                sCode = StatusCode.Cancel;
                            }
                        }
                        else if (cmdPolyLine.Value is gPoint)
                        {
                            point = cmdPolyLine.Value as gPoint;
                            cmdPolyLine.Figure.VertexList.Add(point);
                        }
                    }
                }

                doc.ActionLayout.Entities.AddItem(cmdPolyLine.Entity);
                doc.ActionDrawFigure(cmdPolyLine.Entity);
                result.Add(cmdPolyLine.Entity);
            }
            
            return result;
        }
    }
}
