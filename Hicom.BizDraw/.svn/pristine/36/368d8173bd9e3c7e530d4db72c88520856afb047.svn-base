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

namespace Hicom.BizDraw.Command
{
    public class CmdTrimResult
    {
        public vdFigure Soruce;
        public vdFigure Result;
    }
    public class CmdExtendTrim : ActionBase
    {
        public enum eMethod
        {
            None = 1,
            Common = 2,
            Fence = 3,
            Cross = 4
        }

        //public override vdFigure Entity { get { return Figure; } }
        //private vdFigure Figure { get; set; }

        public gPoints extentPoints { get; set; }
        
        private int IntersectionCount { get; set; }
        private List<gPoint> IntersectionPoints { get; set; }
        private List<vdFigure> IntersectionFigures { get; set; }

        public eMethod Method { get; set; }
        public List<vdFigure> Result { get; set; }
        public vdSelection BoundaryEntities { get; set; }
        public CmdExtendTrim(gPoint reference, vdLayout layout, eMethod method) : base(reference, layout)
        {
            this.Method = method;

            this.extentPoints = new gPoints();
            this.extentPoints.Add(reference);

            this.IntersectionCount = 0;
            this.IntersectionPoints = new List<gPoint>();
            this.IntersectionFigures = new List<vdFigure>();
            this.Result = new List<vdFigure>();
        }

        protected override void OnMyPositionChanged(gPoint newPosition)
        {
        }
        
        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            if (extentPoints.Count > 0)
            {
                switch (this.Method)
                {
                    case eMethod.Common:
                        break;
                    case eMethod.Fence:
                        for (int ix = 0; ix < extentPoints.Count; ix++)
                        {
                            gPoint startPoint = extentPoints[ix];
                            gPoint endPoint = (ix < extentPoints.Count - 1) ? extentPoints[ix + 1] : null;

                            if (startPoint != null && endPoint != null)
                                this.DrawLine(startPoint, endPoint, render);
                        }
                        break;
                    case eMethod.Cross:
                        gPoint firstCurvePoint = extentPoints[0];
                        if(firstCurvePoint != null)
                        {
                            gPoint sePoint = new gPoint(firstCurvePoint.x, orthoPoint.y);
                            gPoint esPoint = new gPoint(orthoPoint.x, firstCurvePoint.y);
                            DrawLine(firstCurvePoint, sePoint, render);
                            DrawLine(sePoint, this.OrthoPoint, render);
                            DrawLine(this.OrthoPoint, esPoint, render);
                            DrawLine(esPoint, firstCurvePoint, render);
                        }
                        break;
                }
            }
        }

