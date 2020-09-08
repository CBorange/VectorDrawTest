using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Geometry;

namespace Hicom.BizDraw.Geometry
{
    public class Circle
    {
        public gPoint Center { get; set; }
        public double Radius { get; set; }

        public bool IsCircle
        {
            get
            {
                if (this.Center == null || this.Radius <= 0)
                    return false;
                if (double.IsNaN(this.Center.x) || double.IsNaN(this.Center.y))
                    return false;
                return true;
            }
        }

        public Circle()
        {
        }

        public Circle(gPoint center, double radius)
        {
            this.Center = new gPoint(center);
            this.Radius = radius;
        }

        /// <summary>
        /// 세점을 지나는 원
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Circle(gPoint p1, gPoint p2, gPoint p3)
        {
            gPoint center;
            double radius;
            if(Circle.GetCircle(p1, p2, p3, out center, out radius))
            {
                this.Center = center;
                this.Radius = radius;
            }            
        }

        public Circle(gPoint p1, gPoint p2)
        {
            this.Center = (p1 + p2) / 2;
            this.Radius = p1.Distance2D(p2) / 2;
        }

        public void SetValue(gPoint center, double radius)
        {
            this.Center = new gPoint(center);
        }

        /// <summary>
        /// 선과의 교차되는 선분 찾기
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineIntersection"></param>
        /// <returns></returns>
        virtual public int Intersection(Line line, out linesegment lineIntersection)
        {
            gPoint p1 = new gPoint();
            gPoint p2 = new gPoint();
            int result = Globals.IntersectionLC2D(line.Point, line.ExtendPoint, this.Center, this.Radius, p1, p2);
            lineIntersection = new linesegment(p1, p2);
            return result;
        }

        /// <summary>
        ///  세점을 지나는 원의 반경
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public static bool GetCircle(gPoint p1, gPoint p2, gPoint p3, out gPoint center, out double radius)
        {
            gPoint pt1 = null;
            gPoint pt2 = null;
            gPoint pt3 = null;

            if (Geometry.Equals(p1.y, p2.y, Globals.VD_ZERO3))
            {
                pt1 = new gPoint(p1);
                pt2 = new gPoint(p3);
                pt3 = new gPoint(p2);
            }
            else if (Geometry.Equals(p2.y, p3.y, Globals.VD_ZERO3))
            {
                pt1 = new gPoint(p3);
                pt2 = new gPoint(p1);
                pt3 = new gPoint(p2);
            }
            else
            {
                pt1 = new gPoint(p1);
                pt2 = new gPoint(p2);
                pt3 = new gPoint(p3);
            }

            center = new gPoint();
            radius = 0;
            if (Geometry.Equals(pt2.y, pt1.y) == false && Geometry.Equals(pt3.y, pt2.y) == false)
            {
                double d1 = (pt2.x - pt1.x) / (pt2.y - pt1.y);
                double d2 = (pt3.x - pt2.x) / (pt3.y - pt2.y);
                if(Math.Abs(d2 - d1) > 0)
                {
                    center.x = ((pt3.y - pt1.y) + (pt2.x + pt3.x) * d2 - (pt1.x + pt2.x) * d1) / (2 * (d2 - d1));
                    center.y = -d1 * (center.x - (pt1.x + pt2.x) / 2) + (pt1.y + pt2.y) / 2;
                    radius = Math.Sqrt(Math.Pow(pt1.x - center.x, 2) + Math.Pow(pt1.y - center.y, 2));
                    return true;
                }
            }

            return false;
        }
    }
}
