using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using VectorDraw.Geometry;
using VectorDraw.Serialize;

namespace Hicom.BizDraw.Geometry
{
    public class Line : IVDSerialise, IFromString
    {
        private const double Length = 1000;
        public gPoint Point { get; set; }
        public Vector Direction { get; set; }
        public gPoint ExtendPoint
        {
            get
            {
                return this.Point + this.Direction * Line.Length;
            }
        }
                
        public Line(gPoint point, Vector direction)
        {
            this.Point = new gPoint(point);
            this.Direction = new Vector(direction);
            this.Direction.Normalize();
            this.Direction.z = 0;
        }

        public Line(Line other)
        {
            if(other != null && other.Direction.Normalize())
            {
                this.Point = new gPoint(other.Point);
                this.Direction = new Vector(other.Direction);
            }
            else
            {
                this.Point = new gPoint();
                this.Direction = new Vector(0, 0, 0);
            }
        }

        public Line(gPoint p1, gPoint p2)
        {
            this.Point = new gPoint(p1);
            this.Direction = new Vector(p2 - p1);
            this.Direction.Normalize();
            this.Direction.z = 0;
        }
        
        public Line(linesegment seg) : this(seg.StartPoint, seg.EndPoint)
        {

        }

        public void Serialize(Serializer serializer)
        {
            int flag = 1;
            serializer.Serialize(flag, "Line.flag");
            serializer.Serialize(this.Point, "Line.Point");
            serializer.Serialize(this.Direction, "Line.Direction");
        }

        public void FromString(string str)
        {
            string[] strArrays = str.Split(new char[] { ';' });
            if ((int)strArrays.Length == 2)
            {
                this.Point.FromString(strArrays[0]);
                this.Direction.FromString(strArrays[1]);
            }
        }

        public bool DeSerialize(DeSerializer deserializer, string fieldname, object value)
        {
            bool result = true;
            switch(fieldname)
            {
                case "Line.flag":
                    int flag = (int)value;
                    break;
                case "LIne.Point":
                    this.Point = (gPoint)value;
                    break;
                case "Line.Direction":
                    this.Direction = (Vector)value;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        public int Intersection(Line line, out gPoint result)
        {
            result = new gPoint(0, 0);
            return Globals.IntersectionLL2D(this.Point, this.ExtendPoint, line.Point, line.ExtendPoint, result);
        }

        public bool PointOnLine(gPoint point, double equality)
        {
            return Geometry.PointOnLine(this.Point, this.ExtendPoint, point, equality);
        }

        public double GetAngle(Line line)
        {
            return this.Direction.GetAngle(line.Direction);
        }

        public void Transformby(Matrix matrix)
        {
            gPoint p1 = new gPoint(Point);
            gPoint p2 = p1 + Direction * 100;
            p1 = matrix.Transform(p1);
            p2 = matrix.Transform(p2);
            this.Point = new gPoint(p1);
            this.Direction = new Vector(p2 - p1);
            bool nor = this.Direction.Normalize();
        }

        public string ToString(Serializer serializer)
        {
            return this.ToString(serializer.DoublePrecisionFormat);
        }

        public string ToString(string format)
        {
            string[] str = new string[] { this.Point.ToString(format), ";", this.Direction.ToString(format) };
            return string.Concat(str);
        }

        public override string ToString()
        {
            return string.Concat(this.Point, "|", this.Direction);
        }
    }
}
