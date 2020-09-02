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
    public class UpCuttingResult
    {
        public UpCuttingResult()
        {
            CutAngle = 0;
            ExtendLength = 0;
            FirstCutPoint = null;
            SecondCutPoint = null;
            ExtendPoint = null;
            ResultCode = 0;
            HasExtend = false;
            ExtendDir = string.Empty;
        }
        public double CutAngle;
        public double ExtendLength;
        public gPoint FirstCutPoint;
        public gPoint SecondCutPoint;
        public gPoint ExtendPoint;
        public int ResultCode;
        public bool HasExtend;
        public string ExtendDir;
    }
    public class UpCuttingAlgorithm
    {
        private CurtainWallMath math;
        private UpCuttingResult result;
        private Bar upBar;
        private Bar cutBar;
        private bool needExtend;

        // 계산용 확장 지점
        private gPoint rt2ltExtend;
        private gPoint rb2lbExtend;
        private gPoint lt2rtExtend;
        private gPoint lb2rbExtend;

        private gPoint lt2lbExtend;
        private gPoint rt2rbExtend;
        private gPoint lb2ltExtend;
        private gPoint rb2rtExtend;

        private List<gPoint> entireColPoints;
        private List<PointAndDis> colPointsAndDisList;

        public UpCuttingAlgorithm()
        {
            math = new CurtainWallMath();
        }
        public UpCuttingResult GetCuttingResult(gPoint upBar_StartPoint, gPoint upBar_EndPoint, gPoint cutBar_StartPoint, gPoint cutBar_EndPoint,
            double upBarWidth, double cutBarWidth, double upBarLength, double cutBarLength)
        {
            result = new UpCuttingResult();

            upBar = CuttingUtil.CreateBar(upBar_StartPoint, upBar_EndPoint, upBarWidth, upBarLength);
            cutBar = CuttingUtil.CreateBar(cutBar_StartPoint, cutBar_EndPoint, cutBarWidth, cutBarLength);
            needExtend = false;
            entireColPoints = new List<gPoint>();

            // 확장 여부 검사
            int colPointCount = 0;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.LT, upBar))
                colPointCount += 1;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.LB, upBar))
                colPointCount += 1;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.RT, upBar))
                colPointCount += 1;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.RB, upBar))
                colPointCount += 1;

            if (colPointCount == 1)
                needExtend = true;

            // 검사 용 각 꼭짓점 확장 지점 생성
            rt2ltExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RT, cutBar.LT, 1000000);
            rb2lbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RB, cutBar.LB, 1000000);
            lt2rtExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LT, cutBar.RT, 1000000);
            lb2rbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LB, cutBar.RB, 1000000);
            lt2lbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LT, cutBar.LB, 1000000);
            rt2rbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RT, cutBar.RB, 1000000);
            lb2ltExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LB, cutBar.LT, 1000000);
            rb2rtExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RB, cutBar.RT, 1000000);

            int checkResult = CheckCalculatable();
            if (checkResult != 0)
            {
                result.ResultCode = checkResult;
                return result;
            }
            entireColPoints.Clear();
            CuttingProcess_Left2Right();
            CuttingProcess_Right2Left();
            colPointsAndDisList = ManufactureCollisionPoint(entireColPoints);

            if (needExtend)
            {
                double origin2SecondNearPointLen = GetOrigin2SecondNearPointLen();
                if (origin2SecondNearPointLen != -1)
                {
                    result.ExtendLength = origin2SecondNearPointLen - cutBar.Length;
                }
            }

            if (result.ExtendLength < 0)
            {
                result.ResultCode = -3;
                return result;
            }
            result.CutAngle = GetCuttingAngle();
            if (needExtend)
                result.HasExtend = true;
            return result;
        }
        public UpCuttingResult GetCuttingResult(linesegment upBarSegment, linesegment cutBarSegment, double upBarWidth, double cutBarWidth)
        {
            result = new UpCuttingResult();

            upBar = CuttingUtil.CreateBar(upBarSegment.StartPoint, upBarSegment.EndPoint, upBarWidth, upBarSegment.length);
            cutBar = CuttingUtil.CreateBar(cutBarSegment.StartPoint, cutBarSegment.EndPoint, cutBarWidth, cutBarSegment.length);
            needExtend = false;
            entireColPoints = new List<gPoint>();

            // 확장 여부 검사
            int colPointCount = 0;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.LT, upBar))
                colPointCount += 1;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.LB, upBar))
                colPointCount += 1;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.RT, upBar))
                colPointCount += 1;
            if (CurtainWallMath.GetPoint2BarCollision(cutBar.RB, upBar))
                colPointCount += 1;

            if (colPointCount == 1)
                needExtend = true;

            // 검사 용 각 꼭짓점 확장 지점 생성
            rt2ltExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RT, cutBar.LT, 1000000);
            rb2lbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RB, cutBar.LB, 1000000);
            lt2rtExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LT, cutBar.RT, 1000000);
            lb2rbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LB, cutBar.RB, 1000000);
            lt2lbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LT, cutBar.LB, 1000000);
            rt2rbExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RT, cutBar.RB, 1000000);
            lb2ltExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LB, cutBar.LT, 1000000);
            rb2rtExtend = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RB, cutBar.RT, 1000000);

            int checkResult = CheckCalculatable();
            if (checkResult != 0)
            {
                result.ResultCode = checkResult;
                return result;
            }
            entireColPoints.Clear();
            CuttingProcess_Left2Right();
            CuttingProcess_Right2Left();
            colPointsAndDisList = ManufactureCollisionPoint(entireColPoints);

            if (needExtend)
            {
                double origin2SecondNearPointLen = GetOrigin2SecondNearPointLen();
                if (origin2SecondNearPointLen != -1)
                {
                    result.ExtendLength = origin2SecondNearPointLen - cutBar.Length;
                }
            }

            if (result.ExtendLength < 0)
            {
                result.ResultCode = -3;
                return result;
            }
            result.CutAngle = GetCuttingAngle();
            if (needExtend)
                result.HasExtend = true;
            return result;
        }

        #region Calculate Left To Right
        // 좌측 -> 우측 검사 Process 함수
        private void CuttingProcess_Left2Right()
        {
            List<gPoint> currentCutPoints = new List<gPoint>();
            // 좌 -> 우 충돌 검사
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.LT, cutBar.RT, lt2rtExtend, currentCutPoints);
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.LB, cutBar.RB, lb2rbExtend, currentCutPoints);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.LT, cutBar.RT, lt2rtExtend, currentCutPoints);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.LB, cutBar.RB, lb2rbExtend, currentCutPoints);
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.LT, cutBar.RT, lt2rtExtend, currentCutPoints);
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.LB, cutBar.RB, lb2rbExtend, currentCutPoints);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.LT, cutBar.RT, lt2rtExtend, currentCutPoints);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.LB, cutBar.RB, lb2rbExtend, currentCutPoints);

            entireColPoints.AddRange(currentCutPoints);
        }
        #endregion

        #region Calculate Right To Left
        private void CuttingProcess_Right2Left()
        {
            List<gPoint> currentCutPoints = new List<gPoint>();
            // 우 -> 좌
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.RT, cutBar.LT, rt2ltExtend, currentCutPoints);
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.RB, cutBar.LB, rb2lbExtend, currentCutPoints);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.RT, cutBar.LT, rt2ltExtend, currentCutPoints);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.RB, cutBar.LB, rb2lbExtend, currentCutPoints);
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.RT, cutBar.LT, rt2ltExtend, currentCutPoints);
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.RB, cutBar.LB, rb2lbExtend, currentCutPoints);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.RT, cutBar.LT, rt2ltExtend, currentCutPoints);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.RB, cutBar.LB, rb2lbExtend, currentCutPoints);

            entireColPoints.AddRange(currentCutPoints);
        }
        #endregion

        #region Common Function
        
        private List<PointAndDis> ManufactureCollisionPoint(List<gPoint> unManufacturedPoints)
        {
            List<PointAndDis> colPoints = new List<PointAndDis>();
            for (int i = 0; i < unManufacturedPoints.Count; ++i)
            {
                PointAndDis pointAndDis = new PointAndDis();
                pointAndDis.Point = unManufacturedPoints[i];
                pointAndDis.Distance = CurtainWallMath.GetLengthBy2Point(cutBar.Center, unManufacturedPoints[i]);
                colPoints.Add(pointAndDis);
            }
            if (needExtend)
                colPoints = DistinctSelf_Double(colPoints);
            else
                colPoints = DistinctSelf_gPoint(colPoints);
            colPoints = colPoints.OrderBy(dis => dis.Distance).ToList();

            if (colPoints.Count <= 1)
                return null;
            result.FirstCutPoint = colPoints[0].Point;
            result.SecondCutPoint = colPoints[1].Point;
            return colPoints;
        }
        private double GetCuttingAngle()
        {
            gPoint farPoint = new gPoint(result.SecondCutPoint);
            gPoint originPoint = null;
            double rot = cutBar.Rotation * -1;
            farPoint = CurtainWallMath.GetRotatedPoint(rot, farPoint, cutBar.Center);
            if (farPoint.y <= cutBar.Center.y)
            {
                if (farPoint.x <= cutBar.Center.x)
                    originPoint = cutBar.RB;
                else
                    originPoint = cutBar.LB;
            }
            else
            {
                if (farPoint.x <= cutBar.Center.x)
                    originPoint = cutBar.RT;
                else
                    originPoint = cutBar.LT;
            }
            rot *= -1;
            farPoint = result.SecondCutPoint;

            Vector cutVec = CurtainWallMath.GetUnitVecBy2Point(result.FirstCutPoint, farPoint);
            Vector horVec = CurtainWallMath.GetUnitVecBy2Point(originPoint, farPoint);
            double cutAngle = cutVec.Dot(horVec);
            cutAngle = Math.Acos(cutAngle);
            cutAngle = Globals.RadiansToDegrees(cutAngle);
            return cutAngle;
        }
        private double GetOrigin2SecondNearPointLen()
        {
            if (colPointsAndDisList.Count <= 1)
                return -1;
            double rot = cutBar.Rotation * -1;
            gPoint secondNearestPoint = CurtainWallMath.GetRotatedPoint(rot, colPointsAndDisList[1].Point, cutBar.Center);

            double origin2SecondNearestPointLen = 0;
            if (secondNearestPoint.y > cutBar.Center.y)
            {
                if (secondNearestPoint.x <= cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Left;
                    result.ExtendDir = "LEFT";
                    secondNearestPoint = CurtainWallMath.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(secondNearestPoint, cutBar.RT);
                }
                else if (secondNearestPoint.x > cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Right;
                    result.ExtendDir = "RIGHT";
                    secondNearestPoint = CurtainWallMath.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(secondNearestPoint, cutBar.LT);
                }
            }
            else if (secondNearestPoint.y <= cutBar.Center.y)
            {
                if (secondNearestPoint.x <= cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Left;
                    result.ExtendDir = "LEFT";
                    secondNearestPoint = CurtainWallMath.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(secondNearestPoint, cutBar.RB);
                }
                else if (secondNearestPoint.x > cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Right;
                    result.ExtendDir = "RIGHT";
                    secondNearestPoint = CurtainWallMath.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = CurtainWallMath.GetLengthBy2Point(secondNearestPoint, cutBar.LB);
                }
            }
            return origin2SecondNearestPointLen;
        }
        private int CheckCalculatable()
        {
            // CutBar의 넓은 면의 선분 2개를 확장하였을 때 UpBar와 충돌해야 True
            int colCount = 0;
            if (CurtainWallMath.GetBarLineCollision(upBar, lt2rtExtend, rt2ltExtend)) colCount += 1;
            if (CurtainWallMath.GetBarLineCollision(upBar, lb2rbExtend, rb2lbExtend)) colCount += 1;
            if (colCount < 2)
                return -2;

            // UpBar의 꼭짓점이 CutBar와 충돌하지 않아야 함
            if (CurtainWallMath.GetPoint2BarCollision(upBar.LT, cutBar)) return -1;
            if (CurtainWallMath.GetPoint2BarCollision(upBar.RT, cutBar)) return -1;
            if (CurtainWallMath.GetPoint2BarCollision(upBar.LB, cutBar)) return -1;
            if (CurtainWallMath.GetPoint2BarCollision(upBar.RB, cutBar)) return -1;

            return 0;
        }

        
        private List<PointAndDis> DistinctSelf_Double(List<PointAndDis> originList)
        {
            List<PointAndDis> resultList = new List<PointAndDis>();
            for (int i = 0; i < originList.Count; ++i)
            {
                bool duplicate = false;
                for (int j = 0; j < i; ++j)
                {
                    if (CurtainWallMath.CompareDouble(originList[i].Distance, originList[j].Distance))
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
        private List<PointAndDis> DistinctSelf_gPoint(List<PointAndDis> originList)
        {
            List<PointAndDis> resultList = new List<PointAndDis>();
            for (int i = 0; i < originList.Count; ++i)
            {
                bool duplicate = false;
                for (int j = 0; j < i; ++j)
                {
                    if (CurtainWallMath.CompareDouble(originList[i].Point.x, originList[j].Point.x) &&
                        CurtainWallMath.CompareDouble(originList[i].Point.y, originList[j].Point.y))
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
        private bool GetCollisionPoints(gPoint upBeam_SPoint, gPoint upBeam_EPoint, gPoint cutBeam_SPoint, gPoint cutBeam_EPoint, gPoint cutBeam_EpointExpend,
            List<gPoint> currentCutPoints)
        {
            if (CurtainWallMath.GetLineIsCross(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EPoint))
            {
                currentCutPoints.Add(CurtainWallMath.GetCrossPoint(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EPoint));
                return true;
            }
            else
            {
                if (CurtainWallMath.GetLineIsCross(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EpointExpend))
                {
                    currentCutPoints.Add(CurtainWallMath.GetCrossPoint(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EpointExpend));
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
   
}
