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
        public const double VD_ZERO1 = 0.1;

        /// <summary>
        /// 두 Vector의 사잇각 구하기(예각)
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double GetBetweenAngle(this Vector v1, Vector v2)
        {
            Vector normal = new Vector(v1);
            Vector vector = new Vector(v2);
            normal.Normalize();
            vector.Normalize();

            if (Math.Abs(normal.Dot(vector) - 1) <= Globals.VD_ZERO8)
            {
                return 0;
            }
            if (Math.Abs(normal.Dot(vector) + 1) <= Globals.VD_ZERO8)
            {
                return Math.PI;
            }
            return Math.Acos(normal.Dot(vector));
        }

        public static bool IsVertical(this Vector vec)
        {
            return vec.IsParallel(new Vector(0, 1, 0), Globals.VD_ZERO3);
        }

        public static bool IsHorizontal(this Vector vec)
        {
            return vec.IsParallel(new Vector(1, 0, 0), Globals.VD_ZERO3);
        }

        public static bool IsVertical(this Vector vec, double equality)
        {
            return vec.IsParallel(new Vector(0, 1, 0), equality);
        }

        public static bool IsHorizontal(this Vector vec, double equality)
        {
            return vec.IsParallel(new Vector(1, 0, 0), equality);
        }

        /// <summary>
        /// 수직인 선분인지 판단
        /// </summary>
        /// <param name="seg"></param>
        /// <returns></returns>
        public static bool IsVertical(this linesegment seg)
        {
            return seg.NormalDirection().IsVertical();
        }

        public static bool IsVertical(this linesegment seg, double equality)
        {
            return seg.NormalDirection().IsVertical(equality);
        }

        public static bool IsHorizontal(this linesegment seg)
        {
            return seg.NormalDirection().IsHorizontal();
        }

        public static bool IsHorizontal(this linesegment seg, double equality)
        {
            return seg.NormalDirection().IsHorizontal(equality);
        }

        public static bool IsVertical(this vdLine line)
        {
            linesegment seg = new linesegment(line.StartPoint, line.EndPoint);
            return seg.IsVertical();
        }

        public static bool IsHorizontal(this vdLine line)
        {
            linesegment seg = new linesegment(line.StartPoint, line.EndPoint);
            return seg.IsHorizontal();
        }

        public static int Intersection(this linesegment seg, Line line, gPoint retPt, double equality = Globals.VD_ZERO4)
        {
            int result = Globals.IntersectionLL2D(seg.StartPoint, seg.EndPoint, line.Point, line.ExtendPoint, retPt);
            if(result == 1)
            {
                if (seg.IsInsideLineSegment(retPt, equality))
                    return 1;
            }
            return 0;
        }

        public static void Extend(this linesegment seg, linesegment other)
        {
            gPoint ptCross = new gPoint();
            int result = seg.IntersectionLL2D(other, ptCross);
            if(result == 1)
            {
                double distStt = ptCross.Distance2D(seg.StartPoint);
                double distEnd = ptCross.Distance2D(seg.EndPoint);
                bool onpoint1 = seg.IsInsideLineSegment(ptCross);
                bool onpoint2 = other.IsInsideLineSegment(ptCross);
                if ( !onpoint1 && onpoint2 )
                {
                    if (distStt < distEnd)
                        seg.StartPoint.Set(ptCross);
                    else
                        seg.EndPoint.Set(ptCross);
                }
            }
        }

        public static gPoint TrimStartPoint(gPoint ptStart, gPoint ptEnd, double trim)
        {
            Vector vecDir = new Vector(ptEnd - ptStart);
            vecDir.Normalize();
            Vector vecTrim = new Vector(vecDir * trim);
            return new Vector(ptStart) + vecTrim;
        }

        public static gPoint TrimEndPoint(gPoint ptStart, gPoint ptEnd, double trim)
        {
            Vector vecDir = new Vector(ptEnd - ptStart);
            vecDir.Normalize();
            Vector vecTrim = new Vector(vecDir * trim);
            return new Vector(ptEnd) - vecTrim;
        }

        public static void Set(this gPoint point, gPoint other)
        {
            point.x = other.x;
            point.y = other.y;
            point.z = other.z;
        }
        /// <summary>
        /// linesegment Offset
        /// </summary>
        /// <param name="seg"></param>
        /// <param name="dir"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        public static linesegment Offset(this linesegment seg, Vector dir, double dist)
        {
            Vector vec = new Vector(dir);
            vec.Normalize();

            gPoint stt = seg.StartPoint + vec * dist;
            gPoint end = seg.EndPoint + vec * dist;

            return new linesegment(stt, end);
        }

        /// <summary>
        /// 선분안에 포인트가 존재하는지 여부
        /// </summary>
        /// <param name="seg"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static bool IsInsideLineSegment(this linesegment seg, gPoint pt, double equality = Globals.VD_ZERO4)
        {
            double dist1 = seg.StartPoint.Distance2D(pt);
            double dist2 = seg.EndPoint.Distance2D(pt);

            double dist = dist1 + dist2;
            if (Math.Abs(dist - seg.length) < equality)
                return true;
            return false;
        }

        public static eConnectedPosition GetConnectedPosition(this linesegment seg, linesegment other, out gPoint pointCross, double equality = Globals.VD_ZERO4)
        {
            pointCross = new gPoint();
            int result = seg.Intersection(other, pointCross, Geometry.VD_ZERO1);
            if(result == 1)
            {
                if (Geometry.Equals(seg.StartPoint, pointCross, Geometry.VD_ZERO1)) // Globals.VD_ZERO2
                    return eConnectedPosition.START;
                else if (Geometry.Equals(seg.EndPoint, pointCross, Geometry.VD_ZERO1)) // Globals.VD_ZERO2
                    return eConnectedPosition.END;
                else
                    return eConnectedPosition.INNER;
            }
            else
            {
                if (seg.IsConnectedStart(other, out pointCross, equality))
                    return eConnectedPosition.START;
                if(seg.IsConnectedEnd(other, out pointCross, equality))
                    return eConnectedPosition.END;
            }
            
            return eConnectedPosition.None;
        }

        public static linesegment ToLinesegment(gPoint p1, gPoint p2, Vector vec)
        {
            vec.Normalize();
            gPoints points = new gPoints() { p1, p2 };
            Geometry.Sort(ref points, vec);
            return new linesegment(new gPoint(points[0]), new gPoint(points[1]));
        }

        public static linesegment ToLinesegment(linesegment seg, Vector vec)
        {
            if(seg != null)
                return ToLinesegment(seg.StartPoint, seg.EndPoint, vec);
            return null;
        }

        /// <summary>
        /// 두 벡터의 평행여부
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <param name="equality"></param>
        /// <returns></returns>
        public static bool IsParallel(this Vector vec1, Vector vec2, double equality = Globals.VD_ZERO4) 
        {
            if (vec1.Normalize() && vec2.Normalize())
            {
                double dot = Math.Abs(vec1.Dot(vec2)) - 1;
                if (Math.Abs(dot) < equality)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 두 선분의 평행 여부 판단
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <returns></returns>
        public static bool IsParallel(this linesegment seg1, linesegment seg2)
        {
            Vector vec1 = seg1.NormalDirection();
            Vector vec2 = seg2.NormalDirection();

            return vec1.IsParallel(vec2);
        }

        /// <summary>
        /// Linesegment의 단위 방향벡터
        /// </summary>
        /// <param name="seg"></param>
        /// <returns></returns>
        public static Vector NormalDirection(this linesegment seg)
        {
            if(seg != null && seg.StartPoint != null && seg.EndPoint != null)
            {
                Vector vec = new Vector(seg.EndPoint - seg.StartPoint);
                vec.Normalize();
                return vec;
            }

            return new Vector(0, 0, 0);
        }

        /// <summary>
        /// lineSeg1에서 lineSeg2가 있는 수직방향을 Vector를 찾는다.
        /// </summary>
        /// <param name="lineSeg1"></param>
        /// <param name="lineSeg2"></param>
        /// <returns></returns>
        public static Vector GetCrossDirection(this linesegment lineSeg1, linesegment lineSeg2)
        {
            linesegment seg1 = lineSeg1;
            linesegment seg2 = lineSeg2;

            if (seg1.IsParallel(seg2) == false)
                return null;
            Vector vecCross = seg1.NormalDirection();
            vecCross.Rotate90();
            linesegment segStt = new linesegment(seg1.StartPoint, seg1.StartPoint + vecCross * 1000);

            gPoint retStt = new gPoint(0, 0);
            int result1 = Globals.IntersectionLL2D(segStt.StartPoint, segStt.EndPoint, seg2.StartPoint, seg2.EndPoint, retStt);

            Vector dir = new Vector(retStt - segStt.StartPoint);
            dir.Normalize();

            if(dir == null)
            {
                return null;
            }

            return dir;
        }


        /// <summary>
        /// 평행한 두선분간의 수직 거리 구하기
        /// 두선분이 겹쳐지지 않으면 distance는 0이다
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <returns></returns>
        public static double GetCrossDistance(this linesegment lineSeg1, linesegment lineSeg2)
        {
            double distance = 0;
            linesegment seg1 = lineSeg1;
            linesegment seg2 = lineSeg2;

            if (seg1.IsParallel(seg2) == false)
                return distance;
            Vector vecCross = seg1.NormalDirection();
            vecCross.Rotate90();
            linesegment segStt = new linesegment(seg1.StartPoint, seg1.StartPoint + vecCross * 1000);
            linesegment segEnd = new linesegment(seg1.EndPoint, seg1.EndPoint + vecCross * 1000);

            gPoint retStt = new gPoint(0, 0);
            gPoint retEnd = new gPoint(0, 0);
            int result1 = Globals.IntersectionLL2D(segStt.StartPoint, segStt.EndPoint, seg2.StartPoint, seg2.EndPoint, retStt);
            int result2 = Globals.IntersectionLL2D(segEnd.StartPoint, segEnd.EndPoint, seg2.StartPoint, seg2.EndPoint, retEnd);
            
            if (result1 == 1 && result2 == 1)
            {
                distance = seg1.StartPoint.Distance2D(retStt);
            }
            return distance;
        }

        /// <summary>
        /// 박스간 거리 구하기
        /// 두 박스가 겹칠 경우 0 반환
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static double GetBoxDistance(this Box box1, Box box2)
        {
            if (box1.Intersection(box2))
            {
                return 0;
            }

            Box left = box1.Center().x < box2.Center().x ? box1 : box2;
            Box right = box2.Center().x < box1.Center().x ? box1 : box2;

            double xDifference = left.Center().x == right.Center().x ? 0 : right.Center().x - (left.Center().x + left.Width);
            xDifference = Math.Max(0, xDifference);

            Box upper = box1.Center().y < box2.Center().y ? box1 : box2;
            Box lower = box2.Center().y < box1.Center().y ? box1 : box2;

            double yDifference = upper.Center().y == lower.Center().y ? 0 : lower.Center().y - (upper.Center().y + upper.Height);
            yDifference = Math.Max(0, yDifference);

            return Math.Sqrt(xDifference * xDifference + yDifference * yDifference);
        }
        
        /// <summary>
        /// 90도 회전
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="left"></param>
        public static void Rotate90(this Vector vec, bool left = true)
        {
            Vector posDir;
            if (left)
                posDir = new Vector(-vec.y, vec.x, vec.z);
            else
                posDir = new Vector(vec.y, -vec.x, vec.z);

            vec.x = posDir.x;
            vec.y = posDir.y;
            vec.z = posDir.z;
        }
        
        /// <summary>
        /// 두 점의 내적
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double GetInner(this gPoint point1, gPoint point2)
        {
            return (point1.x * point2.x) + (point1.y * point2.y) + (point1.z * point2.z);
        }

        /// <summary>
        /// 두 선분의 교차점
        /// </summary>
        /// <param name="seg1"></param>
        /// <param name="seg2"></param>
        /// <param name="retpt"></param>
        /// <returns>0 : 교차점 없음 1 : 교차점 있음</returns>
        public static int Intersection(this linesegment seg1, linesegment seg2, gPoint retpt, double equality = Geometry.VD_ZERO1) //Globals.VD_ZERO4
        {
            if(seg1.IntersectionLL2D(seg2, retpt) == 1)
            {
                bool inside1 = seg1.IsInsideLineSegment(retpt, equality);
                bool inside2 = seg2.IsInsideLineSegment(retpt, equality);
                if (inside1 && inside2)
                    return 1;
            }
            return 0;
        }

        //무한선이 소스, 대상이 일반선
        public static int IntersectionInfinitySrc(this linesegment srcSeg, linesegment destSeg, gPoint retpt, double equality = Geometry.VD_ZERO1) //Globals.VD_ZERO4
        {
            if (srcSeg.IntersectionLL2D(destSeg, retpt) == 1)
            {
                bool inside = destSeg.IsInsideLineSegment(retpt, equality);
                if (inside)
                    return 1;
            }
            return 0;
        }

        //일반선이 소스, 대상이 무한선
        public static int IntersectionInfinityDes(this linesegment srcSeg, linesegment destSeg, gPoint retpt, double equality = Geometry.VD_ZERO1) //Globals.VD_ZERO4
        {
            if (srcSeg.IntersectionLL2D(destSeg, retpt) == 1)
            {
                bool inside = srcSeg.IsInsideLineSegment(retpt, equality);
                if (inside)
                    return 1;
            }
            return 0;
        }

        public static int IntersectionLL2D(this linesegment seg1, linesegment seg2, gPoint retpt)
        {
            return Globals.IntersectionLL2D(seg1.StartPoint, seg1.EndPoint, seg2.StartPoint, seg2.EndPoint, retpt);
        }

        public static int IntersectionLL2D(this linesegment seg1, Line line, gPoint retpt)
        {
            return Globals.IntersectionLL2D(seg1.StartPoint, seg1.EndPoint, line.Point, line.ExtendPoint, retpt);
        }

        /// <summary>
        /// 벡터를 양의 방향 벡터로 바꾼다
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector ToPositive(this Vector v)
        {
            return new Vector(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
        }

        public static bool IsConnectedStart(this linesegment src, linesegment other, out gPoint point, double equality = Globals.VD_ZERO3)
        {
            point = null;
            if (src.StartPoint.AreEqual(other.StartPoint, equality) || src.StartPoint.AreEqual(other.EndPoint, equality))
            {
                point = src.StartPoint;
                return true;
            }
            return false;
        }

        public static bool IsConnectedEnd(this linesegment src, linesegment other, out gPoint point, double equality = Globals.VD_ZERO3)
        {
            point = null;
            if (src.EndPoint.AreEqual(other.StartPoint, equality) || src.EndPoint.AreEqual(other.EndPoint, equality))
            {
                point = src.EndPoint;
                return true;
            }
            return false;
        }

        public static gPoint Round(this gPoint point, int round = 3)
        {
            gPoint result = new gPoint();
            result.x = Math.Round(point.x, round);
            result.y = Math.Round(point.y, round);
            result.z = Math.Round(point.z, round);
            return result;
        }

        public static Vector Round(this Vector vec, int round = 5)
        {
            Vector result = new Vector();
            result.x = Math.Round(vec.x, round);
            result.y = Math.Round(vec.y, round);
            result.z = Math.Round(vec.z, round);
            return result;
        }
        
        /// <summary>
        /// 정점배열 중 동일한 정점은 제거함
        /// </summary>
        /// <param name="PointList"></param>
        static public void RemoveSamePoint(this gPoints points, double equality = Globals.VD_ZERO3)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                gPoint a = new gPoint(points[i]);
                
                for (int j = i + 1; j < points.Count; j++)
                {
                    gPoint b = new gPoint(points[j]);
                    
                    if (a.AreEqual(b, equality))
                    {
                        points.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        static public void RemoveSamePoint(this Vertexes points, double equality = Globals.VD_ZERO3)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                gPoint a = new gPoint(points[i]);

                for (int j = i + 1; j < points.Count; j++)
                {
                    gPoint b = new gPoint(points[j]);

                    if (a.AreEqual(b, equality))
                    {
                        points.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        public static void RemoveSameDirectionPoint(this gPoints points, double dEpsilon = 0.01)
        {
            // 동일선상에 있는 정점 제거
            if (points.Count >= 3)
            {
                for (int idx = 0; idx < points.Count; idx++)
                {
                    int idxEnd = idx + 2;
                    int idxMid = idx + 1;
                    if (idx == points.Count - 2)
                    {
                        idxEnd = 0;
                        idxMid = points.Count - 1;
                    }
                    else if (idx == points.Count - 1)
                    {
                        idxEnd = 1;
                        idxMid = 0;
                    }

                    gPoint start = points[idx];
                    gPoint mid = points[idxMid];
                    gPoint End = points[idxEnd];

                    Vector vector1 = new Vector(mid - start);
                    vector1.Normalize();
                    Vector vector2 = new Vector(End - mid);
                    vector2.Normalize();

                    if (vector1.AreEqual(vector2, 0.01))
                    {
                        if (idx == points.Count - 1)
                        {
                            points.RemoveAt(0);
                        }
                        else
                        {
                            points.RemoveAt(idx + 1);
                            idx--;
                        }
                    }
                }
            }
        }

        public static bool Intersection(this Box box, Box other)
        {
            if (box.PointInBox(other.Min))
                return true;
            if (box.PointInBox(other.Max))
                return true;
            if (box.PointInBox(other.LowerRight))
                return true;
            if (box.PointInBox(other.UpperLeft))
                return true;
            return false;
        }

        public static bool Intersection2(this Box box, Box other)
        {
            Box srcBox = box;
            Box figBox = other;

            if (srcBox.Top >= figBox.Bottom && srcBox.Right >= figBox.Left &&
                srcBox.Left <= figBox.Right && srcBox.Bottom <= figBox.Top)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Intersection(this Box box, linesegment lineSeg)
        {
            linesegment bottom = new linesegment(box.Min, box.LowerRight);
            linesegment left = new linesegment(box.Min, box.UpperLeft);
            linesegment top = new linesegment(box.UpperLeft, box.Max);
            linesegment right = new linesegment(box.Max, box.LowerRight);

            gPoint pt = new gPoint();
            if (bottom.IntersectionInfinityDes(lineSeg, pt) == 1)
                return true;

            if (left.IntersectionInfinityDes(lineSeg, pt) == 1)
                return true;

            if (top.IntersectionInfinityDes(lineSeg, pt) == 1)
                return true;

            if (right.IntersectionInfinityDes(lineSeg, pt) == 1)
                return true;

            return false;
        }

        public static Box ExtendBox(this Box box, double dist)
        {
            Vector vecH = new Vector(box.LowerRight - box.Min);
            vecH.Normalize();
            Vector vecV = new Vector(box.UpperLeft - box.Min);
            vecV.Normalize();

            linesegment bottom = new linesegment(box.Min - vecV * dist, box.LowerRight - vecV * dist);
            linesegment left = new linesegment(box.Min - vecH * dist, box.UpperLeft - vecH * dist);
            linesegment top = new linesegment(box.UpperLeft + vecV * dist, box.Max + vecV * dist);
            linesegment right = new linesegment(box.Max + vecH * dist, box.LowerRight + vecH * dist);

            gPoint min = new gPoint();
            gPoint max = new gPoint();
            if(Globals.IntersectionLL2D(bottom.StartPoint, bottom.EndPoint, left.StartPoint, left.EndPoint, min) == 1 &&
                Globals.IntersectionLL2D(top.StartPoint, top.EndPoint, right.StartPoint, right.EndPoint, max) == 1)
            {
                return new Box(min, max);
            }
            return box;
        }

        public static gPoints ToPoints(this Vertexes ver)
        {
            gPoints points = new gPoints();
            for (int ix = 0; ix < ver.Count; ix++)
                points.Add(ver[ix]);
            return points;
        }

        public static List<linesegment> ToLinesegments(this Vertexes vert)
        {
            List<linesegment> result = new List<linesegment>();
            for(int ix = 0; ix < vert.Count; ix++)
            {
                gPoint p1 = vert[ix];
                gPoint p2 = ix+1 == vert.Count ? vert[0] : vert[ix + 1];

                result.Add(new linesegment(p1, p2));
            }

            return result;
        }

        public static gPoint Center(this Box box)
        {
            return (box.Min + box.Max) / 2;
        }

        public static bool AreEqual(this Box b1, Box b2, double equality = Globals.VD_ZERO2)
        {
            bool min = b1.Min.AreEqual(b2.Min, equality);
            bool max = b1.Max.AreEqual(b2.Max, equality);
            return min && max;
        }

        public static bool AreEqual(this double d1, double d2, double equality = Globals.VD_ZERO3)
        {
            return Globals.AreEqual(d1, d2, equality);
        }
        
        public static bool IsInsidePolygon(gPoints polygon1, gPoints polygon2, bool lineOn = false)
        {
            for (int ix = 0; ix < polygon2.Count; ix++)
            {
                int result = IsInsidePolygon(polygon1, polygon2[ix]);
                if ( (lineOn == false &&  result != 1 ) || (lineOn && (result != 1 && result != 0)))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 한점이 Close된 Polygon 내에 있는지 조사. (Arc는 직선으로 간주)
        /// </summary>
        /// <param name="pointList"></param>
        /// <param name="point"></param>
        /// return =  1 : 내부
        /// return =  0 : 경계선상
        /// return = -1 : 외부
        /// return = -2 : 에러
        public static int IsInsidePolygon(gPoints pointList, gPoint point)
        {
            int sizeOfVertexs = pointList.Count;

            if (sizeOfVertexs < 3)
            {
                return -1;
            }

            int followIndex = sizeOfVertexs - 1;
            bool isOddNodes = false;

            for (int frontIndex = 0; frontIndex < sizeOfVertexs; frontIndex++)
            {
                gPoint frontPoint = pointList[frontIndex];
                gPoint followPoint = pointList[followIndex];
                
                linesegment lineSeg = new linesegment(frontPoint, followPoint);
                if (lineSeg.IsInsideLineSegment(point) == true)
                    return 0;

                if (frontPoint.y < point.y && followPoint.y >= point.y || followPoint.y < point.y && frontPoint.y >= point.y)
                {
                    if (frontPoint.x + (point.y - frontPoint.y) / (followPoint.y - frontPoint.y) * (followPoint.x - frontPoint.x) < point.x)
                    {
                        isOddNodes = !isOddNodes;
                    }
                }

                followIndex = frontIndex;
            }

            return isOddNodes ? 1 : -1;
        }

        public static gPoint GetCenterPoint(gPoint pt1, gPoint pt2)
        {
            gPoint center = new gPoint();
            if ( pt1 == pt2)
            {
                center = pt1;
            }
            else
            {
                center.x = pt1.x + (pt2.x - pt1.x) / 2.0f;
                center.y = pt1.y + (pt2.y - pt1.y) / 2.0f;
                center.z = pt1.z + (pt2.z - pt1.z) / 2.0f;
            }

            return center;
        }

        public static gPoint MidPoint(this vdLine line)
        {
            return GetCenterPoint(line.StartPoint, line.EndPoint);
        }
        
        public static Vertex FindNearVertex(this Vertexes vertexes, gPoint pt)
        {
            if (vertexes.Count == 0)
                return null;

            Vertex near = vertexes[0];
            Vertex cur = near;
            gPoint ptCur = new gPoint(cur.x, cur.y);
            double distanceBase = pt.Distance2D(ptCur);
            for ( int ix = 1; ix < vertexes.Count; ++ix)
            {
                cur = vertexes[ix];
                ptCur = new gPoint(cur.x, cur.y);
                double distance = pt.Distance2D(ptCur);

                if (distanceBase > distance)
                {
                    distanceBase = distance;
                    near = cur;
                }
            }
            return near;
        }

        public static void Offset(this gPoints points, gPoint ptBase)
        {
            for ( int ix = 0; ix < points.Count; ++ix )
            {
                points[ix].x = ptBase.x + points[ix].x;
                points[ix].y = ptBase.y + points[ix].y;
                points[ix].z = ptBase.z + points[ix].z;
            }
        }
    }
}
