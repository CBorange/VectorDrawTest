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

                gPoint calcLineA_End = math.GetExpandedPointBy2Points(horBeam.RightTop, horBeam.LeftTop, 5);
                gPoint calcLineB_End = math.GetExpandedPointBy2Points(horBeam.RightBottom, horBeam.LeftBottom, 5);
            }
        }
    }
}
