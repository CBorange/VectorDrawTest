using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using System.Drawing;
using VectorDraw.Professional.PropertyList;
using VectordrawTest.Model.Manager;
using VectordrawTest.Model.CustomFigure;
using System.Diagnostics;
using Hicom.BizDraw.Geometry;

namespace VectordrawTest.Model.CuttingAlgorithm
{
    public class MidlineCuttingResult
    {
        public MidlineCuttingResult()
        {
            BarA_CutAngle = 0;
            BarA_ExtendLength = 0;
            BarA_ExtendDir = string.Empty;

            BarB_CutAngle = 0;
            BarB_ExtendLength = 0;
            BarB_ExtendDir = string.Empty;

            ResultCode = 0;
            FirstCutPoint = null;
            SecondCutPoint = null;
        }

        public double BarA_CutAngle;
        public double BarA_ExtendLength;
        public string BarA_ExtendDir;

        public double BarB_CutAngle;
        public double BarB_ExtendLength;
        public string BarB_ExtendDir;

        public int ResultCode;
        public gPoint FirstCutPoint;
        public gPoint SecondCutPoint;
    }
    public class MidlineCuttingAlgorithm
    {
        private CurtainWallMath math;
        private MidlineCuttingResult result;

        private Bar barA;
        private Bar barB;

        // 계산용 확장 지점
        private gPoint barA_ExtendRT2LT;
        private gPoint barA_ExtendLT2RT;
        private gPoint barA_ExtendRB2LB;
        private gPoint barA_ExtendLB2RB;

        private gPoint barB_ExtendRT2LT;
        private gPoint barB_ExtendLT2RT;
        private gPoint barB_ExtendRB2LB;
        private gPoint barB_ExtendLB2RB;

        private List<gPoint> entireColPoints;
        private List<PointAndDis> barAColPoints;
        private List<PointAndDis> barBColPoints;

        public MidlineCuttingAlgorithm()
        {
            math = new CurtainWallMath();
            entireColPoints = new List<gPoint>();
        }

