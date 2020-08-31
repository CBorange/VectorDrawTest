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
    
    
    public class UpCutting_Cross : IUpCuttingAlgorithm
    {
        private MathSupporter math;
        private Beam upBeam;
        private Beam cuttedBeam;

        // 계산용
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
        private bool needExtend;
        private CuttingResult result;

        public UpCutting_Cross()
        {
            math = MathSupporter.Instance;
        }

        // Ver / Hor : Lagacy Parameter
        // 항상 CuttedBeam -> UpBeam 방향으로 검사 진행(즉 CuttedBeam이 확장 대상임)
        public CuttingResult CalcAlgorithm_CuttingRect(Beam upBeam, Beam cuttedBeam)
        {
            // Init
            result = new CuttingResult();

            this.cuttedBeam = cuttedBeam;
            this.upBeam = upBeam;
            needExtend = false;
            entireColPoints = new List<gPoint>();
            colPointsAndDisList = new List<PointAndDis>();

            // 확장 여부 검사
            int colPointCount = 0;
            if (math.Point2BeamCollision(cuttedBeam.LeftTop, upBeam))
                colPointCount += 1;
            if (math.Point2BeamCollision(cuttedBeam.LeftBottom, upBeam))
                colPointCount += 1;
            if (math.Point2BeamCollision(cuttedBeam.RightTop, upBeam))
                colPointCount += 1;
            if (math.Point2BeamCollision(cuttedBeam.RightBottom, upBeam))
                colPointCount += 1;

            if (colPointCount == 1)
                needExtend = true;

            // 검사 용 각 꼭짓점 확장 지점 생성
            rt2ltExtend = math.GetExtendedPointBy2Points(cuttedBeam.RightTop, cuttedBeam.LeftTop, 1000000);
            rb2lbExtend = math.GetExtendedPointBy2Points(cuttedBeam.RightBottom, cuttedBeam.LeftBottom, 1000000);
            lt2rtExtend = math.GetExtendedPointBy2Points(cuttedBeam.LeftTop, cuttedBeam.RightTop, 1000000);
            lb2rbExtend = math.GetExtendedPointBy2Points(cuttedBeam.LeftBottom, cuttedBeam.RightBottom, 1000000);
            lt2lbExtend = math.GetExtendedPointBy2Points(cuttedBeam.LeftTop, cuttedBeam.LeftBottom, 1000000);
            rt2rbExtend = math.GetExtendedPointBy2Points(cuttedBeam.RightTop, cuttedBeam.RightBottom, 1000000);
            lb2ltExtend = math.GetExtendedPointBy2Points(cuttedBeam.LeftBottom, cuttedBeam.LeftTop, 1000000);
            rb2rtExtend = math.GetExtendedPointBy2Points(cuttedBeam.RightBottom, cuttedBeam.RightTop, 1000000);

            if (!CheckCalculatable())
            {
                result.ExceptionShape = true;
                MessageBox.Show("계산할 수 없는 형태의 이형 입니다.", "계산 불가", MessageBoxButtons.OK);
                return result;
            }
            CuttingProcess_Left2Right();
            CuttingProcess_Right2Left();

            if (result.ExtendLength < 0)
            {
                result.ExceptionShape = true;
                MessageBox.Show("계산할 수 없는 형태의 이형 입니다.", "계산 불가", MessageBoxButtons.OK);
                return result;
            }
            result.CutAngle = GetCuttingAngle();
            return result;
        }

        // 좌측 -> 우측 검사 Process 함수
        private void CuttingProcess_Left2Right()
        {
            CalcCutting_Left2Right();

            if (needExtend)
            {
                double origin2SecondNearPointLen = GetOrigin2SecondNearPointLen();
                if (origin2SecondNearPointLen == -1)    // 확장 검사가 가능한 방향 Process가 아님
                    return;
                result.ExtendLength = origin2SecondNearPointLen - cuttedBeam.BeamWidth;
            }
            ApplyFigureGraphic(CreateExtendPoints_Left2Right());
        }
        private void CalcCutting_Left2Right()
        {
            // 좌 -> 우 충돌 검사
            entireColPoints.Clear();
            GetCollisionPoints(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.LeftTop, cuttedBeam.RightTop, lt2rtExtend);
            GetCollisionPoints(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.LeftBottom, cuttedBeam.RightBottom, lb2rbExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.LeftTop, cuttedBeam.RightTop, lt2rtExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.LeftBottom, cuttedBeam.RightBottom, lb2rbExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.LeftTop, cuttedBeam.LeftTop, cuttedBeam.RightTop, lt2rtExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.LeftTop, cuttedBeam.LeftBottom, cuttedBeam.RightBottom, lb2rbExtend);
            GetCollisionPoints(upBeam.RightBottom, upBeam.RightTop, cuttedBeam.LeftTop, cuttedBeam.RightTop, lt2rtExtend);
            GetCollisionPoints(upBeam.RightBottom, upBeam.RightTop, cuttedBeam.LeftBottom, cuttedBeam.RightBottom, lb2rbExtend);

            CalcCollisionPoint();
        }
        private gPoint[] CreateExtendPoints_Left2Right()
        {
            Vector lt2rtUnit = math.GetUnitVecBy2Point(cuttedBeam.RightTop, cuttedBeam.LeftTop);
            Vector lb2rbUnit = math.GetUnitVecBy2Point(cuttedBeam.RightBottom, cuttedBeam.LeftBottom);
            gPoint extendRT = math.GetExtendPoint(cuttedBeam.RightTop, lt2rtUnit * result.ExtendLength);
            gPoint extendRB = math.GetExtendPoint(cuttedBeam.RightBottom, lb2rbUnit * result.ExtendLength);

            gPoint[] extendBox = new gPoint[4];
            extendBox[0] = new gPoint(cuttedBeam.RightTop);
            extendBox[1] = new gPoint(extendRT);
            extendBox[2] = new gPoint(extendRB);
            extendBox[3] = new gPoint(cuttedBeam.RightBottom);
            return extendBox;
        }

        // 우측 -> 좌측 검사 Process 함수
        private void CuttingProcess_Right2Left()
        {
            CalcCutting_Right2Left();

            if (needExtend)
            {
                double origin2SecondNearPointLen = GetOrigin2SecondNearPointLen();
                if (origin2SecondNearPointLen == -1)    // 확장 검사가 가능한 방향 Process가 아님
                    return;
                result.ExtendLength = origin2SecondNearPointLen - cuttedBeam.BeamWidth;
            }
            ApplyFigureGraphic(CreateExtendPoints_Right2Left());
        }
        private void CalcCutting_Right2Left()
        {
            // 우 -> 좌
            entireColPoints.Clear();
            GetCollisionPoints(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.RightTop, cuttedBeam.LeftTop, rt2ltExtend);
            GetCollisionPoints(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.RightBottom, cuttedBeam.LeftBottom, rb2lbExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.RightTop, cuttedBeam.LeftTop, rt2ltExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.RightBottom, cuttedBeam.LeftBottom, rb2lbExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.LeftTop, cuttedBeam.RightTop, cuttedBeam.LeftTop, rt2ltExtend);
            GetCollisionPoints(upBeam.LeftBottom, upBeam.LeftTop, cuttedBeam.RightBottom, cuttedBeam.LeftBottom, rb2lbExtend);
            GetCollisionPoints(upBeam.RightBottom, upBeam.RightTop, cuttedBeam.RightTop, cuttedBeam.LeftTop, rt2ltExtend);
            GetCollisionPoints(upBeam.RightBottom, upBeam.RightTop, cuttedBeam.RightBottom, cuttedBeam.LeftBottom, rb2lbExtend);

            CalcCollisionPoint();
        }
        private gPoint[] CreateExtendPoints_Right2Left()
        {
            Vector rt2ltUnit = math.GetUnitVecBy2Point(cuttedBeam.LeftTop, cuttedBeam.RightTop);
            Vector rb2lbUnit = math.GetUnitVecBy2Point(cuttedBeam.LeftBottom, cuttedBeam.RightBottom);
            gPoint extendLT = math.GetExtendPoint(cuttedBeam.LeftTop, rt2ltUnit * result.ExtendLength);
            gPoint extendLB = math.GetExtendPoint(cuttedBeam.LeftBottom, rb2lbUnit * result.ExtendLength);

            gPoint[] extendBox = new gPoint[4];
            extendBox[0] = new gPoint(cuttedBeam.LeftTop);
            extendBox[1] = new gPoint(extendLT);
            extendBox[2] = new gPoint(extendLB);
            extendBox[3] = new gPoint(cuttedBeam.LeftBottom);
            return extendBox;
        }

        // 방향 상관없이 공통 함수
        private void ApplyFigureGraphic(gPoint[] extendPoints)
        {
            // 산출한 확장 길이 비주얼 적용
            if (needExtend)
                cuttedBeam.AddExtendFigure(extendPoints, Color.Green, 0.4f);

            // 충돌 좌표 중 가까운 2 충돌 점만 표시
            List<gPoint> finalCuttingFigures = new List<gPoint>(2);
            finalCuttingFigures.Add(colPointsAndDisList[0].Point);
            finalCuttingFigures.Add(colPointsAndDisList[1].Point);
            cuttedBeam.AddCuttingFigure(finalCuttingFigures, Color.Yellow,0.75f);
        }
        private void CalcCollisionPoint()
        {
            colPointsAndDisList.Clear();
            for (int i = 0; i < entireColPoints.Count; ++i)
            {
                PointAndDis pointAndDis = new PointAndDis();
                pointAndDis.Point = entireColPoints[i];
                pointAndDis.Distance = math.GetLengthBy2Point(cuttedBeam.Center, entireColPoints[i]);
                colPointsAndDisList.Add(pointAndDis);
            }
            if (needExtend)
                colPointsAndDisList = DistinctSelf_Double(colPointsAndDisList);
            else
                colPointsAndDisList = DistinctSelf_gPoint(colPointsAndDisList);
            colPointsAndDisList = colPointsAndDisList.OrderBy(dis => dis.Distance).ToList();

            if (colPointsAndDisList.Count <= 1)
                return;
            result.FirstCutPoint = colPointsAndDisList[0].Point;
            result.SecondCutPoint = colPointsAndDisList[1].Point;
        }
        private double GetCuttingAngle()
        {
            gPoint farPoint = new gPoint(result.SecondCutPoint);
            gPoint originPoint = null;
            double rot = cuttedBeam.Rotation * -1;
            farPoint = math.GetRotatedPoint(rot, farPoint, cuttedBeam.Center);
            if (farPoint.y <= cuttedBeam.Center.y)
            {
                if (farPoint.x <= cuttedBeam.Center.x)
                    originPoint = cuttedBeam.RightBottom;
                else
                    originPoint = cuttedBeam.LeftBottom;
            }
            else
            {
                if (farPoint.x <= cuttedBeam.Center.x)
                    originPoint = cuttedBeam.RightTop;
                else
                    originPoint = cuttedBeam.RightBottom;
            }
            rot *= -1;
            farPoint = result.SecondCutPoint;

            Vector cutVec = math.GetUnitVecBy2Point(result.FirstCutPoint, farPoint);
            Vector horVec = math.GetUnitVecBy2Point(originPoint, farPoint);
            double cutAngle = cutVec.Dot(horVec);
            cutAngle = Math.Acos(cutAngle);
            cutAngle = Globals.RadiansToDegrees(cutAngle);
            return cutAngle;
        }
        private double GetOrigin2SecondNearPointLen()
        {
            if (colPointsAndDisList.Count <= 1)
                return -1;
            double rot = cuttedBeam.Rotation * -1;
            gPoint secondNearestPoint = math.GetRotatedPoint(rot, colPointsAndDisList[1].Point, cuttedBeam.Center);

            double origin2SecondNearestPointLen = 0;
            if (secondNearestPoint.y > cuttedBeam.Center.y)
            {
                if (secondNearestPoint.x <= cuttedBeam.Center.x)
                {
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cuttedBeam.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cuttedBeam.RightTop);
                }
                else if (secondNearestPoint.x > cuttedBeam.Center.x)
                {
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cuttedBeam.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cuttedBeam.LeftTop);
                }
            }
            else if (secondNearestPoint.y <= cuttedBeam.Center.y)
            {
                if (secondNearestPoint.x <= cuttedBeam.Center.x)
                {
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cuttedBeam.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cuttedBeam.RightBottom);
                }
                else if (secondNearestPoint.x > cuttedBeam.Center.x)
                {
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cuttedBeam.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cuttedBeam.LeftBottom);
                }
            }
            return origin2SecondNearestPointLen;
        }
        private bool CheckCalculatable()
        {
            int colCount = 0;
            if (math.GetBeamLineCollision(upBeam, lt2rtExtend, rt2ltExtend)) colCount += 1;
            if (math.GetBeamLineCollision(upBeam, lb2rbExtend, rb2lbExtend)) colCount += 1;
            if (colCount >= 2)
                return true;
            return false;
        }

        private bool CompareDouble(double a, double b)
        {
            double difference = Math.Abs(a - b);
            if (difference <= 0.000000001 || difference == 0)
                return true;
            return false;
        }
        private List<PointAndDis> DistinctSelf_Double(List<PointAndDis> originList)
        {
            List<PointAndDis> resultList = new List<PointAndDis>();
            for (int i = 0; i < originList.Count; ++i)
            {
                bool duplicate = false;
                for (int j = 0; j < i; ++j)
                {
                    if (CompareDouble(originList[i].Distance, originList[j].Distance))
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
                    if (CompareDouble(originList[i].Point.x, originList[j].Point.x) &&
                        CompareDouble(originList[i].Point.y, originList[j].Point.y))
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
        private bool GetCollisionPoints(gPoint upBeam_SPoint, gPoint upBeam_EPoint, gPoint cutBeam_SPoint, gPoint cutBeam_EPoint, gPoint cutBeam_EpointExpend)
        {
            if (math.GetLineIsCross(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EPoint))
            {
                entireColPoints.Add(math.GetCrossPoint(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EPoint));
                return true;
            }
            else
            {
                if (math.GetLineIsCross(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EpointExpend))
                {
                    entireColPoints.Add(math.GetCrossPoint(upBeam_SPoint, upBeam_EPoint, cutBeam_SPoint, cutBeam_EpointExpend));
                    return true;
                }
                return false;
            }
        }
        
        
    }
}
