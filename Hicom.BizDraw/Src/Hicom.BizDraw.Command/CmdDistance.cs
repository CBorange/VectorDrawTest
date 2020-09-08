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
    public class CmdDistance : ActionBase
    {
        //public override vdFigure Entity { get { return Figure; } }
        //private vdFigure Figure { get; set; }
        
        private List<gPoints> DistancePointsList { get; set; }
                
        // Line
        public CmdDistance(gPoint reference, vdLayout layout) : base(reference, layout)
        {
            this.DistancePointsList = new List<gPoints>();
        }
        
        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            
        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            for (int ix = 0; ix < this.DistancePointsList.Count; ix++)
            {
                gPoints pickPoints = this.DistancePointsList[ix];
                if (ix < (DistancePointsList.Count - 1)) // 작업 완료
                {
                    if (pickPoints.Count == 2) //Line
                        this.DrawAlignedDimension(pickPoints[0], pickPoints[1], render);
                    if (pickPoints.Count == 3) //Arc
                        this.DrawArcLengthDimension(pickPoints[0], pickPoints[1], pickPoints[2], render);
                }
                else // 작업 진행
                {
                    if (pickPoints.Count == 1) //Line (Arc라해도 아직 2점이 아님으로 Line으로 취급)
                        this.DrawAlignedDimension(pickPoints[0], orthoPoint, render);
                    if (pickPoints.Count == 2) // Arc
                        this.DrawArcLengthDimension(pickPoints[0], pickPoints[1], orthoPoint, render);
                }
            }
        }

        public static List<string> Run(vdDocument doc, DrawDimension.DimensionOptions opt)
        {
            //double totalDist = 0.0;
            List<string> message = new List<string>();
            
            gPoint firstpt = null;
            doc.Prompt("Pick First Point");
            if (doc.ActionUtility.getUserPoint(out firstpt) == StatusCode.Success)
            {
                gPoints pickPoints = new gPoints();
                pickPoints.Add(firstpt);

                CmdDistance cmdDist = new CmdDistance(firstpt, doc.ActiveLayOut);
                cmdDist.DistancePointsList.Add(pickPoints);

                //------------------------------ 차후 삭제하고 수정 요망 ----------------------------------
                if (cmdDist.WaitToFinish("Pick Next Point", valueType.REFPOINT, true) == StatusCode.Success)
                {
                    if (cmdDist.Value is gPoint)
                    {
                        pickPoints.Add(cmdDist.Value);
                        if (pickPoints.Count > 1)
                        {
                            double dist = Math.Round(pickPoints[0].Distance2D(pickPoints[1]), 3);
                            double xInc = Math.Round(Math.Abs(pickPoints[0].x - pickPoints[1].x), 3);
                            double yInc = Math.Round(Math.Abs(pickPoints[0].y - pickPoints[1].y), 3);
                            message.Add(string.Concat("거리 = ", dist));
                            message.Add(string.Concat("X 중분 = ", xInc, ", Y 증분 = ", yInc));
                            
                            //totalDist += Geometry.Geometry.GetLineLength(pickPoints[0], pickPoints[1]);
                        }       
                    }
                }
                //-----------------------------------------------------------------------------------------

                //--------------------------- MultiPoint 개발시 참조하여 개발 -----------------------------
                //string multi = string.Format("{0};m;", Command.DIST_MULTI);
                //cmdDist.SetAcceptedStringValues(new string[] { multi }, string.Empty);
                //if (cmdDist.WaitToFinish("Pick Next Point or [Multi Point(M)]", valueType.STRING | valueType.REFPOINT, true) == StatusCode.Success)
                //{
                //    if (cmdDist.Value is gPoint)
                //    {
                //        pickPoints.Add(cmdDist.Value);
                //        if(pickPoints.Count > 1)
                //            Geometry.Geometry.GetLineLength(pickPoints[0], pickPoints[1]);
                //    }
                //    if (cmdDist.Value is string)
                //    {
                //        string command = cmdDist.Value as string;
                //        if (command.ToLower().Equals(Command.DIST_MULTI))
                //        {

                //        }
                //    }
                //}
                //-----------------------------------------------------------------------------------------
            }

            return message;
        }
    }
}
