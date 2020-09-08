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
using Hicom.BizDraw.Geometry;

namespace VectordrawTest.Model.CuttingAlgorithm
{
    public class UpCuttingResult
    {
        public UpCuttingResult()
        {
            CutBar_CutAngle = 0;
            CutBar_ExtendPoint_First = null;
            CutBar_ExtendPoint_Second = null;
            CutBar_CutPoint_First = null;
            CutBar_CutPoint_Second = null;

            UpBar_CutAngle = 0;
            UpBar_ExtendPoint_First = null;
            UpBar_ExtendPoint_Second = null;
            UpBar_CutPoint_First = null;
            UpBar_CutPoint_Second = null;
        }
        public double CutBar_CutAngle;
        public gPoint CutBar_ExtendPoint_First;
        public gPoint CutBar_ExtendPoint_Second;
        public gPoint CutBar_CutPoint_First;
        public gPoint CutBar_CutPoint_Second;

        public double UpBar_CutAngle;
        public gPoint UpBar_ExtendPoint_First;
        public gPoint UpBar_ExtendPoint_Second;
        public gPoint UpBar_CutPoint_First;
        public gPoint UpBar_CutPoint_Second;

        public int ResultCode;
    }
    public class UpCuttingAlgorithm
    {
        private CurtainWallMath math;
        private UpCuttingResult result;
        private Bar upBar;
        private Bar cutBar;

        // 계산용 확장 지점
        private gPoint cut_RT2LT_Ext;
        private gPoint cut_RB2LB_Ext;
        private gPoint cut_LT2RT_Ext;
        private gPoint cut_LB2RB_Ext;

        private gPoint up_RT2LT_Ext;
        private gPoint up_RB2LB_Ext;
        private gPoint up_LT2RT_Ext;
        private gPoint up_LB2RB_Ext;

        private List<gPoint> entireColPoints;
        private List<PointAndDis> upBar_ColPoints;
        private List<PointAndDis> cutBar_ColPoints;