        public MidlineCuttingResult GetCuttingResult(gPoint barA_StartPoint, gPoint barA_EndPoint, double barA_Width, double barA_Length,
            gPoint barB_StartPoint, gPoint barB_EndPoint, double barB_Width, double barB_Length)
        {
            try
            {
                result = new MidlineCuttingResult();
                barA = CuttingUtil.CreateBar(barA_StartPoint, barA_EndPoint, barA_Width, barA_Length);
                barB = CuttingUtil.CreateBar(barB_StartPoint, barB_EndPoint, barB_Width, barB_Length);

                // 계산용 확장 선 산출
                barA_ExtendRT2LT = CurtainWallMath.GetExtendedPointBy2Points(barA.RT, barA.LT, 1000000);
                barA_ExtendLT2RT = CurtainWallMath.GetExtendedPointBy2Points(barA.LT, barA.RT, 1000000);
                barA_ExtendRB2LB = CurtainWallMath.GetExtendedPointBy2Points(barA.RB, barA.LB, 1000000);
                barA_ExtendLB2RB = CurtainWallMath.GetExtendedPointBy2Points(barA.LB, barA.RB, 1000000);

                barB_ExtendRT2LT = CurtainWallMath.GetExtendedPointBy2Points(barB.RT, barB.LT, 1000000);
                barB_ExtendLT2RT = CurtainWallMath.GetExtendedPointBy2Points(barB.LT, barB.RT, 1000000);
                barB_ExtendRB2LB = CurtainWallMath.GetExtendedPointBy2Points(barB.RB, barB.LB, 1000000);
                barB_ExtendLB2RB = CurtainWallMath.GetExtendedPointBy2Points(barB.LB, barB.RB, 1000000);

                CalculateCollisionPoints();
                ManufactureCuttingPoints();
                //AngleBasedCuttingAlgorithm();

                return result;
            }
            catch(Exception)
            {
                result.ResultCode = -2;
                return result;
            }
        }
        private void AngleBasedCuttingAlgorithm()
        {
            //BarA <->BarB의 BaseLine 사이각 계산, BarA, BarB의 BaseLine이 부딪히는 지점 계산
            gPoint[] barBLPoints = new gPoint[4];
            barBLPoints[0] = barA.Left;
            barBLPoints[1] = barA.Right;
            barBLPoints[2] = barB.Left;
            barBLPoints[3] = barB.Right;
            gPoint barLinkPoint = CuttingUtil.GetDuplicatePointOnArray(barBLPoints);
            if (barLinkPoint == null)
            {
                result.ResultCode = -1;
            }

            gPoint barASymmetryPoint = barA.Left;
            if (CuttingUtil.IsSamePoint(barA.Left, barLinkPoint))
                barASymmetryPoint = barA.Right;

            gPoint barBSymmetryPoint = barB.Left;
            if (CuttingUtil.IsSamePoint(barB.Left, barLinkPoint))
                barBSymmetryPoint = barB.Right;

            Vector barAVec = CurtainWallMath.GetUnitVecBy2Point(barASymmetryPoint, barLinkPoint);
            Vector barBVec = CurtainWallMath.GetUnitVecBy2Point(barBSymmetryPoint, barLinkPoint);
            double blAngle = barAVec.Dot(barBVec);
            blAngle = Math.Acos(blAngle);
            blAngle = Globals.RadiansToDegrees(blAngle);
            blAngle = Math.Abs(blAngle);

            double halfBLAngle = (blAngle * 0.5);
        }
        public MidlineCuttingResult GetCuttingResult(linesegment barA_Segment, linesegment barB_Segment, double barA_Width, double barB_Width)
        {
            try
            {
                result = new MidlineCuttingResult();
                barA = CuttingUtil.CreateBar(barA_Segment.StartPoint, barA_Segment.EndPoint, barA_Width, barA_Segment.length);
                barB = CuttingUtil.CreateBar(barB_Segment.StartPoint, barB_Segment.EndPoint, barB_Width, barB_Segment.length);

                // 계산용 확장 선 산출
                barA_ExtendRT2LT = CurtainWallMath.GetExtendedPointBy2Points(barA.RT, barA.LT, 1000000);
                barA_ExtendLT2RT = CurtainWallMath.GetExtendedPointBy2Points(barA.LT, barA.RT, 1000000);
                barA_ExtendRB2LB = CurtainWallMath.GetExtendedPointBy2Points(barA.RB, barA.LB, 1000000);
                barA_ExtendLB2RB = CurtainWallMath.GetExtendedPointBy2Points(barA.LB, barA.RB, 1000000);

                barB_ExtendRT2LT = CurtainWallMath.GetExtendedPointBy2Points(barB.RT, barB.LT, 1000000);
                barB_ExtendLT2RT = CurtainWallMath.GetExtendedPointBy2Points(barB.LT, barB.RT, 1000000);
                barB_ExtendRB2LB = CurtainWallMath.GetExtendedPointBy2Points(barB.RB, barB.LB, 1000000);
                barB_ExtendLB2RB = CurtainWallMath.GetExtendedPointBy2Points(barB.LB, barB.RB, 1000000);

                CalculateCollisionPoints();
                ManufactureCuttingPoints();

                return result;
            }
            catch(Exception)
            {
                result.ResultCode = -2;
                return result;
            }
        }
        private void CalculateCollisionPoints()
        {
            entireColPoints.Clear();

            linesegment barAExtendTop = new linesegment(barA_ExtendLT2RT, barA_ExtendRT2LT);
            linesegment barAExtendBot = new linesegment(barA_ExtendLB2RB, barA_ExtendRB2LB);
            linesegment barBExtendTop = new linesegment(barB_ExtendLT2RT, barB_ExtendRT2LT);
            linesegment barBExtendBot = new linesegment(barB_ExtendLB2RB, barB_ExtendRB2LB);

            // BarA : Top -> BarB : Top,Bot
            gPoint col_ATop_To_BTop = new gPoint();
            if (Geometry.Intersection(barAExtendTop, barBExtendTop, col_ATop_To_BTop) == 1) entireColPoints.Add(col_ATop_To_BTop);

            gPoint col_ATop_To_BBot = new gPoint();
            if (Geometry.Intersection(barAExtendTop, barBExtendBot, col_ATop_To_BBot) == 1) entireColPoints.Add(col_ATop_To_BBot);

            // BarA : Bot -> BarB : Top, Bot
            gPoint col_ABot_To_BTop = new gPoint();
            if (Geometry.Intersection(barAExtendBot, barBExtendTop, col_ABot_To_BTop) == 1) entireColPoints.Add(col_ABot_To_BTop);

            gPoint col_ABot_To_BBot = new gPoint();
            if (Geometry.Intersection(barAExtendBot, barBExtendBot, col_ABot_To_BBot) == 1) entireColPoints.Add(col_ABot_To_BBot);
        }
        private void ManufactureCuttingPoints()
        {
            // Calc Cutting Point
            entireColPoints = DistinctSelf_gPointList(entireColPoints);

            // BarA <-> BarB의 BaseLine 사이각 계산, BarA, BarB의 BaseLine이 부딪히는 지점 계산
            gPoint[] barBLPoints = new gPoint[4];
            barBLPoints[0] = barA.Left;
            barBLPoints[1] = barA.Right;
            barBLPoints[2] = barB.Left;
            barBLPoints[3] = barB.Right;
            gPoint barLinkPoint = CuttingUtil.GetDuplicatePointOnArray(barBLPoints);
            if (barLinkPoint == null)
            {
                result.ResultCode = -1;
                return;
            }

            gPoint barASymmetryPoint = barA.Left;
            if (CuttingUtil.IsSamePoint(barA.Left, barLinkPoint))
                barASymmetryPoint = barA.Right;

            gPoint barBSymmetryPoint = barB.Left;
            if (CuttingUtil.IsSamePoint(barB.Left, barLinkPoint))
                barBSymmetryPoint = barB.Right;

            Vector barAVec = CurtainWallMath.GetUnitVecBy2Point(barASymmetryPoint, barLinkPoint);
            Vector barBVec = CurtainWallMath.GetUnitVecBy2Point(barBSymmetryPoint, barLinkPoint);
            double blAngle = barAVec.Dot(barBVec);
            blAngle = Math.Acos(blAngle);
            blAngle = Globals.RadiansToDegrees(blAngle);
            blAngle = Math.Abs(blAngle);

            // 각 Bar 사이의 지점 계산(더 짧은 바의 Center 기준)
            Bar shortBar = barA;
            Bar longBar = barB;
            if (barA.Length > barB.Length)
            {
                shortBar = barB;
                longBar = barA;
            }
            Vector link2LongBarCenter = CurtainWallMath.GetUnitVecBy2Point(longBar.Center, barLinkPoint);
            link2LongBarCenter *= (shortBar.Length * 0.5);
            gPoint longBarCenter = CurtainWallMath.GetExtendPoint(barLinkPoint, link2LongBarCenter);

            double lenBy2BarCenters = CurtainWallMath.GetLengthBy2Point(shortBar.Center, longBarCenter);
            Vector short2LongVec = CurtainWallMath.GetUnitVecBy2Point(longBarCenter, shortBar.Center);
            short2LongVec *= (lenBy2BarCenters * 0.5);
            gPoint centerBy2Bars = CurtainWallMath.GetExtendPoint(shortBar.Center, short2LongVec);

            // BarA <-> BarB BL 사이 각에 따라 CuttingPoint 산출
            // 예각
            if (blAngle <= 90)
            {
                List<PointAndDis> barsCenterNearPoints = Conversion_gPoint2PointAndDisList(centerBy2Bars);
                barsCenterNearPoints = barsCenterNearPoints.OrderBy(obj => obj.Distance).ToList();
                result.FirstCutPoint = barsCenterNearPoints[0].Point;
                result.SecondCutPoint = barsCenterNearPoints[barsCenterNearPoints.Count - 1].Point;
            }
            // 둔각
            else if (blAngle > 90)
            {
                List<PointAndDis> barsCenterNearPoints = Conversion_gPoint2PointAndDisList(centerBy2Bars);
                barsCenterNearPoints = barsCenterNearPoints.OrderBy(obj => obj.Distance).ToList();
                result.FirstCutPoint = barsCenterNearPoints[0].Point;

                List<PointAndDis> barLinkNearPoints = Conversion_gPoint2PointAndDisList(barLinkPoint);
                barLinkNearPoints = barLinkNearPoints.OrderBy(obj => obj.Distance).ToList();

                result.SecondCutPoint = barLinkNearPoints[0].Point;
                if (CuttingUtil.IsSamePoint(result.FirstCutPoint, result.SecondCutPoint))
                    result.SecondCutPoint = barLinkNearPoints[1].Point;
            }

            // BarA
            barAColPoints = Conversion_gPoint2PointAndDisList(barA);
            SearchAdjacentCuttingPoints(barA, barAColPoints, "A");
            barAColPoints = barAColPoints.OrderBy(obj => obj.Distance).ToList();

            if (barAColPoints.Count > 2)
                barAColPoints.RemoveRange(2, barAColPoints.Count - 2);
            for (int i = 0; i < barAColPoints.Count; ++i)
                barAColPoints[i].Distance = CurtainWallMath.GetLengthBy2Point(barA.Center, barAColPoints[i].Point);
            barAColPoints = barAColPoints.OrderBy(obj => obj.Distance).ToList();

            // BarB
            barBColPoints = Conversion_gPoint2PointAndDisList(barB);
            SearchAdjacentCuttingPoints(barB, barBColPoints, "B");
            barBColPoints = barBColPoints.OrderBy(obj => obj.Distance).ToList();

            if (barBColPoints.Count > 2)
                barBColPoints.RemoveRange(2, barBColPoints.Count - 2);
            for (int i = 0; i < barBColPoints.Count; ++i)
                barBColPoints[i].Distance = CurtainWallMath.GetLengthBy2Point(barB.Center, barBColPoints[i].Point);
            barBColPoints = barBColPoints.OrderBy(obj => obj.Distance).ToList();

            // Calc Extend Length
            result.BarA_ExtendLength = GetLengthSecondCut2Near(barA);
            result.BarB_ExtendLength = GetLengthSecondCut2Near(barB);

            // Calc CuttingAngle
            result.BarA_CutAngle = GetCuttingAngle(barA, result.FirstCutPoint, result.SecondCutPoint);
            result.BarB_CutAngle = GetCuttingAngle(barB, result.FirstCutPoint, result.SecondCutPoint);

            result.ResultCode = 0;
        }
        private double GetCuttingAngle(Bar targetBar, gPoint targetBar_FirstCutPoint, gPoint targetBar_SecondCutPoint)
        {
            // BarA
            gPoint secondCutPoint = new gPoint(targetBar_SecondCutPoint);
            gPoint scndCutSymmetryPoint = CuttingUtil.GetBarVertexSymmetryPoint(secondCutPoint, targetBar);

            Vector cutVec = CurtainWallMath.GetUnitVecBy2Point(targetBar_FirstCutPoint, targetBar_SecondCutPoint);
            Vector toSymmetryVec = CurtainWallMath.GetUnitVecBy2Point(scndCutSymmetryPoint, targetBar_SecondCutPoint);
            double cutAngle = cutVec.Dot(toSymmetryVec);
            cutAngle = Math.Acos(cutAngle);
            cutAngle = Globals.RadiansToDegrees(cutAngle);
            return cutAngle;
        }
        private void SearchAdjacentCuttingPoints(Bar targetBar, List<PointAndDis> pointAndDisList, string barType)
        {
            double left2PointsLengthSum = 0;
            double right2PointsLengthSum = 0;

            // Left 또는 Right Point <-> 각 충돌 Point 거리 계산 해서 확장 위치 산출
            for (int i = 0; i < pointAndDisList.Count; ++i)
            {
                left2PointsLengthSum += CurtainWallMath.GetLengthBy2Point(targetBar.Left, pointAndDisList[i].Point);
                right2PointsLengthSum += CurtainWallMath.GetLengthBy2Point(targetBar.Right, pointAndDisList[i].Point);
            }

            if (left2PointsLengthSum < right2PointsLengthSum)
            {
                for (int i = 0; i < pointAndDisList.Count; ++i)
                    pointAndDisList[i].Distance = CurtainWallMath.GetLengthBy2Point(targetBar.Left, pointAndDisList[i].Point);
                if (barType.Equals("A"))
                    result.BarA_ExtendDir = "LEFT";
                else
                    result.BarB_ExtendDir = "LEFT";
            }
            else
            {
                for (int i = 0; i < pointAndDisList.Count; ++i)
                    pointAndDisList[i].Distance = CurtainWallMath.GetLengthBy2Point(targetBar.Right, pointAndDisList[i].Point);
                if (barType.Equals("A"))
                    result.BarA_ExtendDir = "RIGHT";
                else
                    result.BarB_ExtendDir = "RIGHT";
            }

        }

