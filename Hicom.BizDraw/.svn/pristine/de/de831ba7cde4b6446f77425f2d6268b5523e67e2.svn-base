using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Geometry;

namespace Hicom.BizDraw.Geometry
{
    public class Arc : Circle
    {
        public double StartAngle { get; set; }        
        public double EndAngle { get; set; }
        public gPoint StartPoint
        {
            get
            {
                Vector vec = new Vector(StartAngle);
                vec.Normalize();
                return this.Center + vec * this.Radius;
            }
        }

        public gPoint EndPoint
        {
            get
            {
                Vector vec = new Vector(EndAngle);
                vec.Normalize();
                return this.Center + vec * this.Radius;
            }
        }

        public Arc()
        {

        }

        public Arc(gPoint center, double radius, double startAngle, double endAngle) : base(center, radius)
        {
            this.StartAngle = startAngle;
            this.EndAngle = endAngle;
        }

        // 3점을 지나는 Arc
        public Arc(gPoint first, gPoint secondary, gPoint third) : base(first, secondary, third)
        {
            if(this.IsCircle)
            {   
                double ang1 = this.Center.GetAngle(first);
                double ang3 = this.Center.GetAngle(third);

                // 첫번째, 두번째 점의 방향으로 세번째 점의 회전방향을 구한다.
                int rotDir = Geometry.GetDirectionRotateByPoint(first, secondary, third);
                
                // 세번째 점이 시계방향 회전이면 반시계방향으로 arc 생성(autocad 기준)
                if(rotDir == 1)
                {
                    // Clockwise
                    this.StartAngle = ang3;
                    this.EndAngle = ang1;
                }
                else if(rotDir == -1)
                {
                    // CountClockwise
                    this.StartAngle = ang1;
                    this.EndAngle = ang3;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="startPoint"></param>
        /// <param name="endAngle"> 글로벌 X축과의 각도</param>
        public Arc(gPoint center, gPoint startPoint, double endAngle) : base(center, center.Distance2D(startPoint))
        {
            this.StartAngle = center.GetAngle(startPoint);
            this.EndAngle = endAngle;
        }

        public override int Intersection(Line line, out linesegment lineIntersection)
        {
            int result = base.Intersection(line, out lineIntersection);
            if(result == 1)
            {

            }

            return result;

        }

        public gPoint MiddlePoint()
        {
            Vector vStt = new Vector(this.StartPoint - this.Center);
            Vector vEnd = new Vector(this.EndPoint - this.Center);
            double betAng = vStt.GetBetweenAngle(vEnd);
            Vector vec = new Vector(this.StartAngle + betAng / 2);
            vec.Normalize();
            gPoint result = this.Center + vec * this.Radius;
            return result;
        }

    }
}
