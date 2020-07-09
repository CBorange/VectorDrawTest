using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using Spatial;
using System.Diagnostics;

namespace MathPractice
{
    public class MathSupporter
    {
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
    }
}