        private double GetLengthSecondCut2Near(Bar targetBar)
        {
            List<PointAndDis> vertexPoints = new List<PointAndDis>(4);
            vertexPoints.Add(new PointAndDis(targetBar.LT, CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.LT)));
            vertexPoints.Add(new PointAndDis(targetBar.LB, CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.LB)));
            vertexPoints.Add(new PointAndDis(targetBar.RT, CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.RT)));
            vertexPoints.Add(new PointAndDis(targetBar.RB, CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.RB)));
            vertexPoints = vertexPoints.OrderBy(obj => obj.Distance).ToList();

            return vertexPoints[0].Distance;
        }
        

        private List<PointAndDis> Conversion_gPoint2PointAndDisList(gPoint center)
        {
            List<PointAndDis> newList = new List<PointAndDis>();
            for (int i = 0; i < entireColPoints.Count; ++i)
            {
                PointAndDis pointAndDis = new PointAndDis();
                pointAndDis.Point = entireColPoints[i];
                pointAndDis.Distance = CurtainWallMath.GetLengthBy2Point(center, entireColPoints[i]);
                newList.Add(pointAndDis);
            }
            return newList;
        }
        private List<PointAndDis> Conversion_gPoint2PointAndDisList(Bar bar)
        {
            List<PointAndDis> newList = new List<PointAndDis>();
            for (int i = 0; i < entireColPoints.Count; ++i)
            {
                PointAndDis pointAndDis = new PointAndDis();
                pointAndDis.Point = entireColPoints[i];
                pointAndDis.Distance = CurtainWallMath.GetLengthBy2Point(bar.Center, entireColPoints[i]);
                newList.Add(pointAndDis);
            }
            return newList;
        }
        private List<gPoint> DistinctSelf_gPointList(List<gPoint> originList)
        {
            List<gPoint> resultList = new List<gPoint>();
            for (int i = 0; i < originList.Count; ++i)
            {
                bool duplicate = false;
                for (int j = 0; j < i; ++j)
                {
                    if (CurtainWallMath.CompareDouble(originList[i].x, originList[j].x) &&
                        CurtainWallMath.CompareDouble(originList[i].y, originList[j].y)) 
                    {
                        duplicate = true;
                        break;
                    }
                }
                if (!duplicate)
                    resultList.Add(originList[i]);
            }
            return resultList;
        }
    }
}
