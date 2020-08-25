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

namespace MathPractice.Model.CuttingAlgorithm
{

    public class UpCutting_Cross : IUpCuttingAlgorithm
    {
        private MathSupporter math;
        private Beam upBeam;
        private Beam cuttedBeam;

        private gPoint ltExtend;
        private gPoint lbExtend;
        private gPoint rtExtend;
        private gPoint rbExtend;
        private List<gPoint> entireColPoints;
        private bool needExtend;
        public UpCutting_Cross()
        {
            math = MathSupporter.Instance;
        }

        // Ver / Hor : Lagacy Parameter
        public CuttingResult CalcAlgorithm_CuttingRect(Beam upBeam, Beam cuttedBeam)
        {
            // Init
            CuttingResult result = new CuttingResult();

            this.cuttedBeam = cuttedBeam;
            this.upBeam = upBeam;

            // 확장 여부 검사
            needExtend = false;

            // 확장 여부 검사
            int colPointCount = 0;
            if (math.Point2BeamCollision(cuttedBeam.LeftTop, upBeam))
                colPointCount += 1;
            if (math.Point2BeamCollision(cuttedBeam.LeftBottom, upBeam))
                colPointCount += 1;
            if (math.Point2BeamCollision(cuttedBeam.RightTop, upBeam))
                colPointCount += 1;
            if (math.Point2BeamCollision(cuttedBeam.RightBottom, upBeam))
                colPointCount += 1;

            if (colPointCount == 1)
                needExtend = true;

            // 충돌 위치 산출
            ltExtend = math.GetExpandedPointBy2Points(cuttedBeam.RightTop, cuttedBeam.LeftTop, 1000);
            lbExtend = math.GetExpandedPointBy2Points(cuttedBeam.RightBottom, cuttedBeam.LeftBottom, 1000);
            rtExtend = math.GetExpandedPointBy2Points(cuttedBeam.LeftTop, cuttedBeam.RightTop, 1000);
            rbExtend = math.GetExpandedPointBy2Points(cuttedBeam.LeftBottom, cuttedBeam.RightBottom, 1000);

            entireColPoints = new List<gPoint>();
            GetCollisionPoints();
            // 확장 길이 산출
            if (needExtend)
            {
                gPoint farthestColPoint = math.GetFarthestPoint(cuttedBeam.Center, entireColPoints.ToArray());
                double rot = cuttedBeam.Rotation * -1;
                farthestColPoint = math.GetRotatedPoint(rot, farthestColPoint, cuttedBeam.Center);

                // Center 기준 상단에 가장 먼 충돌지점(이하 충돌지점) 존재
                if (farthestColPoint.y > cuttedBeam.Center.y)
                {
                    rot *= -1;
                    farthestColPoint = math.GetRotatedPoint(rot, farthestColPoint, cuttedBeam.Center);
                    // Center 기준 충돌지점의 X값으로 좌측 또는 우측 분리하여 계산
                    if (farthestColPoint.x <= cuttedBeam.Center.x)
                        result.LeftExtendLength = math.GetLengthBy2Point(cuttedBeam.RightTop, farthestColPoint) - cuttedBeam.BeamWidth;
                    else if (farthestColPoint.x > cuttedBeam.Center.x)
                        result.LeftExtendLength = math.GetLengthBy2Point(cuttedBeam.LeftTop, farthestColPoint) - cuttedBeam.BeamWidth;
                }
                // 반대로 계산
                else if (farthestColPoint.y < cuttedBeam.Center.y)
                {
                    rot *= -1;
                    farthestColPoint = math.GetRotatedPoint(rot, farthestColPoint, cuttedBeam.Center);
                    if (farthestColPoint.x <= cuttedBeam.Center.x)
                        result.RightExtendLength = math.GetLengthBy2Point(cuttedBeam.RightBottom, farthestColPoint) - cuttedBeam.BeamWidth;
                    else if (farthestColPoint.x > cuttedBeam.Center.x)
                        result.RightExtendLength = math.GetLengthBy2Point(cuttedBeam.LeftBottom, farthestColPoint) - cuttedBeam.BeamWidth;
                }
            }
            cuttedBeam.AddCuttingFigure(entireColPoints, Color.Azure);
            return result;
        }
        private void GetCollisionPoints()
        {
            if (math.GetLineIsCross(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.RightTop, ltExtend))
                entireColPoints.Add(math.GetCrossPoint(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.RightTop, ltExtend));
            if (math.GetLineIsCross(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.RightBottom, lbExtend))
                entireColPoints.Add(math.GetCrossPoint(upBeam.LeftTop, upBeam.RightTop, cuttedBeam.RightBottom, lbExtend));
            if (entireColPoints.Count == 2 && needExtend == true) return;
            if (math.GetLineIsCross(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.LeftTop, rtExtend))
                entireColPoints.Add(math.GetCrossPoint(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.LeftTop, rtExtend));
            if (math.GetLineIsCross(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.LeftBottom, rbExtend))
                entireColPoints.Add(math.GetCrossPoint(upBeam.LeftBottom, upBeam.RightBottom, cuttedBeam.LeftBottom, rbExtend));
        }

    }
}
