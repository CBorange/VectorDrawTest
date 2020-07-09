using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spatial;

namespace MathPractice
{
    public class Beam
    {
        private const int CALIB_ROTCOORD = 90;
        private int beamWidth;
        public int BeamWidth
        {
            get { return beamWidth; }
        }
        private int beamHeight;
        public int BeamHeight
        {
            get { return beamHeight; }
        }

        // Rect Point
        private int rotation;
        public int Rotation
        {
            get { return rotation + 90; }
        }
        private int disCenterToVertex;
        public int DisCenterToVertex
        {
            get { return disCenterToVertex; }
        }
        private Point2 leftTop;
        public Point2 LeftTop { get { return leftTop; } }

        private Point2 rightTop;
        public Point2 RightTop { get { return rightTop; } }

        private Point2 leftBottom;
        public Point2 LeftBottom { get { return leftBottom; } }

        private Point2 rightBottom;
        public Point2 RightBottom { get { return rightBottom; } }

        private Point2 bottom;
        public Point2 Bottom { get { return bottom; } }

        private Point2 top;
        public Point2 Top { get { return top; } }

        private Point2 center;
        public Point2 Center { get { return center; } }

        public Color DrawColor;
        public Beam(Point2 centerPos, int width, int height, Color color,int rot)
        {
            beamWidth = width;
            beamHeight = height;
            center = centerPos;
            rotation = rot - CALIB_ROTCOORD;
            DrawColor = color;

            CalcRectData();
        }
        private void CalcRectData()
        {
            int halfWidth = (int)(beamWidth * 0.5f);
            int halfHeight = (int)(beamHeight * 0.5f);

            leftTop = new Point2(center.X - halfWidth, center.Y + halfHeight);
            rightTop = new Point2(center.X + halfWidth, center.Y + halfHeight);
            rightBottom = new Point2(center.X + halfWidth, center.Y - halfHeight);
            leftBottom = new Point2(center.X - halfWidth, center.Y - halfHeight);
            top = new Point2(center.X, center.Y + halfHeight);
            bottom = new Point2(center.X, center.Y - halfHeight);

            double[,] rotMatrix = new double[2, 2];
            rotMatrix[0, 0] = Math.Cos(rotation * Vector2.DegreesToRadians);
            rotMatrix[0, 1] = -Math.Sin(rotation * Vector2.DegreesToRadians);
            rotMatrix[1,0] = Math.Sin(rotation * Vector2.DegreesToRadians);
            rotMatrix[1,1] = Math.Cos(rotation * Vector2.DegreesToRadians);

            leftTop = leftTop.RotateAt(center, rotation);
            rightTop = rightTop.RotateAt(center, rotation);
            rightBottom = rightBottom.RotateAt(center, rotation);
            leftBottom = leftBottom.RotateAt(center, rotation);
            top = top.RotateAt(center, rotation);
            bottom = bottom.RotateAt(center, rotation);
        }
        public void DrawBeam(Graphics g)
        {
            Pen pen = new Pen(DrawColor, 2);
            g.DrawLine(pen, MathSupporter.Instance.TransformToLeftHand(leftTop),
                MathSupporter.Instance.TransformToLeftHand(rightTop));
            g.DrawLine(pen, MathSupporter.Instance.TransformToLeftHand(rightTop),
                MathSupporter.Instance.TransformToLeftHand(rightBottom));
            g.DrawLine(pen, MathSupporter.Instance.TransformToLeftHand(rightBottom),
                MathSupporter.Instance.TransformToLeftHand(leftBottom));
            g.DrawLine(pen, MathSupporter.Instance.TransformToLeftHand(leftBottom),
                MathSupporter.Instance.TransformToLeftHand(leftTop));

            Pen innerLinePen = new Pen(Color.AliceBlue, 2);
            innerLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawLine(innerLinePen, MathSupporter.Instance.TransformToLeftHand(bottom), MathSupporter.Instance.TransformToLeftHand(top));
        }
        public void SetPosition(Point2 newPos)
        {
            center = newPos;
            CalcRectData();
        }
        public void Translate(Point2 delta)
        {
            center += delta;
            CalcRectData();
        }
        public void RotateBeam(int degreeAngle)
        {
            rotation += degreeAngle;
            CalcRectData();
        }
        public void SetRotation(int degreeAngle)
        {
            rotation = degreeAngle - CALIB_ROTCOORD;
            CalcRectData();
        }
    }
}
