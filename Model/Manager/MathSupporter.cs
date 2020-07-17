using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spatial;
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
        public Point TransformToLeftHand(Point2 point)
        {
            Matrix2 transformMat = new Matrix2(1, 0, 0, -1, 0, 0);
            Point2 newPoint = Matrix2.TransformPoint(transformMat, point);
            Point2 transformPoint = new Point2(ClientWidth * 0.5f, ClientHeight * 0.5f);
            Point2 resultPoint = newPoint + transformPoint;

            return new Point((int)resultPoint.X, (int)resultPoint.Y);
        }
        public gPoint GetCrossPoint(gPoint pointA, gPoint pointB, gPoint pointC, gPoint pointD)
        {
            Vector2 lineA2C = new Vector2(pointC.x - pointA.x, pointC.y - pointA.y);
            Vector2 lineA2B = new Vector2(pointB.x - pointA.x, pointB.y - pointA.y);
            Vector2 lineA2B_Unit = lineA2B.Normalize();

            double a2eLength = lineA2C.Dot(lineA2B_Unit);
            Vector2 lineA2E = lineA2B_Unit * a2eLength;
            gPoint pointE = new gPoint(pointA.x + lineA2E.X, pointA.y + lineA2E.Y);

            Vector2 lineC2E = new Vector2(pointE.x - pointC.x, pointE.y - pointC.y);
            Vector2 lineC2E_Unit = lineC2E.Normalize();
            Vector2 lineC2D = new Vector2(pointD.x - pointC.x, pointD.y - pointC.y);
            double c2fLength = lineC2D.Dot(lineC2E_Unit);

            Vector2 lineC2F = lineC2E_Unit * c2fLength;
            gPoint pointF = new gPoint(pointC.x + lineC2F.X, pointC.y + lineC2F.Y);

            double c2eRatio = lineC2E.Length() / c2fLength;

            double crossPointLength = lineC2D.Length() * c2eRatio;
            Vector2 lineC2D_Unit = lineC2D.Normalize();
            Vector2 lineC2CrossPoint = lineC2D_Unit * crossPointLength;

            gPoint crossPoint = new gPoint(pointC.x + lineC2CrossPoint.X, pointC.y + lineC2CrossPoint.Y);
            return crossPoint;
        }
        public double GetLengthBy2Point(gPoint pointA, gPoint pointB)
        {
            Vector2 vec = new Vector2(pointA.x - pointB.x, pointA.y - pointB.y);
            return vec.Length();
        }
        public Vector2 GetVectorBy2Point(gPoint target, gPoint origin)
        {
            return new Vector2(target.x - origin.x, target.y - origin.y);
        }
        public Vector2 GetUnitVecBy2Point(gPoint target, gPoint origin)
        {
            return GetVectorBy2Point(target, origin).Normalize();
        }
        public gPoint GetExpandPoint(gPoint origin, Vector2 expandVec)
        {
            return new gPoint(origin.x + expandVec.X, origin.y + expandVec.Y);
        }
        public gPoint GetExpandedPointBy2Points(gPoint startP, gPoint endP, int expandValue)
        {
            Vector2 vec = GetVectorBy2Point(startP, endP);
            vec = vec.Normalize();
            vec *= expandValue;
            gPoint expanded = GetExpandPoint(startP, vec);
            return expanded;
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
            if (rot >= 90 && rot <270)
                rot -= 180;
            if (rot >= 270 && rot < 360)
                rot -= 360;

            double[,] mat = GetRotationMat(rot);
            gPoint result = new gPoint();
            result.x = ((mat[0, 0] * (point.x - center.x)) + (mat[0, 1] * (point.y - center.y)) + center.x);
            result.y = ((mat[1, 0] * (point.x - center.x)) + (mat[1, 1] * (point.y - center.y)) + center.y);
            return result;
        }
        public Vector2 GetRotatedVector(double rot, gPoint center, gPoint unitPoint)
        {
            double[,] mat = GetRotationMat(rot);
            Vector2 result = new Vector2(((mat[0, 0] * (unitPoint.x - center.x)) + (mat[0, 1] * (unitPoint.y - center.y)) + center.x),
                ((mat[1, 0] * (unitPoint.x - center.x)) + (mat[1, 1] * (unitPoint.y - center.y)) + center.y));

            return result;
        }
        public double GetShetaBy2Points(gPoint center, gPoint target)
        {
            gPoint diffrent = center - target;
            return Globals.RadiansToDegrees(Math.Atan2(diffrent.y, diffrent.x));
        }
        public bool OBBCollision(Beam beamA, Beam beamB)
        {
            Vector2 disVec = GetVectorBy2Point(beamA.Center, beamB.Center);
            Vector2[] seperateVec =
            {
                GetVectorBy2Point(beamA.Top,beamA.Center),
                GetVectorBy2Point(beamA.Right,beamA.Center),
                GetVectorBy2Point(beamB.Top,beamB.Center),
                GetVectorBy2Point(beamB.Right,beamB.Center)
            };
            Vector2 seperateUnit;
            for (int seperateIDX = 0; seperateIDX < 4; ++seperateIDX)
            {
                double sum = 0;
                seperateUnit = seperateVec[seperateIDX].Normalize();
                for (int vecIDx = 0; vecIDx < 4; ++vecIDx)
                    sum += Math.Abs(seperateVec[vecIDx].Dot(seperateUnit));

                if (Math.Abs(disVec.Dot(seperateUnit)) > sum)
                    return false;
            }
            return true;
        }
        //public bool Rect2PointCollision(Beam rect, gPoint point)
        //{
        //    // Calc Rotated Point Based On Rect Rotation
        //     GetRotatedPoint(-rect.Rotation, point, rect.Center);
        //}
    }
}
