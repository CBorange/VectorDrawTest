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
using MathPractice.Model.Manager;
using MathPractice.Model.CustomFigure;
using System.Diagnostics;

namespace MathPractice.Model.CuttingAlgorithm
{
    public class Bar
    {
        public gPoint Left;
        public gPoint Right;
        public gPoint Center;
        public gPoint LT;
        public gPoint LB;
        public gPoint RT;
        public gPoint RB;
        public gPoint BOT;
        public gPoint TOP;
        public double Rotation;
        public double Width;
        public double Length;
    }
    public class CurtainWallCutting
    {
        private CurtainWallMath math;
        private CuttingResult result;
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

        public CurtainWallCutting()
        {
            math = new CurtainWallMath();
        }
        public CuttingResult GetCuttingResult(gPoint upBar_LeftP, gPoint upBar_RightP, gPoint cutBar_LeftP, gPoint cutBar_RightP, double barWidth, double barLength)
        {
            result = new CuttingResult();

            upBar = CreateBar(upBar_LeftP, upBar_RightP, barWidth, barLength);
            cutBar = CreateBar(cutBar_LeftP, cutBar_RightP, barWidth, barLength);
            needExtend = false;
            entireColPoints = new List<gPoint>();
            colPointsAndDisList = new List<PointAndDis>();

            // 확장 여부 검사
            int colPointCount = 0;
            if (math.Point2BarCollision(cutBar.LT, upBar))
                colPointCount += 1;
            if (math.Point2BarCollision(cutBar.LB, upBar))
                colPointCount += 1;
            if (math.Point2BarCollision(cutBar.RT, upBar))
                colPointCount += 1;
            if (math.Point2BarCollision(cutBar.RB, upBar))
                colPointCount += 1;

            if (colPointCount == 1)
                needExtend = true;

            // 검사 용 각 꼭짓점 확장 지점 생성
            rt2ltExtend = math.GetExtendedPointBy2Points(cutBar.RT, cutBar.LT, 1000000);
            rb2lbExtend = math.GetExtendedPointBy2Points(cutBar.RB, cutBar.LB, 1000000);
            lt2rtExtend = math.GetExtendedPointBy2Points(cutBar.LT, cutBar.RT, 1000000);
            lb2rbExtend = math.GetExtendedPointBy2Points(cutBar.LB, cutBar.RB, 1000000);
            lt2lbExtend = math.GetExtendedPointBy2Points(cutBar.LT, cutBar.LB, 1000000);
            rt2rbExtend = math.GetExtendedPointBy2Points(cutBar.RT, cutBar.RB, 1000000);
            lb2ltExtend = math.GetExtendedPointBy2Points(cutBar.LB, cutBar.LT, 1000000);
            rb2rtExtend = math.GetExtendedPointBy2Points(cutBar.RB, cutBar.RT, 1000000);

            if (!CheckCalculatable())
            {
                result.ExceptionShape = true;
                return result;
            }
            CuttingProcess_Left2Right();
            CuttingProcess_Right2Left();

            if (result.ExtendLength < 0)
            {
                result.ExceptionShape = true;
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
            CalcCutting_Left2Right();

            if (needExtend)
            {
                double origin2SecondNearPointLen = GetOrigin2SecondNearPointLen();
                if (origin2SecondNearPointLen == -1)    // 확장 검사가 가능한 방향 Process가 아님
                    return;
                result.ExtendLength = origin2SecondNearPointLen - cutBar.Length;
            }
        }
        private void CalcCutting_Left2Right()
        {
            // 좌 -> 우 충돌 검사
            entireColPoints.Clear();
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.LT, cutBar.RT, lt2rtExtend);
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.LB, cutBar.RB, lb2rbExtend);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.LT, cutBar.RT, lt2rtExtend);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.LB, cutBar.RB, lb2rbExtend);
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.LT, cutBar.RT, lt2rtExtend);
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.LB, cutBar.RB, lb2rbExtend);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.LT, cutBar.RT, lt2rtExtend);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.LB, cutBar.RB, lb2rbExtend);

            CalcCollisionPoint();
        }
        #endregion

        #region Calculate Right To Left
        private void CuttingProcess_Right2Left()
        {
            CalcCutting_Right2Left();

            if (needExtend)
            {
                double origin2SecondNearPointLen = GetOrigin2SecondNearPointLen();
                if (origin2SecondNearPointLen == -1)    // 확장 검사가 가능한 방향 Process가 아님
                    return;
                result.ExtendLength = origin2SecondNearPointLen - cutBar.Length;
            }
        }
        private void CalcCutting_Right2Left()
        {
            // 우 -> 좌
            entireColPoints.Clear();
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.RT, cutBar.LT, rt2ltExtend);
            GetCollisionPoints(upBar.LT, upBar.RT, cutBar.RB, cutBar.LB, rb2lbExtend);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.RT, cutBar.LT, rt2ltExtend);
            GetCollisionPoints(upBar.LB, upBar.RB, cutBar.RB, cutBar.LB, rb2lbExtend);
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.RT, cutBar.LT, rt2ltExtend);
            GetCollisionPoints(upBar.LB, upBar.LT, cutBar.RB, cutBar.LB, rb2lbExtend);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.RT, cutBar.LT, rt2ltExtend);
            GetCollisionPoints(upBar.RB, upBar.RT, cutBar.RB, cutBar.LB, rb2lbExtend);

            CalcCollisionPoint();
        }
        #endregion

        #region Common Function
        private Bar CreateBar(gPoint barLeftP, gPoint barRightP, double barWidth, double barLength)
        {
            Bar bar = new Bar();
            // 인자로 들어온 데이터 저장
            bar.Left = barLeftP;
            bar.Right = barRightP;
            bar.Width = barWidth;
            bar.Length = barLength;

            // 필요한 데이터 계산
            bar.Center = math.GetCenterBy2Points(bar.Left, bar.Right);
            double halfWidth = bar.Width * 0.5;
            double halfLength = bar.Length * 0.5;

            // Rotation 계산
            Vector l2rVec = math.GetVectorBy2Point(barRightP, barLeftP);
            Vector centerLineUnit = new Vector(1, 0, 0);
            if (l2rVec.x < 0)
                centerLineUnit = new Vector(-1, 0, 0);

            double x = l2rVec.Dot(centerLineUnit);
            bar.Rotation = Math.Atan2(l2rVec.y, x);
            bar.Rotation = Globals.RadiansToDegrees(bar.Rotation);
            if (l2rVec.x < 0 && l2rVec.y >= 0)
                bar.Rotation *= -1;
            else if (l2rVec.x < 0 && l2rVec.y <= 0)
                bar.Rotation *= -1;

            // 각 Bar Point 계산
            bar.Left = new gPoint(bar.Center.x - halfLength, bar.Center.y);
            bar.Right = new gPoint(bar.Center.x + halfLength, bar.Center.y);
            bar.LT = new gPoint(bar.Center.x - halfLength, bar.Center.y + halfWidth);
            bar.RT = new gPoint(bar.Center.x + halfLength, bar.Center.y + halfWidth);
            bar.LB = new gPoint(bar.Center.x - halfLength, bar.Center.y - halfWidth);
            bar.RB = new gPoint(bar.Center.x + halfLength, bar.Center.y - halfWidth);
            bar.BOT = new gPoint(bar.Center.x, bar.Center.y - halfWidth);
            bar.TOP = new gPoint(bar.Center.x, bar.Center.y + halfWidth);

            bar.Left = math.GetRotatedPoint(bar.Rotation, bar.Left, bar.Center);
            bar.Right = math.GetRotatedPoint(bar.Rotation, bar.Right, bar.Center);
            bar.LT = math.GetRotatedPoint(bar.Rotation, bar.LT, bar.Center);
            bar.RT = math.GetRotatedPoint(bar.Rotation, bar.RT, bar.Center);
            bar.LB = math.GetRotatedPoint(bar.Rotation, bar.LB, bar.Center);
            bar.RB = math.GetRotatedPoint(bar.Rotation, bar.RB, bar.Center);
            bar.BOT = math.GetRotatedPoint(bar.Rotation, bar.BOT, bar.Center);
            bar.TOP = math.GetRotatedPoint(bar.Rotation, bar.TOP, bar.Center);

            return bar;
        }
        private void CalcCollisionPoint()
        {
            colPointsAndDisList.Clear();
            for (int i = 0; i < entireColPoints.Count; ++i)
            {
                PointAndDis pointAndDis = new PointAndDis();
                pointAndDis.Point = entireColPoints[i];
                pointAndDis.Distance = math.GetLengthBy2Point(cutBar.Center, entireColPoints[i]);
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
            double rot = cutBar.Rotation * -1;
            farPoint = math.GetRotatedPoint(rot, farPoint, cutBar.Center);
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
                    originPoint = cutBar.RB;
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
            double rot = cutBar.Rotation * -1;
            gPoint secondNearestPoint = math.GetRotatedPoint(rot, colPointsAndDisList[1].Point, cutBar.Center);

            double origin2SecondNearestPointLen = 0;
            if (secondNearestPoint.y > cutBar.Center.y)
            {
                if (secondNearestPoint.x <= cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Left;
                    result.ExtendDir = "LEFT";
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cutBar.RT);
                }
                else if (secondNearestPoint.x > cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Right;
                    result.ExtendDir = "RIGHT";
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cutBar.LT);
                }
            }
            else if (secondNearestPoint.y <= cutBar.Center.y)
            {
                if (secondNearestPoint.x <= cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Left;
                    result.ExtendDir = "LEFT";
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cutBar.RB);
                }
                else if (secondNearestPoint.x > cutBar.Center.x)
                {
                    result.ExtendPoint = cutBar.Right;
                    result.ExtendDir = "RIGHT";
                    secondNearestPoint = math.GetRotatedPoint(rot * -1, secondNearestPoint, cutBar.Center);
                    origin2SecondNearestPointLen = math.GetLengthBy2Point(secondNearestPoint, cutBar.LB);
                }
            }
            return origin2SecondNearestPointLen;
        }
        private bool CheckCalculatable()
        {
            int colCount = 0;
            if (math.GetBarLineCollision(upBar, lt2rtExtend, rt2ltExtend)) colCount += 1;
            if (math.GetBarLineCollision(upBar, lb2rbExtend, rb2lbExtend)) colCount += 1;
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
        #endregion
    }
    public class CurtainWallMath
    {
        public double[,] GetRotationMat(double rot)
        {
            double[,] rotMatrix = new double[2, 2];
            rotMatrix[0, 0] = Math.Cos(Globals.DegreesToRadians(rot));
            rotMatrix[0, 1] = -Math.Sin(Globals.DegreesToRadians(rot));
            rotMatrix[1, 0] = Math.Sin(Globals.DegreesToRadians(rot));
            rotMatrix[1, 1] = Math.Cos(Globals.DegreesToRadians(rot));
            return rotMatrix;
        }
        public gPoint GetRotatedPoint(double rot, gPoint point, gPoint center)
        {
            double[,] mat = GetRotationMat(rot);
            gPoint result = new gPoint();
            result.x = ((mat[0, 0] * (point.x - center.x)) + (mat[0, 1] * (point.y - center.y)) + center.x);
            result.y = ((mat[1, 0] * (point.x - center.x)) + (mat[1, 1] * (point.y - center.y)) + center.y);
            return result;
        }
        public bool Point2BarCollision(gPoint point, Bar bar)
        {
            gPoint left = new gPoint(bar.Left);
            gPoint right = new gPoint(bar.Right);
            gPoint bottom = new gPoint(bar.BOT);
            gPoint top = new gPoint(bar.TOP);

            double rot = bar.Rotation * -1;
            left = GetRotatedPoint(rot, left, bar.Center);
            right = GetRotatedPoint(rot, right, bar.Center);
            bottom = GetRotatedPoint(rot, bottom, bar.Center);
            top = GetRotatedPoint(rot, top, bar.Center);
            point = GetRotatedPoint(rot, point, bar.Center);

            if (point.x >= left.x &&
                point.x <= right.x &&
                point.y <= top.y &&
                point.y >= bottom.y)
                return true;
            return false;
        }
        public gPoint GetCrossPoint(gPoint pointA, gPoint pointB, gPoint pointC, gPoint pointD)
        {
            Vector lineA2C = GetVectorBy2Point(pointC, pointA);
            Vector lineA2B = GetVectorBy2Point(pointB, pointA);
            Vector lineA2B_Unit = new Vector(lineA2B);
            lineA2B_Unit.Normalize();

            double a2eLength = lineA2C.Dot(lineA2B_Unit);
            Vector lineA2E = lineA2B_Unit * a2eLength;
            gPoint pointE = new gPoint(pointA.x + lineA2E.x, pointA.y + lineA2E.y);

            Vector lineC2E = GetVectorBy2Point(pointE, pointC);
            Vector lineC2E_Unit = new Vector(lineC2E);
            lineC2E_Unit.Normalize();
            Vector lineC2D = GetVectorBy2Point(pointD, pointC);
            double c2fLength = lineC2D.Dot(lineC2E_Unit);

            Vector lineC2F = lineC2E_Unit * c2fLength;
            gPoint pointF = new gPoint(pointC.x + lineC2F.x, pointC.y + lineC2F.y);

            double c2eRatio = lineC2E.Length / c2fLength;

            double crossPointLength = lineC2D.Length * c2eRatio;
            Vector lineC2D_Unit = new Vector(lineC2D);
            lineC2D_Unit.Normalize();
            Vector lineC2CrossPoint = lineC2D_Unit * crossPointLength;

            gPoint crossPoint = new gPoint(pointC.x + lineC2CrossPoint.x, pointC.y + lineC2CrossPoint.y);
            return crossPoint;
        }
        public double GetLengthBy2Point(gPoint pointA, gPoint pointB)
        {
            Vector vec = GetVectorBy2Point(pointB, pointA);
            return vec.Length;
        }
        public Vector GetVectorBy2Point(gPoint target, gPoint origin)
        {
            Vector newVec = new Vector(target.x - origin.x, target.y - origin.y, 0);
            return newVec;
        }
        public Vector GetUnitVecBy2Point(gPoint target, gPoint origin)
        {
            Vector vec = GetVectorBy2Point(target, origin);
            vec.Normalize();
            return vec;
        }
        public gPoint GetExtendPoint(gPoint origin, Vector expandVec)
        {
            return new gPoint(origin.x + expandVec.x, origin.y + expandVec.y);
        }
        public gPoint GetExtendedPointBy2Points(gPoint startP, gPoint endP, int expandValue)
        {
            Vector vec = GetVectorBy2Point(endP, startP);
            vec.Normalize();
            vec *= expandValue;
            gPoint expanded = GetExtendPoint(startP, vec);
            return expanded;
        }
        public gPoint GetCenterBy2Points(gPoint pointA, gPoint pointB)
        {
            Vector a2B = MathSupporter.Instance.GetVectorBy2Point(pointB, pointA);
            double start2CenterLength = a2B.Length * 0.5f;
            a2B.Normalize();
            a2B *= start2CenterLength;
            return new gPoint(pointA.x + a2B.x, pointA.y + a2B.y);
        }
        public gPoint GetNearestPoint(gPoint center, gPoint[] points)
        {
            gPoint nearestPoint = points[0];
            double minLength = GetLengthBy2Point(center, points[0]);
            for (int i = 1; i < points.Length; ++i)
            {
                double dis = GetLengthBy2Point(center, points[i]);
                if (dis < minLength)
                {
                    minLength = dis;
                    nearestPoint = points[i];
                }
            }
            return new gPoint(nearestPoint);
        }
        public gPoint GetFarthestPoint(gPoint center, gPoint[] points)
        {
            gPoint farthestPoint = points[0];
            double maxLength = GetLengthBy2Point(center, points[0]);
            for (int i = 1; i < points.Length; ++i)
            {
                double dis = GetLengthBy2Point(center, points[i]);
                if (dis > maxLength)
                {
                    maxLength = dis;
                    farthestPoint = points[i];
                }
            }
            return new gPoint(farthestPoint);
        }
        public bool OBBCollision(Beam beamA, Beam beamB)
        {
            Vector disVec = GetVectorBy2Point(beamA.Center, beamB.Center);
            Vector[] seperateVec =
            {
                GetVectorBy2Point(beamA.Top,beamA.Center),
                GetVectorBy2Point(beamA.Right,beamA.Center),
                GetVectorBy2Point(beamB.Top,beamB.Center),
                GetVectorBy2Point(beamB.Right,beamB.Center)
            };
            Vector seperateUnit;
            for (int seperateIDX = 0; seperateIDX < 4; ++seperateIDX)
            {
                double sum = 0;
                seperateUnit = new Vector(seperateVec[seperateIDX]);
                seperateUnit.Normalize();

                for (int vecIDx = 0; vecIDx < 4; ++vecIDx)
                    sum += Math.Abs(seperateVec[vecIDx].Dot(seperateUnit));

                if (Math.Abs(disVec.Dot(seperateUnit)) > sum)
                    return false;
            }
            return true;
        }
        public bool Point2BeamCollision(gPoint point, Beam beam)
        {
            gPoint left = new gPoint(beam.Left);
            gPoint right = new gPoint(beam.Right);
            gPoint bottom = new gPoint(beam.Bottom);
            gPoint top = new gPoint(beam.Top);

            double rot = beam.Rotation * -1;
            left = GetRotatedPoint(rot, left, beam.Center);
            right = GetRotatedPoint(rot, right, beam.Center);
            bottom = GetRotatedPoint(rot, bottom, beam.Center);
            top = GetRotatedPoint(rot, top, beam.Center);
            point = GetRotatedPoint(rot, point, beam.Center);

            if (point.x >= left.x && point.x <= right.x && point.y <= top.y && point.y >= bottom.y)
                return true;
            return false;
        }

        public int CCW(gPoint a, gPoint b, gPoint c)
        {
            double op = (a.x * b.y) + (b.x * c.y) + (c.x * a.y);
            op -= (a.y * b.x) + (b.y * c.x) + (c.y * a.x);
            if (op > 0) return 1;
            else if (op == 0) return 0;
            else return -1;
        }
        public bool GetLineIsCross(gPoint pointA, gPoint pointB, gPoint pointC, gPoint pointD)
        {
            int ab = CCW(pointA, pointB, pointC) * CCW(pointA, pointB, pointD);
            int cd = CCW(pointC, pointD, pointA) * CCW(pointC, pointD, pointB);
            if (ab == 0 && cd == 0)
            {
                Vector AC = GetVectorBy2Point(pointC, pointA);
                Vector AD = GetVectorBy2Point(pointD, pointA);
                Vector BC = GetVectorBy2Point(pointC, pointB);
                Vector BD = GetVectorBy2Point(pointD, pointB);

                double maxLength_A = Math.Max(AC.Length, AD.Length);
                double maxLength_B = Math.Max(BC.Length, BD.Length);
                double crossLineLength = Math.Max(maxLength_A, maxLength_B);

                Vector AB = GetVectorBy2Point(pointB, pointA);
                Vector CD = GetVectorBy2Point(pointD, pointC);

                if (crossLineLength < AB.Length + CD.Length)
                    return true;
            }
            return ab <= 0 && cd <= 0;
        }
        public bool GetBarLineCollision(Bar beam, gPoint lineStart, gPoint lineEnd)
        {
            if (GetLineIsCross(beam.LT, beam.RT, lineStart, lineEnd))
                return true;
            if (GetLineIsCross(beam.LT, beam.RT, lineStart, lineEnd))
                return true;
            if (GetLineIsCross(beam.LB, beam.LT, lineStart, lineEnd))
                return true;
            if (GetLineIsCross(beam.RB, beam.RT, lineStart, lineEnd))
                return true;
            return false;
        }
    }
}