        ///<summary>
        ///Extend 또는 Trim을 실행하는 함수
        ///</summary>
        /// <param name="doc">도면 Document</param>
        /// <param name="boundSelection">Entities를 선택한 Selection 정보</param>
        /// <param name="isExtendOrTrim">Extend 또는 Trim 2가지 작동(true : Extend, false : Trime)</param>
        /// <param name="repeat">기능의 반복 여부</param>
        /// <returns>Extend 또는 Trim에 영향을 받은 vdFigure객체 List</returns>
        public static List<object> Run(vdDocument doc, vdSelection boundSelection, bool isExtendOrTrim, bool repeat = false)
        {
            List<object> result = new List<object>();

            if (boundSelection == null || boundSelection.Count < 1)
                return result;

            vdFigure retfig = null;
            gPoint retpt = null;
            StatusCode scode = StatusCode.Cancel;
            eMethod method = eMethod.None;
            List<vdFigure> figs = new List<vdFigure>();
            do
            {
                doc.Prompt(string.Format("Fence(F)/Cross(C)/Project(P)<{0}>/Edge(E)<{1}>", doc.ProjectionTrimExtend, doc.EdgeExtendTrim));
                string fence = string.Format("{0};f;", Command.EXTEND_TRIM_FENCE);
                string cross = string.Format("{0};c;", Command.EXTEND_TRIM_CROSS);
                string project = string.Format("{0};p;", Command.EXTEND_TRIM_PROJECT);
                string edge = string.Format("{0};e;", Command.EXTEND_TRIM_EDGE);
                doc.ActionUtility.SetAcceptedStringValues(new string[] { fence, cross, project, edge }, Command.EXTEND_TRIM_CROSS);
                
                string subcommand = doc.ActionUtility.getUserEntityWithStringValuesOneClick(out retfig, out retpt, vdDocument.LockLayerMethodEnum.EnableAddToSelections, true);
                switch (subcommand.ToLower())
                {
                    case Command.EXTEND_TRIM_PROJECT:
                        doc.Prompt(string.Format("ProjectOption<{0}> None(N)/UCS(U)/View(V)", doc.ProjectionTrimExtend));
                        string none = string.Format("{0};n;", Command.EXTEND_TRIM_PROJECT_NONE);
                        string ucs = string.Format("{0};u;", Command.EXTEND_TRIM_PROJECT_UCS);
                        string view = string.Format("{0};v;", Command.EXTEND_TRIM_PROJECT_VIEW);
                        doc.ActionUtility.SetAcceptedStringValues(new string[] { none, ucs, view }, Command.EXTEND_TRIM_PROJECT_NONE);
                        string projectOption = doc.ActionUtility.getUserString();
                        switch (projectOption.ToLower())
                        {
                            case Command.EXTEND_TRIM_PROJECT_NONE:
                                doc.ProjectionTrimExtend = vdDocument.ProjectionTrimExtendEnum.None;
                                break;
                            case Command.EXTEND_TRIM_PROJECT_UCS:
                                doc.ProjectionTrimExtend = vdDocument.ProjectionTrimExtendEnum.UCS;
                                break;
                            case Command.EXTEND_TRIM_PROJECT_VIEW:
                                doc.ProjectionTrimExtend = vdDocument.ProjectionTrimExtendEnum.View;
                                break;
                        }
                        method = eMethod.None;
                        break;
                    case Command.EXTEND_TRIM_EDGE:
                        doc.EdgeExtendTrim = !doc.EdgeExtendTrim;
                        method = eMethod.None;
                        break;
                    case Command.EXTEND_TRIM_FENCE:
                        doc.Prompt("First Fence Point");
                        scode = doc.ActionUtility.getUserPoint(out retpt);
                        method = eMethod.Fence;
                        break;
                    case Command.EXTEND_TRIM_CROSS:
                        doc.Prompt("Start Curve Point");
                        scode = doc.ActionUtility.getUserPoint(out retpt);
                        method = eMethod.Cross;
                        break;
                    default: //case "":
                        if (retfig == null) // 선택된 Entity가 없는 경우 Cross로 연결
                        {
                            scode = (retpt == null) ? StatusCode.Cancel : StatusCode.Success;
                            method = eMethod.Cross;
                        }
                        else
                        {
                            figs.Add(retfig);
                            scode = StatusCode.Success;
                            method = eMethod.Common;
                        }
                        break;
                }
            } while (method == eMethod.None);

            if (scode != StatusCode.Success)
                return result;
            
            CmdExtendTrim cmdExtendTrim = new CmdExtendTrim(retpt, doc.ActiveLayOut, method);
            cmdExtendTrim.BoundaryEntities = boundSelection;
            if (cmdExtendTrim.Method == eMethod.Common)
                IntersectCommon(cmdExtendTrim, retpt, figs);
            else if (cmdExtendTrim.Method == eMethod.Fence)
                IntersectFence(cmdExtendTrim, doc);
            else if (cmdExtendTrim.Method == eMethod.Cross)
                intersectCross(cmdExtendTrim, doc);
            else
                return result;
            
            cmdExtendTrim.ExtendTrim(doc, boundSelection, out result, isExtendOrTrim);
            if(result.Count > 0)
                doc.Redraw(true);

            return result;
        }

        public void AddIntersectionValue(gPoint retpt, vdFigure retfig)
        {
            this.IntersectionCount++;
            this.IntersectionPoints.Add(retpt);
            this.IntersectionFigures.Add(retfig);
        }

        private static void IntersectCommon(CmdExtendTrim cmdExtendTrim, gPoint retpt, List<vdFigure> retfigs)
        {
            retfigs.ForEach(s => cmdExtendTrim.AddIntersectionValue(retpt, s));
        }

        private static void IntersectFence(CmdExtendTrim cmdExtendTrim, vdDocument doc)
        {
            while (cmdExtendTrim.WaitToFinish("Next Fence Pont", valueType.REFPOINT, true) == StatusCode.Success)
            {
                if (cmdExtendTrim.Value is gPoint)
                    cmdExtendTrim.extentPoints.Add(cmdExtendTrim.Value);
            }

            if (cmdExtendTrim.extentPoints.Count > 1)
                cmdExtendTrim.IntersectContinuousLine(doc, cmdExtendTrim.extentPoints);
        }

        private static void intersectCross(CmdExtendTrim cmdExtendTrim, vdDocument doc)
        {
            if (cmdExtendTrim.WaitToFinish("End Curve Point", valueType.REFPOINT, true) == StatusCode.Success)
            {
                if (cmdExtendTrim.Value is gPoint)
                    cmdExtendTrim.extentPoints.Add(cmdExtendTrim.Value);
            }

            if (cmdExtendTrim.extentPoints.Count > 1)
            {
                gPoint startPoint = cmdExtendTrim.extentPoints[0];
                gPoint endPoint = cmdExtendTrim.extentPoints[1];
                gPoint sePoint = new gPoint(startPoint.x, endPoint.y);
                gPoint esPoint = new gPoint(endPoint.x, startPoint.y);

                gPoints crossExtentPoints = new gPoints();
                crossExtentPoints.Add(startPoint);
                crossExtentPoints.Add(sePoint);
                crossExtentPoints.Add(endPoint);
                crossExtentPoints.Add(esPoint);
                crossExtentPoints.Add(startPoint);// 닫힘이기 때문에 마지막에 시작점을 넣는다.

                cmdExtendTrim.IntersectContinuousLine(doc, crossExtentPoints);
            }
        }

