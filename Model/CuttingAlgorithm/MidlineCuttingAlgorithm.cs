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
            result.ResultCode = 0;
            return result;
        }
        public MidlineCuttingResult GetCuttingResult(linesegment barA_Segment, linesegment barB_Segment, double barA_Width, double barB_Width)
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
            result.ResultCode = 0;
            return result;
        }
        private void CalculateCollisionPoints()
        {
            entireColPoints.Clear();

            // BarA : Top -> BarB : Top,Bot
            if (CurtainWallMath.GetLineIsCross(barA_ExtendLT2RT, barA_ExtendRT2LT, barB_ExtendLT2RT, barB_ExtendRT2LT))
                entireColPoints.Add(CurtainWallMath.GetCrossPoint(barA_ExtendLT2RT, barA_ExtendRT2LT, barB_ExtendLT2RT, barB_ExtendRT2LT));
            if (CurtainWallMath.GetLineIsCross(barA_ExtendLT2RT, barA_ExtendRT2LT, barB_ExtendLB2RB, barB_ExtendRB2LB))
                entireColPoints.Add(CurtainWallMath.GetCrossPoint(barA_ExtendLT2RT, barA_ExtendRT2LT, barB_ExtendLB2RB, barB_ExtendRB2LB));

            // BarA : Bot -> BarB : Top, Bot
            if (CurtainWallMath.GetLineIsCross(barA_ExtendLB2RB, barA_ExtendRB2LB, barB_ExtendLT2RT, barB_ExtendRT2LT))
                entireColPoints.Add(CurtainWallMath.GetCrossPoint(barA_ExtendLB2RB, barA_ExtendRB2LB, barB_ExtendLT2RT, barB_ExtendRT2LT));
            if (CurtainWallMath.GetLineIsCross(barA_ExtendLB2RB, barA_ExtendRB2LB, barB_ExtendLB2RB, barB_ExtendRB2LB))
                entireColPoints.Add(CurtainWallMath.GetCrossPoint(barA_ExtendLB2RB, barA_ExtendRB2LB, barB_ExtendLB2RB, barB_ExtendRB2LB));
        }
        private void ManufactureCuttingPoints()
        {
            // Calc Cutting Point
            entireColPoints = DistinctSelf_gPointList(entireColPoints);

            // Get Center Point By 2 Bar's Center(A, B)
            double lenBy2BarCenters = CurtainWallMath.GetLengthBy2Point(barA.Center, barB.Center);
            Vector a2bVec = CurtainWallMath.GetUnitVecBy2Point(barB.Center, barA.Center);
            a2bVec *= (lenBy2BarCenters * 0.5);
            gPoint centerBy2Bars = CurtainWallMath.GetExtendPoint(barA.Center, a2bVec);

            List<PointAndDis> barsCenterNearPoints = Conversion_gPoint2PointAndDisList(centerBy2Bars);
            barsCenterNearPoints = barsCenterNearPoints.OrderBy(obj => obj.Distance).ToList();
            result.FirstCutPoint = barsCenterNearPoints[0].Point;
            result.SecondCutPoint = barsCenterNearPoints[barsCenterNearPoints.Count - 1].Point;

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
            CalcExtendLength("A");
            CalcExtendLength("B");

            // Calc CuttingAngle
            result.BarA_CutAngle = CalcCuttingAngle(barA, result.FirstCutPoint, result.SecondCutPoint);
            result.BarB_CutAngle = CalcCuttingAngle(barB, result.FirstCutPoint, result.SecondCutPoint);
        }
        private double CalcCuttingAngle(Bar targetBar, gPoint targetBar_FirstCutPoint, gPoint targetBar_SecondCutPoint)
        {
            // BarA
            gPoint secondCutPoint = new gPoint(targetBar_SecondCutPoint);
            gPoint originPoint = null;
            double rot = targetBar.Rotation * -1;
            secondCutPoint = CurtainWallMath.GetRotatedPoint(rot, secondCutPoint, targetBar.Center);
            if (secondCutPoint.y <= targetBar.Center.y)
            {
                if (secondCutPoint.x <= targetBar.Center.x)
                    originPoint = targetBar.RB;
                else
                    originPoint = targetBar.LB;
            }
            else
            {
                if (secondCutPoint.x <= targetBar.Center.x)
                    originPoint = targetBar.RT;
                else
                    originPoint = targetBar.LT;
            }

            rot *= -1;
            Vector cutVec = CurtainWallMath.GetUnitVecBy2Point(targetBar_FirstCutPoint, targetBar_SecondCutPoint);
            Vector originVec = CurtainWallMath.GetUnitVecBy2Point(originPoint, targetBar_SecondCutPoint);
            double cutAngle = cutVec.Dot(originVec);
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
        private void CalcExtendLength(string barType)
        {
            if (barType.Equals("A"))
                result.BarA_ExtendLength = GetOrigin2SecondCutPointLen(barA);
            else
                result.BarB_ExtendLength = GetOrigin2SecondCutPointLen(barB);
        }
        private double GetOrigin2SecondCutPointLen(Bar targetBar)
        {
            double rot = targetBar.Rotation * -1;
            gPoint secondCutPoint = CurtainWallMath.GetRotatedPoint(rot, result.SecondCutPoint, targetBar.Center);

            double origin2SecondNearestPointLen = 0;
            if (secondCutPoint.y > targetBar.Center.y)
            {
                if (secondCutPoint.x <= targetBar.Center.x)
                {
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.LT);
                }
                else if (secondCutPoint.x > targetBar.Center.x)
                {
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.RT);
                }
            }
            else if (secondCutPoint.y <= targetBar.Center.y)
            {
                if (secondCutPoint.x <= targetBar.Center.x)
                {
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.LB);
                }
                else if (secondCutPoint.x > targetBar.Center.x)
                {
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(result.SecondCutPoint, targetBar.RB);
                }
            }
            return origin2SecondNearestPointLen;
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
