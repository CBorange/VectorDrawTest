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
            // 우측
            if (horBeam.Center.x > verBeam.Center.x)
            {
                // 확장 여부 검사
                int colPointCount = 0;
                if (math.Point2BeamCollision(horBeam.LeftTop, verBeam))
                    colPointCount += 1;
                if (math.Point2BeamCollision(horBeam.LeftBottom, verBeam))
                    colPointCount += 1;

                if (colPointCount > 0)
                    needExpand = true;

                // 충돌 위치 산출
                gPoint calcLineA_End = math.GetExpandedPointBy2Points(horBeam.RightTop, horBeam.LeftTop, 1000);
                VectorDrawConfigure.Instance.AddCircleToDocument(horBeam.RightTop, 3);
                VectorDrawConfigure.Instance.AddCircleToDocument(calcLineA_End, 3);
                gPoint calcLineB_End = math.GetExpandedPointBy2Points(horBeam.RightBottom, horBeam.LeftBottom, 1000);
                VectorDrawConfigure.Instance.AddCircleToDocument(horBeam.RightBottom, 3);
                VectorDrawConfigure.Instance.AddCircleToDocument(calcLineB_End, 3);

                List<gPoint> calcRectPoints = new List<gPoint>();
                GetCollisionPoints(horBeam.RightTop, calcLineA_End, verBeam, calcRectPoints);
                GetCollisionPoints(horBeam.RightBottom, calcLineB_End, verBeam, calcRectPoints);

                horBeam.AddCuttingFigure(calcRectPoints);
            }
        }
        private void GetCollisionPoints(gPoint calcLineStart, gPoint calcLineEnd, Beam baseBeam, List<gPoint> rectPointList)
        {
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, baseBeam.LeftTop, baseBeam.RightTop))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, baseBeam.LeftTop, baseBeam.RightTop));
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, baseBeam.LeftBottom, baseBeam.RightBottom))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, baseBeam.LeftBottom, baseBeam.RightBottom));
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, baseBeam.LeftTop, baseBeam.LeftBottom))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, baseBeam.LeftTop, baseBeam.LeftBottom));
            if (math.GetLineIsCross(calcLineStart, calcLineEnd, baseBeam.RightTop, baseBeam.RightBottom))
                rectPointList.Add(math.GetCrossPoint(calcLineStart, calcLineEnd, baseBeam.RightTop, baseBeam.RightBottom));
        }
    }
}
