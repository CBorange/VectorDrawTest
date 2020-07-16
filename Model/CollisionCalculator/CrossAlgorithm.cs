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
using System.Drawing;

namespace MathPractice.Model.CollisionCalculator
{
    public class CrossAlgorithm : ICollisionAlgorithm
    {
        private MathSupporter math;
        public CrossAlgorithm()
        {
            math = MathSupporter.Instance;
        }
        public void CalcAlgorithm_CuttingRect(Beam verBeam, Beam horBeam)
        {
            List<gPoint> cuttingPointList = new List<gPoint>();
            gPoint[] expandedPoints = new gPoint[4];
            // VerBar Point
            gPoint verEX_RT2LT = math.GetExpandedPointBy2Points(verBeam.RightTop, verBeam.LeftTop, 5);
            gPoint verEX_RB2LB = math.GetExpandedPointBy2Points(verBeam.RightBottom, verBeam.LeftBottom, 5);
            gPoint verEX_RT2RB = math.GetExpandedPointBy2Points(verBeam.RightTop, verBeam.RightBottom, 5);
            gPoint verEX_LT2LB = math.GetExpandedPointBy2Points(verBeam.LeftTop, verBeam.LeftBottom, 5);

            // HorBar Point
            gPoint horEX_RT2LT = math.GetExpandedPointBy2Points(horBeam.RightTop, horBeam.LeftTop, 5);
            gPoint horEX_RB2LB = math.GetExpandedPointBy2Points(horBeam.RightBottom, horBeam.LeftBottom, 5);

            gPoint horEX_LT2RT = math.GetExpandedPointBy2Points(horBeam.LeftTop, horBeam.RightTop, 5);
            gPoint horEX_LB2RB = math.GetExpandedPointBy2Points(horBeam.LeftBottom, horBeam.RightBottom, 5);

            // 우측
            if (horBeam.Center.x > verBeam.Center.x - 1)
            {
                double horRT2LT_Length = 0;
                double horRB2LB_Length = 0;

                // LT
                gPoint lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.RightTop, horEX_RT2LT);
                if (horBeam.LeftTop.x < verBeam.Top.x)
                    lt = horBeam.LeftTop;
                horRT2LT_Length = math.GetLengthBy2Point(horBeam.RightTop, lt);
                if (lt.y > verBeam.RightTop.y)
                {
                    lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.RightTop, horEX_RT2LT);

                    cuttingPointList.Add(verBeam.RightTop);
                    cuttingPointList.Add(lt);
                    horRT2LT_Length = math.GetLengthBy2Point(horBeam.RightTop, lt) +
                        math.GetLengthBy2Point(verBeam.RightTop, lt);
                }
                cuttingPointList.Add(lt);

                // RT
                gPoint rt = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.RightTop, horEX_RT2LT);
                if (rt.y > verBeam.RightBottom.y)
                {
                    rt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.RightTop, horEX_RT2LT);

