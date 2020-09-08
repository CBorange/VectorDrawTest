using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;

namespace Hicom.BizDraw.Geometry
{
    public static partial class Geometry
    {
        public enum eConnectedPosition
        {
            None = 0x0000,
            START = 0x0001,
            END = 0x0002,
            INNER = 0x0004            
        }
        public static Vector Xaxis = new Vector(1, 0, 0);
        public static Vector Yaxis = new Vector(0, 1, 0);

        /// <summary>
        /// 두 선분의 사이에(예각)에 point가 존재하는지 여부
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsBetween(linesegment seg1, linesegment seg2, gPoint point)
        {
            gPoint retpt = new gPoint();

            int result = Globals.IntersectionLL2D(seg1.StartPoint, seg1.EndPoint, seg2.StartPoint, seg2.EndPoint, retpt);
            bool isBetween = false;
            if (result == 1)
            {
                linesegment seg = new linesegment(retpt, point);
                Vector v1 = seg1.NormalDirection();
                Vector v2 = seg2.NormalDirection();
                Vector v3 = seg.NormalDirection();

                double angle = Globals.RadiansToDegrees(v1.GetBetweenAngle(v2));
                double ang1 = Globals.RadiansToDegrees(v3.GetBetweenAngle(v1));
                double ang2 = Globals.RadiansToDegrees(v3.GetBetweenAngle(v2));
                isBetween = Globals.AreEqualAngle(angle, (ang1+ang2), Globals.VD_ZERO5);
            }
            return isBetween;
        }
         
        public static int GetVerticalIntersection(linesegment seg, gPoint point, gPoint result)
        {
            Vector vec = new Vector(seg.NormalDirection());
            vec.Rotate90();
            Line line = new Line(point, vec);
            
            int ret = Globals.IntersectionLL2D(seg.StartPoint, seg.EndPoint, line.Point, line.ExtendPoint, result);
            return ret;
        }

        public static double GetVerticalDistance(linesegment seg, gPoint point)
        {
            gPoint retpt = new gPoint();
            int result = GetVerticalIntersection(seg, point, retpt);
            double distance = 0;
            if(result == 1)
            {
                distance = retpt.Distance2D(point);
            }
            return distance;
        }
        
        public static double Max(params double[] values)
        {
            double result = Double.MinValue;
            for (int ix = 0; ix < values.Length; ix++)
                result = Math.Max(result, values[ix]);
            return result;
        }

        public static double Min(params double[] values)
        {
            double result = Double.MaxValue;
            for (int ix = 0; ix < values.Length; ix++)
                result = Math.Min(result, values[ix]);
            return result;
        }

        /// <summary>
        /// 두점을 지나는 선상에 point가 있는지 여부
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="pt"></param>
        /// <param name="equality"></param>
        /// <returns></returns>
        public static bool PointOnLine(gPoint p1, gPoint p2, gPoint pt, double equality = Globals.VD_ZERO8)
        {
            if ( Geometry.Equals(p1, pt, equality) || Geometry.Equals(p2, pt, equality))
                return true;

            Vector v1 = new Vector(p1 - p2);
            Vector v2 = new Vector(pt - p2);
            v1.Normalize();
            v2.Normalize();

            double angle = GetBetweenAngle(v1, v2);
            return Geometry.Equals(angle, 0, equality) || Geometry.Equals(angle, Math.PI, equality);
        }

        /// <summary>
        /// seg2와 교차점까지 seg1을 Extend 시킨다.
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <returns></returns>
        public static linesegment ExtendLineSegment(linesegment seg1, linesegment seg2)
        {
            if (seg1.IsParallel(seg2))
                return null;

            gPoint ret = new gPoint(0, 0);
            int result = seg1.Intersection(seg2, ret);
            if (result == 0)
                return null;

            gPoints points = new gPoints();
            points.Add(seg1.StartPoint);
            points.Add(seg2.EndPoint);
            points.Add(ret);
            Geometry.Sort(ref points, seg1.NormalDirection());

            linesegment seg = new linesegment(points[0], points[2]);
            return seg;
        }

