using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spatial;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using System.Drawing;
using MathPractice.Model.Manager;
using MathPractice.Model.CustomFigure;

namespace MathPractice.Model.CollisionCalculator
{
    public class RotationAlgorithm : ICollisionAlgorithm
    {
        private MathSupporter math;
        public RotationAlgorithm()
        {
            math = MathSupporter.Instance;
        }
        public void CalcAlgorithm_CuttingRect(Beam verBeam, Beam horBeam)
        {
            //Triangle miniTriangle = new Triangle();
            //Triangle bigTriangle = new Triangle();

            //// miniTriangle
            //double center2ASheta = Globals.DegreesToRadians(180 - (horizontalBar.Rotation + 90));
            //double Height = (horizontalBar.BeamHeight * 0.5f) * Math.Tan(center2ASheta);
            //miniTriangle.PointA = new gPoint(verticalBar.Center.x - horizontalBar.BeamHeight * 0.5f, verticalBar.Center.y);
            //miniTriangle.PointA.y = miniTriangle.PointA.y + Height;

            //Vector2 center2PointA_Unit = new Vector2(miniTriangle.PointA.x - horizontalBar.Left.x,
            //    miniTriangle.PointA.y - horizontalBar.Left.y).Normalize();
            //miniTriangle.PointB = new gPoint(center2PointA_Unit.X * horizontalBar.BeamHeight * 0.5f,
            //    center2PointA_Unit.Y * horizontalBar.BeamHeight * 0.5f);

            //double a2bLenghth = new Vector2(miniTriangle.PointA.x - miniTriangle.PointB.x,
            //    miniTriangle.PointA.y - miniTriangle.PointB.y).Length();

            //double shetaC = Globals.DegreesToRadians(180 - (horizontalBar.Rotation + 90));
            //double b2cLength = a2bLenghth / Math.Tan(shetaC);

            //Vector2 r2lUnit = new Vector2(horizontalBar.Left.x - horizontalBar.Right.x, 
            //    horizontalBar.Left.y - horizontalBar.Right.y).Normalize();
            //Vector2 rt2PointC = (b2cLength + horizontalBar.BeamWidth) * r2lUnit;
            //miniTriangle.PointC = new gPoint(horizontalBar.RightTop.x + rt2PointC.X,
            //    horizontalBar.RightTop.y + rt2PointC.Y);

            //// bigTriangle
            //double bigHeight = a2bLenghth + horizontalBar.BeamHeight;
            //double rb2PointCLength = bigHeight / Math.Tan(shetaC);
            //Vector2 rb2PointC = (horizontalBar.BeamWidth + rb2PointCLength) * r2lUnit;
            //bigTriangle.PointC = new gPoint(horizontalBar.RightBottom.x + rb2PointC.X,
            //    horizontalBar.RightBottom.y + rb2PointC.Y);

            //AddCircleToDocument(miniTriangle.PointC);
            //AddCircleToDocument(bigTriangle.PointC);
        }
    }
}