                    cuttingPointList.Add(rt);
                    cuttingPointList.Add(verBeam.RightBottom);
                }
                else
                    cuttingPointList.Add(rt);

                // RB
                gPoint rb = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.RightBottom, horEX_RB2LB);
                if (rb.y < verBeam.LeftBottom.y)
                {
                    rb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.RightBottom, horEX_RB2LB);

                    cuttingPointList.Add(verBeam.LeftBottom);
                }
                cuttingPointList.Add(rb);

                // LB
                gPoint lb = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.RightBottom, horEX_RB2LB);
                if (horBeam.LeftBottom.x < verBeam.Top.x)
                    lb = horBeam.LeftBottom;
                horRB2LB_Length = math.GetLengthBy2Point(horBeam.RightBottom, lb);
                if (lb.y < verBeam.LeftTop.y)
                {
                    lb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.RightBottom, horEX_RB2LB);

                    cuttingPointList.Add(lb);
                    cuttingPointList.Add(verBeam.LeftTop);
                    horRB2LB_Length = math.GetLengthBy2Point(horBeam.RightBottom, lb) +
                        math.GetLengthBy2Point(verBeam.LeftTop, lb);
                }
                else
                    cuttingPointList.Add(lb);

                // Cutting Length에 따른 Expanded Point 계산
                if (horRT2LT_Length > horRB2LB_Length)
                {

                    Vector2 horRT2LT_Unit = math.GetUnitVecBy2Point(lt, horBeam.RightTop);
                    Vector2 horRT2LTVec = horRT2LT_Unit * horRT2LT_Length;

                    Vector2 horRB2LB_Unit = math.GetUnitVecBy2Point(lb, horBeam.RightBottom);
                    Vector2 horRB2LBVec = horRB2LB_Unit * horRT2LT_Length;

                    expandedPoints[0] = lt;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.RightTop, horRT2LTVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.RightBottom, horRB2LBVec);
                    expandedPoints[3] = lb;
                }
                else
                {
                    Vector2 horRB2LB_Unit = math.GetUnitVecBy2Point(lb, horBeam.RightBottom);
                    Vector2 horRB2LBVec = horRB2LB_Unit * horRB2LB_Length;

                    Vector2 horRT2LT_Unit = math.GetUnitVecBy2Point(lt, horBeam.RightTop);
                    Vector2 horRT2LTVec = horRT2LT_Unit * horRB2LB_Length;

                    expandedPoints[0] = lb;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.RightBottom, horRB2LBVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.RightTop, horRT2LTVec);
                    expandedPoints[3] = lt;
                }
            }
            // 좌측
            else if (horBeam.Center.x <= verBeam.Center.x - 1)
            {
                double horLT2RT_Length = 0;
                double horLB2RB_Length = 0;

                // LT
                gPoint lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.LeftTop, horEX_LT2RT);
                if (lt.y > verBeam.RightTop.y)
                {
                    lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.LeftTop, horEX_LT2RT);

                    cuttingPointList.Add(verBeam.RightTop);
                }
                cuttingPointList.Add(lt);

                // RT
                gPoint rt = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.LeftTop, horEX_LT2RT);
                if (horBeam.RightTop.x > verBeam.Bottom.x)
                    rt = horBeam.RightTop;
                horLT2RT_Length = math.GetLengthBy2Point(rt, horBeam.LeftTop);
                if (rt.y > verBeam.RightBottom.y)
                {
                    rt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.LeftTop, horEX_LT2RT);

                    cuttingPointList.Add(rt);
                    cuttingPointList.Add(verBeam.RightBottom);
                    horLT2RT_Length = math.GetLengthBy2Point(horBeam.LeftTop, rt) +
                        math.GetLengthBy2Point(verBeam.RightBottom, rt);
                }
                else
                    cuttingPointList.Add(rt);

                // RB
                gPoint rb = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.LeftBottom, horEX_LB2RB);
                if (horBeam.RightBottom.x > verBeam.Bottom.x)
                    rb = horBeam.RightBottom;
                horLB2RB_Length = math.GetLengthBy2Point(horBeam.LeftBottom, rb);
                if (rb.y < verBeam.LeftBottom.y)
                {
                    rb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.LeftBottom, horEX_LB2RB);

                    cuttingPointList.Add(verBeam.LeftBottom);
                    horLB2RB_Length = math.GetLengthBy2Point(horBeam.LeftBottom, rb) +
                        math.GetLengthBy2Point(verBeam.LeftBottom, rb);
                }
                cuttingPointList.Add(rb);

                // LB
                gPoint lb = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.LeftBottom, horEX_LB2RB);
                if (lb.y < verBeam.LeftTop.y)
                {
                    lb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.LeftBottom, horEX_LB2RB);

                    cuttingPointList.Add(lb);
                    cuttingPointList.Add(verBeam.LeftTop);
                }
                else
                    cuttingPointList.Add(lb);

                // Cutting Length에 따른 Expanded Point 계산
                if (horLT2RT_Length > horLB2RB_Length)
                {

                    Vector2 horLT2RT_Unit = math.GetUnitVecBy2Point(rt, horBeam.LeftTop);
                    Vector2 horLT2RTVec = horLT2RT_Unit * horLT2RT_Length;

                    Vector2 horLB2RB_Unit = math.GetUnitVecBy2Point(rb, horBeam.LeftBottom);
                    Vector2 horLB2RBVec = horLB2RB_Unit * horLT2RT_Length;

                    expandedPoints[0] = rt;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.LeftTop, horLT2RTVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.LeftBottom, horLB2RBVec);
                    expandedPoints[3] = rb;
                }
                else
                {
                    Vector2 horLB2RB_Unit = math.GetUnitVecBy2Point(rb, horBeam.LeftBottom);
                    Vector2 horLB2RBVec = horLB2RB_Unit * horLB2RB_Length;

                    Vector2 horLT2RT_Unit = math.GetUnitVecBy2Point(rt, horBeam.LeftTop);
                    Vector2 horLT2RTVec = horLT2RT_Unit * horLB2RB_Length;

                    expandedPoints[0] = rb;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.LeftBottom, horLB2RBVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.LeftTop, horLT2RTVec);
                    expandedPoints[3] = rt;
                }
            }

            gPoint[] cuttingPoints = cuttingPointList.ToArray();

            FigureDrawer cuttingFigure = new FigureDrawer(horBeam.Document);
            cuttingFigure.Visible = true;
            cuttingFigure.DrawColor = Color.Green;
            cuttingFigure.SetPoints(cuttingPoints);
            horBeam.CuttingFigures.Add(cuttingFigure);

            FigureDrawer expandFigure = new FigureDrawer(horBeam.Document);
            expandFigure.Visible = true;
            expandFigure.DrawColor = Color.Yellow;
            expandFigure.SetPoints(expandedPoints);
            horBeam.ExpandFigures.Add(expandFigure);
        }
    }
}
