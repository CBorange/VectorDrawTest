using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using System.Drawing;
using VectorDraw.Professional.PropertyList;
using VectordrawTest.Model.Manager;
using VectordrawTest.Model.CustomFigure;
using System.Diagnostics;

namespace VectordrawTest.Model.CuttingAlgorithm
{
    // 충돌 정보
    public class CollisionInfo
    {
        public CollisionInfo()
        {
            CollisionPoint = null;
            CollidedLine = null;
            Angle = 0;
        }
        public gPoint CollisionPoint;
        public linesegment CollidedLine;
        public double Angle;
    }

    // Line 한개의 충돌정보 집합
    public class LineCollisionDataSet
    {
        public LineCollisionDataSet()
        {
            CurrentLine = null;
            CollisionList = new List<CollisionInfo>();
        }
        public LineCollisionDataSet(linesegment currentLine)
        {
            this.CurrentLine = currentLine;
            CollisionList = new List<CollisionInfo>();
        }
        public linesegment CurrentLine;
        public List<CollisionInfo> CollisionList;
    }
    public class CollisionLineCalculator
    {
        public List<LineCollisionDataSet> GetLinesCollisionDataSet(List<linesegment> baseLines)
        {
            List<LineCollisionDataSet> collisionLines = new List<LineCollisionDataSet>(baseLines.Count);

            for (int checkIdx = 0; checkIdx < baseLines.Count; ++checkIdx)
            {
                LineCollisionDataSet colDataSet = new LineCollisionDataSet(baseLines[checkIdx]);
                for (int lineIdx = 0; lineIdx < baseLines.Count; ++lineIdx)
                {
                    if (checkIdx == lineIdx) continue;
                    if (CurtainWallMath.GetLineIsCross(baseLines[checkIdx].StartPoint, baseLines[checkIdx].EndPoint,
                        baseLines[lineIdx].StartPoint, baseLines[lineIdx].EndPoint))
                    {
                        CollisionInfo colInfo = new CollisionInfo();
                        // 충돌 점 산출
                        colInfo.CollisionPoint = CurtainWallMath.GetCrossPoint(baseLines[checkIdx].StartPoint, baseLines[checkIdx].EndPoint,
                        baseLines[lineIdx].StartPoint, baseLines[lineIdx].EndPoint);

                        // 충돌 선분 저장
                        colInfo.CollidedLine = baseLines[lineIdx];

                        // 충돌 각도 산출
                        colInfo.Angle = CalculateAngle(baseLines[checkIdx].StartPoint, baseLines[checkIdx].EndPoint,
                            baseLines[lineIdx].StartPoint, baseLines[lineIdx].EndPoint);

                        colDataSet.CollisionList.Add(colInfo);
                    }
                }
                collisionLines.Add(colDataSet);
            }
            return collisionLines;
        }
        private double CalculateAngle(gPoint lineA_Start, gPoint lineA_End, gPoint lineB_Start, gPoint lineB_End)
        {
            Vector lineA_S2E = CurtainWallMath.GetUnitVecBy2Point(lineA_End, lineA_Start);
            Vector lineB_S2E = CurtainWallMath.GetUnitVecBy2Point(lineB_End, lineB_Start);
            double angle = lineA_S2E.Dot(lineB_S2E);
            angle = Math.Acos(angle);
            angle = Globals.RadiansToDegrees(angle);
            return angle;
        }
    }
}
