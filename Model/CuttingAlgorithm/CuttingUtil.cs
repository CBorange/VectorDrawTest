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
    public class PointAndDis
    {
        public PointAndDis()
        {
            Point = null;
            Distance = 0;
        }
        public PointAndDis(gPoint point, double dis)
        {
            Point = point;
            Distance = dis;
        }
        public gPoint Point;
        public double Distance;
    }
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
    public class CurtainWallMath
    {
        public static double[,] GetRotationMat(double rot)
        {
            double[,] rotMatrix = new double[2, 2];
            rotMatrix[0, 0] = Math.Cos(Globals.DegreesToRadians(rot));
            rotMatrix[0, 1] = -Math.Sin(Globals.DegreesToRadians(rot));
            rotMatrix[1, 0] = Math.Sin(Globals.DegreesToRadians(rot));
            rotMatrix[1, 1] = Math.Cos(Globals.DegreesToRadians(rot));
            return rotMatrix;
        }
        public static gPoint GetRotatedPoint(double rot, gPoint point, gPoint center)
        {
            double[,] mat = GetRotationMat(rot);
            gPoint result = new gPoint();
            result.x = ((mat[0, 0] * (point.x - center.x)) + (mat[0, 1] * (point.y - center.y)) + center.x);
            result.y = ((mat[1, 0] * (point.x - center.x)) + (mat[1, 1] * (point.y - center.y)) + center.y);
            return result;
        }
        public static bool GetPoint2BarCollision(gPoint point, Bar bar)
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
        public static gPoint GetCrossPoint(gPoint pointA, gPoint pointB, gPoint pointC, gPoint pointD)
        {
            //Vector lineA2C = GetVectorBy2Point(pointC, pointA);
            //Vector lineA2B = GetVectorBy2Point(pointB, pointA);
            //Vector lineA2B_Unit = new Vector(lineA2B);
            //lineA2B_Unit.Normalize();

            //double a2eLength = lineA2C.Dot(lineA2B_Unit);
            //Vector lineA2E = lineA2B_Unit * a2eLength;
            //gPoint pointE = new gPoint(pointA.x + lineA2E.x, pointA.y + lineA2E.y);

            //Vector lineC2E = GetVectorBy2Point(pointE, pointC);
            //Vector lineC2E_Unit = new Vector(lineC2E);
            //lineC2E_Unit.Normalize();
            //Vector lineC2D = GetVectorBy2Point(pointD, pointC);
            //double c2fLength = lineC2D.Dot(lineC2E_Unit);

            //Vector lineC2F = lineC2E_Unit * c2fLength;
            //gPoint pointF = new gPoint(pointC.x + lineC2F.x, pointC.y + lineC2F.y);

            //double c2eRatio = lineC2E.Length / c2fLength;

            //double crossPointLength = lineC2D.Length * c2eRatio;
            //Vector lineC2D_Unit = new Vector(lineC2D);
            //lineC2D_Unit.Normalize();
            //Vector lineC2CrossPoint = lineC2D_Unit * crossPointLength;

            //gPoint crossPoint = new gPoint(pointC.x + lineC2CrossPoint.x, pointC.y + lineC2CrossPoint.y);
            //return crossPoint;

            double px = (pointA.x * pointB.y - pointA.y * pointB.x) * (pointC.x - pointD.x) - (pointA.x - pointB.x) * (pointC.x * pointD.y - pointC.y * pointD.x);
            double py = (pointA.x * pointB.y - pointA.y * pointB.x) * (pointC.y - pointD.y) - (pointA.y - pointB.y) * (pointC.x * pointD.y - pointC.y * pointD.x);
            double p = (pointA.x - pointB.x) * (pointC.y - pointD.y) - (pointA.y - pointB.y) * (pointC.x - pointD.x);

            if (p == 0)
                return null;

            double x = px / p;
            double y = py / p;
            return new gPoint(x, y);
        }
        public static double GetLengthBy2Point(gPoint pointA, gPoint pointB)
        {
            Vector vec = GetVectorBy2Point(pointB, pointA);
            return vec.Length;
        }
        public static Vector GetVectorBy2Point(gPoint target, gPoint origin)
        {
            Vector newVec = new Vector(target.x - origin.x, target.y - origin.y, 0);
            return newVec;
        }
        public static Vector GetUnitVecBy2Point(gPoint target, gPoint origin)
        {
            Vector vec = GetVectorBy2Point(target, origin);
            vec.Normalize();
            return vec;
        }
        public static gPoint GetExtendPoint(gPoint origin, Vector expandVec)
        {
            return new gPoint(origin.x + expandVec.x, origin.y + expandVec.y);
        }
        public static gPoint GetExtendedPointBy2Points(gPoint startP, gPoint endP, int expandValue)
        {
            Vector vec = GetVectorBy2Point(endP, startP);
            vec.Normalize();
            vec *= expandValue;
            gPoint expanded = GetExtendPoint(startP, vec);
            return expanded;
        }
        public static gPoint GetCenterBy2Points(gPoint pointA, gPoint pointB)
        {
            Vector a2B = GetVectorBy2Point(pointB, pointA);
            double start2CenterLength = a2B.Length * 0.5f;
            a2B.Normalize();
            a2B *= start2CenterLength;
            return new gPoint(pointA.x + a2B.x, pointA.y + a2B.y);
        }
        public static int CCW(gPoint a, gPoint b, gPoint c)
        {
            double op = (a.x * b.y) + (b.x * c.y) + (c.x * a.y);
            op -= (a.y * b.x) + (b.y * c.x) + (c.y * a.x);
            if (op > 0) return 1;
            else if (op == 0) return 0;
            else return -1;
        }
        public static bool GetLineIsCross(gPoint pointA, gPoint pointB, gPoint pointC, gPoint pointD)
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
        public static bool GetBarLineCollision(Bar bar, gPoint lineStart, gPoint lineEnd)
        {
            if (GetLineIsCross(bar.LT, bar.RT, lineStart, lineEnd))
                return true;
            if (GetLineIsCross(bar.LB, bar.RB, lineStart, lineEnd))
                return true;
            return false;
        }
        public static bool CompareDouble(double a, double b)
        {
            double difference = Math.Abs(a - b);
            if (difference <= 0.00000001 || difference == 0)
                return true;
            return false;
        }
        public static bool GetPointLineCollision(gPoint point, gPoint lineStart, gPoint lineEnd)
        {
            double point2LineStart = GetLengthBy2Point(point, lineStart);
            double point2LineEnd = GetLengthBy2Point(point, lineEnd);
            double lineLength = GetLengthBy2Point(lineStart, lineEnd);

            double point2LineLength = point2LineStart + point2LineEnd;
            if (CompareDouble(point2LineLength, lineLength))
                return true;
            return false;
        }
    }
    public class CuttingUtil
    {
        public static Bar CreateBar(gPoint barLeftP, gPoint barRightP, double barWidth, double barLength)
        {
            Bar bar = new Bar();
            // 인자로 들어온 데이터 저장
            bar.Left = barLeftP;
            bar.Right = barRightP;
            bar.Width = barWidth;
            bar.Length = barLength;

            // 필요한 데이터 계산
            bar.Center = CurtainWallMath.GetCenterBy2Points(bar.Left, bar.Right);
            double halfWidth = bar.Width * 0.5;
            double halfLength = bar.Length * 0.5;

            // Rotation 계산
            Vector l2rVec = CurtainWallMath.GetVectorBy2Point(barRightP, barLeftP);
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

            bar.Left = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.Left, bar.Center);
            bar.Right = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.Right, bar.Center);
            bar.LT = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.LT, bar.Center);
            bar.RT = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.RT, bar.Center);
            bar.LB = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.LB, bar.Center);
            bar.RB = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.RB, bar.Center);
            bar.BOT = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.BOT, bar.Center);
            bar.TOP = CurtainWallMath.GetRotatedPoint(bar.Rotation, bar.TOP, bar.Center);

            return bar;
        }
        /// <summary>
        /// foundPoint 에 해당하는 Point를 originArray에서 탐색하여 반환
        /// </summary>
        /// <param name="originArray"></param>
        /// <param name="foundPoint"></param>
        /// <returns></returns>
        public static gPoint GetSamePointOnArray(gPoint[] originArray, gPoint foundPoint)
        {
            gPoint samePoint = null;
            for (int i = 0; i < originArray.Length; ++i)
            {
                if (CurtainWallMath.CompareDouble(originArray[i].x, foundPoint.x) &&
                    CurtainWallMath.CompareDouble(originArray[i].y, foundPoint.y))
                {
                    samePoint = new gPoint(originArray[i]);
                    break;
                }
            }
            return samePoint;
        }

        /// <summary>
        /// originArray에서 값이 겹치는 Point를 탐색하여 반환
        /// </summary>
        /// <param name="originArray"></param>
        /// <returns></returns>
        public static gPoint GetDuplicatePointOnArray(gPoint[] originArray)
        {
            gPoint duplicatePoint = null;
            for (int i = 0; i < originArray.Length; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    if (CurtainWallMath.CompareDouble(originArray[i].x, originArray[j].x) &&
                        CurtainWallMath.CompareDouble(originArray[i].y, originArray[j].y))
                    {
                        duplicatePoint = new gPoint(originArray[i]);
                        return duplicatePoint;
                    }
                }
            }
            return null;
        }

        public static bool IsSamePoint(gPoint pointA, gPoint pointB)
        {
            if (CurtainWallMath.CompareDouble(pointA.x, pointB.x) &&
                CurtainWallMath.CompareDouble(pointA.y, pointB.y))
            {
                return true;
            }
            return false;
        }

        public static List<PointAndDis> DistinctPAD_Points(List<PointAndDis> originList)
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

        public static List<PointAndDis> DistinctPAD_Distance(List<PointAndDis> originList)
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

        public static gPoint GetBarVertexSymmetryPoint(gPoint targetPoint, Bar targetBar)
        {
            gPoint target = new gPoint(CurtainWallMath.GetRotatedPoint(targetBar.Rotation * -1, targetPoint, targetBar.Center));
            Vector target2Center = CurtainWallMath.GetVectorBy2Point(targetBar.Center, target);

            double barLength = targetBar.Length * 0.5;
            double t2cLength = Math.Abs(target2Center.x);
            t2cLength = t2cLength - barLength;

            if (t2cLength != 0)
            {
                Vector translateVec = new Vector(target2Center.x, 0, 0);
                translateVec.Normalize();
                if (t2cLength < 0)
                    translateVec.x *= -1;
                translateVec *= Math.Abs(t2cLength);
                target = CurtainWallMath.GetExtendPoint(target, translateVec);
                target2Center = CurtainWallMath.GetVectorBy2Point(targetBar.Center, target);
            }

            gPoint result = CurtainWallMath.GetExtendPoint(target, target2Center);
            target2Center.y *= -1;
            result = CurtainWallMath.GetExtendPoint(result, target2Center);
            result = CurtainWallMath.GetRotatedPoint(targetBar.Rotation, result, targetBar.Center);
            return result;
        }

        public static void GetBarVertexSymmetryPoint(gPoint vertexPoint, Bar targetBar, out gPoint symmetryPoint, out gPoint diagonalPoint)
        {
            gPoint vertex = new gPoint(CurtainWallMath.GetRotatedPoint(targetBar.Rotation * -1, vertexPoint, targetBar.Center));
            Vector vertex2Center = CurtainWallMath.GetVectorBy2Point(targetBar.Center, vertex);

            double barLength = targetBar.Length * 0.5;
            double v2cLength = Math.Abs(vertex2Center.x);
            v2cLength = v2cLength - barLength;

            // vertexPoint를 Bar의 4꼭짓점 중 하나로 변환(가장 가까운 Bar 꼭짓점)
            if (v2cLength != 0)
            {
                Vector translateVec = new Vector(vertex2Center.x, 0, 0);
                translateVec.Normalize();
                if (v2cLength < 0)
                    translateVec.x *= -1;
                translateVec *= Math.Abs(v2cLength);
                vertex = CurtainWallMath.GetExtendPoint(vertex, translateVec);
                vertex2Center = CurtainWallMath.GetVectorBy2Point(targetBar.Center, vertex);
            }

            vertex2Center.y *= -1;
            symmetryPoint = CurtainWallMath.GetExtendPoint(targetBar.Center, vertex2Center);
            symmetryPoint = CurtainWallMath.GetRotatedPoint(targetBar.Rotation, symmetryPoint, targetBar.Center);

            vertex2Center.y *= -1;
            diagonalPoint = CurtainWallMath.GetExtendPoint(targetBar.Center, vertex2Center);
            diagonalPoint = CurtainWallMath.GetRotatedPoint(targetBar.Rotation, diagonalPoint, targetBar.Center);
        }
    }
}