        private void IntersectContinuousLine(vdDocument doc, gPoints linePoints)
        {
            Box box = new Box();
            box.AddPoints(linePoints);
            SelectedObjectArray selectEntities = doc.ActiveLayOut.GetTransformedSelectedObjects(box);
            for (int ix = 0; ix < selectEntities.Count; ix++)
            {
                vdFigure selectFigure = selectEntities[ix].SelectedEntity.Entity;
                gPoints ptCross = new gPoints();
                for (int iy = 0; iy < linePoints.Count; iy++)
                {
                    gPoint lineStartPoint = linePoints[iy];
                    gPoint lineEndPoint = (iy < linePoints.Count - 1) ? linePoints[iy + 1] : null;

                    if (lineStartPoint != null && lineEndPoint != null)
                    {
                        gPoints points = new gPoints();
                        vdLine line = new vdLine(doc, lineStartPoint, lineEndPoint);

                        if (selectFigure.IntersectWith(line, VdConstInters.VdIntOnBothOperands, points) && points.Count > 0)
                            ptCross.Add(points[0]);
                    }
                }

                if (ptCross.Count > 1)
                {
                    gPoint nearPoint;
                    if (FindNearPoint(BoundaryEntities, ptCross, out nearPoint))
                        AddIntersectionValue(nearPoint, selectFigure);
                }
                else if (ptCross.Count == 1)
                {
                    AddIntersectionValue(ptCross[0], selectFigure);
                }
            }
        }

        public bool FindNearPoint(vdSelection figures, gPoints points, out gPoint result)
        {
            result = null;
            double distMin = double.MaxValue;

            Line line = new Line(points[0], points[1]);

            foreach (vdFigure fig in figures)
            {
                Box boundary = fig.BoundingBox;
                List<linesegment> segments = boundary.GetLinesegments();

                foreach (linesegment segment in segments)
                {
                    gPoint cross = new gPoint();

                    if (segment.IntersectionLL2D(line, cross) > 0 && segment.IsInsideLineSegment(cross))
                    {
                        double dist1 = cross.Distance2D(points[0]);
                        double dist2 = cross.Distance2D(points[1]);
                        gPoint pt = dist1 < dist2 ? points[0] : points[1];
                        if (distMin > Math.Min(dist1, dist2))
                        {
                            distMin = Math.Min(dist1, dist2);
                            result = pt;
                        }
                    }
                }
            }

            return result == null ? false : true;
        }

        public void ExtendTrim(vdDocument doc, vdSelection boundSelection, out List<object> result, bool isExtend)
        {
            result = new List<object>();
            try
            {
                for (int ix = 0; ix < IntersectionCount; ix++)
                {
                    vdFigure retfig = this.IntersectionFigures.Count > ix ? this.IntersectionFigures[ix] : null;
                    gPoint retpt = this.IntersectionPoints.Count > ix ? this.IntersectionPoints[ix] : null;

                    if (retfig == null || retpt == null)
                        break;

                    if (isExtend)
                    {
                        if (doc.CommandAction.CmdExtend(boundSelection, new object[] { retfig, retpt }, doc.EdgeExtendTrim))
                            result.Add(retfig);
                    }
                    else
                    {
                        doc.OnAfterAddItem += new vdDocument.AfterAddItemEventHandler(OnAfterAddItem);
                        if (doc.CommandAction.CmdTrim(boundSelection, new object[] { retfig, retpt }, doc.EdgeExtendTrim))
                        {
                            if(Result.Count > 0)
                            {
                                CmdTrimResult ret = new CmdTrimResult();
                                ret.Soruce = retfig;
                                ret.Result = Result[0];
                                result.Add(ret);
                            }
                        }                            
                        doc.OnAfterAddItem -= new vdDocument.AfterAddItemEventHandler(OnAfterAddItem);
                    }
                }
                
            }
            catch(VectorDraw.Professional.Exceptions.vdGeneralException e)
            {
                string message = e.Message;
            }

        }

        private void OnAfterAddItem(object obj)
        {
            Result.Clear();
            if (obj is vdFigure)
            {
                vdFigure fig = obj as vdFigure;
                Result.Add(fig);
            }
        }
    }
}
