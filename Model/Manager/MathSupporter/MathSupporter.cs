using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using MathPractice.Model.CustomFigure;

namespace MathPractice.Model.Manager
{
    public class MathSupporter
    {
        private MathSupporter() { }
        private static MathSupporter instance;
        public static MathSupporter Instance
        {
            get
            {
                if (instance == null)
                    instance = new MathSupporter();
                return instance;
            }
        }

        public int ClientWidth;
        public int ClientHeight;

        public double DegreeToRadian(double angle)
        {
            return angle * (Math.PI / 180.0f);
        }
        public double RadianToDegree(double angle)
        {
            return angle * (180.0f / Math.PI);
        }
        public void SetClientRect(int width, int height)
        {
            ClientWidth = width;
            ClientHeight = height;
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
        public gPoint GetExpandPoint(gPoint origin, Vector expandVec)
        {
            return new gPoint(origin.x + expandVec.x, origin.y + expandVec.y);
        }
        public gPoint GetExpandedPointBy2Points(gPoint startP, gPoint endP, int expandValue)
        {
            Vector vec = GetVectorBy2Point(endP, startP);
            vec.Normalize();
            vec *= expandValue;
            gPoint expanded = GetExpandPoint(startP, vec);
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
        public double GetShetaBy2Points(gPoint center, gPoint target)
        {
            gPoint diffrent = center - target;
            return Globals.RadiansToDegrees(Math.Atan2(diffrent.y, diffrent.x));
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
        //public double GetAngleByCenterLine(gPoint center, gPoint a1, gPoint a2)
        //{
        //    gPoint centerLeft = new gPoint(center.x - 1000.0f, center.y);
        //    gPoint centerRight = new gPoint(center.x + 1000.0f, center.y);

        //}
        //public bool Rect2PointCollision(Beam rect, gPoint point)
        //{
        //    // Calc Rotated Point Based On Rect Rotation
        //     GetRotatedPoint(-rect.Rotation, point, rect.Center);
        //}
    }
}