        public static void Sort(ref gPoints points, Vector vDir)
        {
            DoubleArray dArray = new DoubleArray();
            for (int ix = 0; ix < points.Count; ix++)
            {
                gPoint p = new gPoint(vDir.x, vDir.y, vDir.z);
                double inner = points[ix].GetInner(p);
                dArray.Add(inner);
            }

            for (int i = 1; i < points.Count; i++)
            {
                double tempDis = dArray[i];
                gPoint tempPos = points[i];
                int j = i;
                while (j > 0 && dArray[j - 1] > tempDis)
                {
                    dArray[j] = dArray[j - 1];
                    points[j] = points[j - 1];
                    j--;
                }
                dArray[j] = tempDis;
                points[j] = tempPos;
            }
        }

        /// <summary>
        /// 선분과 수직인 선분 구하기
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static linesegment GetCrossLineSegment(gPoint pt, Vector dir, bool bLeft = true, double length = 9999)
        {
            Vector vec = new Vector(dir);
            dir.Normalize();
            vec.Rotate90(bLeft);

            gPoint pt2 = pt + vec * length;

            linesegment result = new linesegment(pt, pt2);
            return result;

        }

        public static linesegment ToPositiveDirection(linesegment seg)
        {
            gPoints points = new gPoints();
            points.Add(seg.StartPoint);
            points.Add(seg.EndPoint);

            Geometry.Sort(ref points, seg.NormalDirection().ToPositive());
            return new linesegment(points[0], points[1]);
        }
        
        /// <summary>
        /// Rect 객체를 선분으로 분해
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static List<vdLine> Explode(vdRect rect)
        {
            List<vdLine> result = new List<vdLine>();
            vdEntities entities = rect.Explode();
            for (int ix = 0; ix < entities.Count; ix++)
            {
                vdPolyline polyLine = entities[ix] as vdPolyline;
                result.AddRange(Geometry.Explode(polyLine));
            }

            return result;
        }

        /// <summary>
        /// PolyLine 객체를 선분으로 분해
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static List<vdLine> Explode(vdPolyline polyline)
        {
            List<vdLine> result = new List<vdLine>();
            vdEntities ent = polyline.Explode();

            for (int jx = 0; jx < ent.Count; jx++)
            {
                if (ent[jx] is vdLine)
                {
                    vdLine line = ent[jx] as vdLine;
                    result.Add(line);
                }
            }

            return result;
        }

        public static void RemoveEqualSegment(List<linesegment> listSeg)
        {
            for (int ix = 0; ix < listSeg.Count; ix++)
            {
                linesegment seg1 = listSeg[ix];
                for (int jx = ix + 1; jx < listSeg.Count; jx++)
                {
                    linesegment seg2 = listSeg[jx];
                    if (seg1.AreEqual(seg2, Globals.VD_ZERO4))
                    {
                        listSeg.Remove(seg2);
                        jx--;
                    }
                }
            }
        }

        static public bool Equals(gPoint p1, gPoint p2, double equality = Globals.VD_ZERO4)
        {
            bool x = Math.Abs(p1.x - p2.x) < equality;
            bool y = Math.Abs(p1.y - p2.y) < equality;
            bool z = Math.Abs(p1.z - p2.z) < equality;
            return x && y && z;
        }

        static public bool Equals(double d1, double d2, double equality = Globals.VD_ZERO6)
        {
            if (System.Math.Abs(d1 - d2) < equality)
                return true;

            return false;
        }
        
        /// <summary>
        /// 외적((2차원)
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        static public double GetOuter(gPoint point1, gPoint point2)
        {
            return (point1.x * point2.y) - (point1.y * point2.x);
        }
        
        /// <summary>
        /// 차례로 진행하는 세점의 회전 방향을 구한다 (+1 : 시계방향회전 , -1 : 시계반대 방향, 0 : 선상)
        /// </summary>
        /// <param name="pointStt">시점</param>
        /// <param name="pointCen">중간점</param>
        /// <param name="pointEnd">종점</param>
        /// <returns>
        ///       +1 : 시계방향회전 , -1 : 시계반대 방향, 0 : 선상
        /// </returns>
        static public int GetDirectionRotateByPoint(gPoint pointStt, gPoint pointCen, gPoint pointEnd, int round = 6)
        {
            Vector vectorCenStt = new Vector(pointCen - pointStt);
            vectorCenStt.Normalize();

            Vector vectorEndCen = new Vector(pointCen - pointEnd);
            vectorEndCen.Normalize();
            
            double outer = Math.Round(Geometry.GetOuter(vectorCenStt, vectorEndCen), round);
            if (outer > 0)
                return 1;
            else if (outer < 0)
                return -1;
            else
                return 0;
        }
        