        public UpCuttingAlgorithm()
        {
            math = new CurtainWallMath();
        }
        public UpCuttingResult GetCuttingResult(gPoint upBar_StartPoint, gPoint upBar_EndPoint, gPoint cutBar_StartPoint, gPoint cutBar_EndPoint,
            double upBarWidth, double cutBarWidth, double upBarLength, double cutBarLength)
        {
            result = new UpCuttingResult();

            upBar = CuttingUtil.CreateBar(upBar_StartPoint, upBar_EndPoint, upBarWidth, upBarLength);
            cutBar = CuttingUtil.CreateBar(cutBar_StartPoint, cutBar_EndPoint, cutBarWidth, cutBarLength);
            entireColPoints = new List<gPoint>();

            // 검사 용 각 꼭짓점 확장 지점 생성
            cut_RT2LT_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RT, cutBar.LT, 1000000);
            cut_RB2LB_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RB, cutBar.LB, 1000000);
            cut_LT2RT_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LT, cutBar.RT, 1000000);
            cut_LB2RB_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LB, cutBar.RB, 1000000);

            up_RT2LT_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.RT, upBar.LT, 1000000);
            up_RB2LB_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.RB, upBar.LB, 1000000);
            up_LT2RT_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.LT, upBar.RT, 1000000);
            up_LB2RB_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.LB, upBar.RB, 1000000);

            CuttingProcess();

            // CutBar 계산
            double cutBar_Left2CutPointsLength = GetLengthByColPoints(cutBar.Left, entireColPoints);
            double cutBar_Right2CutPointsLength = GetLengthByColPoints(cutBar.Right, entireColPoints);

            gPoint cutBarCenter = cutBar.Left;
            if (cutBar_Right2CutPointsLength > cutBar_Left2CutPointsLength)
                cutBarCenter = cutBar.Right;

            cutBar_ColPoints = GetPADByBarCenter(cutBarCenter, entireColPoints);
            result.CutBar_CutPoint_First = cutBar_ColPoints[0].Point;
            result.CutBar_CutPoint_Second = cutBar_ColPoints[1].Point;
            CalcCutBar_ExtendPoints();

            result.CutBar_CutAngle = GetCuttingAngle(result.CutBar_CutPoint_Second, result.CutBar_CutPoint_First, cutBar);

            // UpBar 계산
            double upBar_Left2CutPointsLength = GetLengthByColPoints(upBar.Left, entireColPoints);
            double upBar_Right2CutPointsLength = GetLengthByColPoints(upBar.Right, entireColPoints);

            gPoint upBarCenter = upBar.Left;
            if (upBar_Right2CutPointsLength > upBar_Left2CutPointsLength)
                upBarCenter = upBar.Right;

            upBar_ColPoints = GetPADByBarCenter(upBarCenter, entireColPoints);
            result.UpBar_CutPoint_First = upBar_ColPoints[upBar_ColPoints.Count - 2].Point;
            result.UpBar_CutPoint_Second = upBar_ColPoints[upBar_ColPoints.Count - 1].Point;
            CalcUpBar_ExtendPoints(upBar_ColPoints);

            result.UpBar_CutAngle = GetCuttingAngle(result.UpBar_CutPoint_Second, result.UpBar_CutPoint_First, upBar);

            return result;
        }
        public UpCuttingResult GetCuttingResult(linesegment upBarSegment, linesegment cutBarSegment, double upBarWidth, double cutBarWidth)
        {
            result = new UpCuttingResult();

            upBar = CuttingUtil.CreateBar(upBarSegment.StartPoint, upBarSegment.EndPoint, upBarWidth, upBarSegment.length);
            cutBar = CuttingUtil.CreateBar(cutBarSegment.StartPoint, cutBarSegment.EndPoint, cutBarWidth, cutBarSegment.length);
            entireColPoints = new List<gPoint>();

            // 검사 용 각 꼭짓점 확장 지점 생성
            cut_RT2LT_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RT, cutBar.LT, 1000000);
            cut_RB2LB_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.RB, cutBar.LB, 1000000);
            cut_LT2RT_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LT, cutBar.RT, 1000000);
            cut_LB2RB_Ext = CurtainWallMath.GetExtendedPointBy2Points(cutBar.LB, cutBar.RB, 1000000);

            up_RT2LT_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.RT, upBar.LT, 1000000);
            up_RB2LB_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.RB, upBar.LB, 1000000);
            up_LT2RT_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.LT, upBar.RT, 1000000);
            up_LB2RB_Ext = CurtainWallMath.GetExtendedPointBy2Points(upBar.LB, upBar.RB, 1000000);

            entireColPoints.Clear();
            CuttingProcess();

            // CutBar 계산
            double cutBar_Left2CutPointsLength = GetLengthByColPoints(cutBar.Left, entireColPoints);
            double cutBar_Right2CutPointsLength = GetLengthByColPoints(cutBar.Right, entireColPoints);

            gPoint cutBarCenter = cutBar.Left;
            if (cutBar_Right2CutPointsLength > cutBar_Left2CutPointsLength)
                cutBarCenter = cutBar.Right;

            cutBar_ColPoints = GetPADByBarCenter(cutBarCenter, entireColPoints);
            result.CutBar_CutPoint_First = cutBar_ColPoints[0].Point;
            result.CutBar_CutPoint_Second = cutBar_ColPoints[1].Point;
            CalcCutBar_ExtendPoints();

            result.CutBar_CutAngle = GetCuttingAngle(result.CutBar_CutPoint_Second, result.CutBar_CutPoint_First, cutBar);

            // UpBar 계산
            double upBar_Left2CutPointsLength = GetLengthByColPoints(upBar.Left, entireColPoints);
            double upBar_Right2CutPointsLength = GetLengthByColPoints(upBar.Right, entireColPoints);

            gPoint upBarCenter = upBar.Left;
            if (upBar_Right2CutPointsLength > upBar_Left2CutPointsLength)
                upBarCenter = upBar.Right;

            upBar_ColPoints = GetPADByBarCenter(upBarCenter, entireColPoints);
            result.UpBar_CutPoint_First = upBar_ColPoints[upBar_ColPoints.Count - 2].Point;
            result.UpBar_CutPoint_Second = upBar_ColPoints[upBar_ColPoints.Count - 1].Point;
            CalcUpBar_ExtendPoints(upBar_ColPoints);

            result.UpBar_CutAngle = GetCuttingAngle(result.UpBar_CutPoint_Second, result.UpBar_CutPoint_First, upBar);

            return result;
        }

        // 충돌 검사
        private void CuttingProcess()
        {
            CalcCollisionPoint(up_LT2RT_Ext, up_RT2LT_Ext, cut_LT2RT_Ext, cut_RT2LT_Ext, entireColPoints);
            CalcCollisionPoint(up_LT2RT_Ext, up_RT2LT_Ext, cut_LB2RB_Ext, cut_RB2LB_Ext, entireColPoints);
            CalcCollisionPoint(up_LB2RB_Ext, up_RB2LB_Ext, cut_LT2RT_Ext, cut_RT2LT_Ext, entireColPoints);
            CalcCollisionPoint(up_LB2RB_Ext, up_RB2LB_Ext, cut_LB2RB_Ext, cut_RB2LB_Ext, entireColPoints);
        }

        private void CalcUpBar_ExtendPoints(List<PointAndDis> colPoints)
        {
            gPoint originPoint = null;
            gPoint sidePoint = null;
            gPoint fartestPoint = colPoints[colPoints.Count - 1].Point;
            gPoint secondPoint = colPoints[colPoints.Count - 2].Point;

            CuttingUtil.GetBarVertexSymmetryPoint(fartestPoint, upBar, out originPoint, out sidePoint);
            double origin2FartestCutLength = CurtainWallMath.GetLengthBy2Point(fartestPoint, originPoint);
            double extendLength = origin2FartestCutLength - upBar.Length;

            // 확장 길이가 0보다 작으면 확장이 필요 없음
            if (extendLength < 0)
                return;

            Vector origin2FartestCutVec = CurtainWallMath.GetUnitVecBy2Point(fartestPoint,originPoint);
            origin2FartestCutVec *= origin2FartestCutLength;
            Vector side2SecondCutVec = CurtainWallMath.GetUnitVecBy2Point(secondPoint, sidePoint);
            side2SecondCutVec *= origin2FartestCutLength;

            result.UpBar_ExtendPoint_Second = CurtainWallMath.GetExtendPoint(originPoint, origin2FartestCutVec);
            result.UpBar_ExtendPoint_First = CurtainWallMath.GetExtendPoint(sidePoint, side2SecondCutVec);
        }
        private void CalcCutBar_ExtendPoints()
        {
            gPoint originPoint = null;
            gPoint sidePoint = null;
            CuttingUtil.GetBarVertexSymmetryPoint(result.CutBar_CutPoint_Second, cutBar, out originPoint, out sidePoint);
            double origin2SecondCutLength = CurtainWallMath.GetLengthBy2Point(result.CutBar_CutPoint_Second, originPoint);
            double extendLength = origin2SecondCutLength - cutBar.Length;

            // 확장 길이가 0보다 작으면 확장이 필요 없음
            if (extendLength < 0)
                return;

            Vector origin2SecondCutVec = CurtainWallMath.GetUnitVecBy2Point(result.CutBar_CutPoint_Second, originPoint);
            origin2SecondCutVec *= origin2SecondCutLength;
            Vector side2FirstCutVec = CurtainWallMath.GetUnitVecBy2Point(result.CutBar_CutPoint_First, sidePoint);
            side2FirstCutVec *= origin2SecondCutLength;

            result.CutBar_ExtendPoint_Second = CurtainWallMath.GetExtendPoint(originPoint, origin2SecondCutVec);
            result.CutBar_ExtendPoint_First = CurtainWallMath.GetExtendPoint(sidePoint, side2FirstCutVec);
        }
        #region Common Function

        private double GetLengthByColPoints(gPoint center, List<gPoint> colPoints)
        {
            double result = 0;

            for (int i = 0; i < colPoints.Count; ++i)
                result += CurtainWallMath.GetLengthBy2Point(center, colPoints[i]);
            return result;
        }
        private List<PointAndDis> GetPADByBarCenter(gPoint barCenter, List<gPoint> unManufacturedPoints)
        {
            List<PointAndDis> colPoints = new List<PointAndDis>();
            for (int i = 0; i < unManufacturedPoints.Count; ++i)
            {
                PointAndDis pointAndDis = new PointAndDis();
                pointAndDis.Point = unManufacturedPoints[i];
                pointAndDis.Distance = CurtainWallMath.GetLengthBy2Point(barCenter, unManufacturedPoints[i]);
                colPoints.Add(pointAndDis);
            }
            colPoints = CuttingUtil.DistinctPAD_Points(colPoints);
            colPoints = colPoints.OrderBy(dis => dis.Distance).ToList();

            if (colPoints.Count <= 1)
                return null;
            return colPoints;
        }
        private double GetCuttingAngle(gPoint secondCutPoint, gPoint firstCutPoint, Bar targetBar)
        {
            gPoint farSymmetryPoint = CuttingUtil.GetBarVertexSymmetryPoint(secondCutPoint, targetBar);

            Vector cutVec = CurtainWallMath.GetUnitVecBy2Point(firstCutPoint, secondCutPoint);
            Vector symmetryVec = CurtainWallMath.GetUnitVecBy2Point(farSymmetryPoint, secondCutPoint);
            double cutAngle = cutVec.Dot(symmetryVec);
            cutAngle = Math.Acos(cutAngle);
            cutAngle = Globals.RadiansToDegrees(cutAngle);
            return cutAngle;
        }
        private void CalcCollisionPoint(gPoint upBeam_SPoint, gPoint upBeam_EPoint, gPoint cutBeam_SPoint, gPoint cutBeam_EPoint, List<gPoint> currentCutPoints)
        {
            linesegment upBeamLine = new linesegment(upBeam_SPoint, upBeam_EPoint);
            linesegment cutBeamLine = new linesegment(cutBeam_SPoint, cutBeam_EPoint);
            gPoint colPoint = new gPoint();
            if (Geometry.Intersection(upBeamLine, cutBeamLine, colPoint) == 1)
                currentCutPoints.Add(colPoint);
        }
        #endregion
    }
   
}
