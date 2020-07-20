using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using System.Drawing;
using VectorDraw.Professional.PropertyList;
using MathPractice.Model.Manager;
using MathPractice.Model.CustomFigure;
using System.Diagnostics;

namespace MathPractice.Model.CollisionCalculator
{
    public class CrossAlgorithm : ICollisionAlgorithm
    {
        private MathSupporter math;
        public CrossAlgorithm()
        {
            math = MathSupporter.Instance;
        }
        public void CalcAlgorithm_CuttingRect(Beam verBeam, Beam horBeam)
        {
            bool needExpand = false;

            // 확장 여부 검사
            int colPointCount = 0;
            if (math.Point2BeamCollision(horBeam.LeftTop, verBeam))
                colPointCount += 1;
            if (math.Point2BeamCollision(horBeam.LeftBottom, verBeam))
                colPointCount += 1;

            if (colPointCount > 0)
                needExpand = true;

            // 충돌 위치 산출
            gPoint ltExpand = math.GetExpandedPointBy2Points(horBeam.RightTop, horBeam.LeftTop, 1000);
            gPoint lbExpand = math.GetExpandedPointBy2Points(horBeam.RightBottom, horBeam.LeftBottom, 1000);
            gPoint rtExpand = math.GetExpandedPointBy2Points(horBeam.LeftTop, horBeam.RightTop, 1000);
            gPoint rbExpand = math.GetExpandedPointBy2Points(horBeam.LeftBottom, horBeam.RightBottom, 1000);

            List<gPoint> calcRectPoints = new List<gPoint>();
            GetCollisionPoints(rtExpand, ltExpand, verBeam, calcRectPoints);
            GetCollisionPoints(rbExpand, lbExpand, verBeam, calcRectPoints);
            if (calcRectPoints.Count < 2)
            {
                GetCollisionPoints(ltExpand, rtExpand, verBeam, calcRectPoints);
                GetCollisionPoints(lbExpand, rbExpand, verBeam, calcRectPoints);
            }
            calcRectPoints = calcRectPoints.OrderBy(x => x.x).ToList();

            horBeam.AddCuttingFigure(calcRectPoints);
        }
        private void GetCollisionPoints(gPoint calcLineStart, gPoint calcLineEnd, Beam targetBeam, List<gPoint> rectPointList)
        {
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, targetBeam.LeftTop, targetBeam.RightTop))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, targetBeam.LeftTop, targetBeam.RightTop));
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, targetBeam.LeftBottom, targetBeam.RightBottom))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, targetBeam.LeftBottom, targetBeam.RightBottom));
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, targetBeam.LeftTop, targetBeam.LeftBottom))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, targetBeam.LeftTop, targetBeam.LeftBottom));
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, targetBeam.RightTop, targetBeam.RightBottom))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, targetBeam.RightTop, targetBeam.RightBottom));
        }
    }
}