        /// <summary>
        /// 하나의 기준점에서 서로 다른 점들간의 방향이 기준방향에 일치하는 점들을 구한다.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="targets"></param>
        /// <param name="standardDirection"></param>
        /// <returns></returns>
        static public gPoints GetEqulsDirectionPoints(gPoint origin, gPoints targets, Vector standardDirection)
        {
            gPoints results = new gPoints();

            for (int ix = 0; ix < targets.Count; ix++)
            {
                gPoint target = targets[ix];
                Vector targetDirection = origin.Direction(target);

                if (standardDirection.Equals(targetDirection))
                    results.Add(target);
            }
            
            return results;
        }

        static public List<linesegment> GetLinesegments(this Box box)
        {
            List<linesegment> result = new List<linesegment>();

            result.Add(box.GetLinesegment(0));
            result.Add(box.GetLinesegment(1));
            result.Add(box.GetLinesegment(2));
            result.Add(box.GetLinesegment(3));
            return result;
        }

        static public linesegment GetLinesegment(this Box box, int dir)
        {
            linesegment linesegment = null;
            switch(dir)
            {
                case 0:     // Left
                    linesegment = new linesegment(box.Min, box.UpperLeft);
                    break;
                case 1:     // Top
                    linesegment = new linesegment(box.UpperLeft, box.Max);
                    break;
                case 2: // Right
                    linesegment = new linesegment(box.Max, box.LowerRight);
                    break;
                case 3:// bottom
                    linesegment = new linesegment(box.LowerRight, box.Min);
                    break;
            }

            return linesegment;
        }

        static public void InflateBox(this Box box, double left, double top, double right, double bottom)
        {
            linesegment leftSeg = box.GetLinesegment(0);
            linesegment topSeg = box.GetLinesegment(1);
            linesegment rightSeg = box.GetLinesegment(2);
            linesegment bottomSeg = box.GetLinesegment(3);

            Vector vecLeft = new Vector(leftSeg.NormalDirection());
            Vector vecTop = new Vector(topSeg.NormalDirection());
            Vector vecRight = new Vector(rightSeg.NormalDirection());
            Vector vecBottom = new Vector(bottomSeg.NormalDirection());

            if (left > 0)       vecLeft.Rotate90(true);
            else                vecLeft.Rotate90(false);

            if (top > 0) vecTop.Rotate90(true);
            else vecTop.Rotate90(false);

            if (right > 0) vecRight.Rotate90(true);
            else vecRight.Rotate90(false);

            if (bottom > 0) vecBottom.Rotate90(true);
            else vecBottom.Rotate90(false);

            leftSeg = leftSeg.Offset(vecLeft, left);
            topSeg = topSeg.Offset(vecTop, top);
            rightSeg = rightSeg.Offset(vecRight, right);
            bottomSeg = bottomSeg.Offset(vecBottom, bottom);

            gPoint ptMin = new gPoint();
            gPoint ptMax = new gPoint();

            int result1 = leftSeg.IntersectionLL2D(bottomSeg, ptMin);
            int result2 = topSeg.IntersectionLL2D(rightSeg, ptMax);

            if(result1 == 1 && result2 == 1)
            {
                box.AddPoint(ptMin);
                box.AddPoint(ptMax);
            }
        }

        static public double GetAngleRadian(this gPoint pt)
        {
            if (pt.x == 0 && pt.y == 0 || Math.Sqrt(pt.x * pt.x + pt.y * pt.y) == 0) // 길이
                return 0;

            double res = Math.Acos(pt.x / Math.Sqrt(pt.x * pt.x + pt.y * pt.y)); // 길이

            return pt.y >= 0.0 ? res : 2 * Math.PI - res;
        }

        static public double GetAngleDegree(this gPoint pt)
        {
            return 180 / Math.PI * Geometry.GetAngleRadian(pt);
        }

        // 개발 필요
        //static public double GetArcLength(gPoint p1, gPoint p2, gPoint p3)
        //{
        //    return 0.0;
        //}

        // 개발 필요
        //static public double GetCircleLength(gPoint p1, gPoint p2, gPoint p3)
        //{ 
        //    return 0.0;
        //}
    }
}
