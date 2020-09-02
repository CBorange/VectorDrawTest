﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Render;
using System.Drawing;
using VectordrawTest.Model.CustomFigure;
using VectordrawTest.Model.CuttingAlgorithm;

namespace VectordrawTest.Model.Manager
{
    public class BeamManager
    {
        // Variable
        private vdDocument document;
        public vdDocument Document 
        {
            get { return document; }
        }

        private List<Beam> verBeams;
        public List<Beam> VerBeams
        {
            get { return verBeams; }
        }

        private List<Beam> horBeams;
        public List<Beam> HorBeams
        {
            get { return horBeams; }
        }

        private BeamCutter collisionCalculator;
        private VectorDrawConfigure drawConfigure;
        private UpCuttingAlgorithm upCutting;
        private MidlineCuttingAlgorithm midLineCutting;

        public BeamManager()
        {

        }
        public void Initialize(vdDocument document, BeamCutter collisionCalculator,
            VectorDrawConfigure configure)
        {
            this.document = document;
            this.collisionCalculator = collisionCalculator;
            this.drawConfigure = configure;
            upCutting = new UpCuttingAlgorithm();
            midLineCutting = new MidlineCuttingAlgorithm();

            InitBaseLine();
            InitBeams();
        }
        private void InitBeams()
        {
            verBeams = new List<Beam>();
            horBeams = new List<Beam>();
        }
        private void InitBaseLine()
        {
            drawConfigure.AddLineToDocument(new gPoint(-1000, 0), new gPoint(1000, 0));
            drawConfigure.AddLineToDocument(new gPoint(0, -1000), new gPoint(0, 1000));
        }
        

        // Event Handler
        public void DrawOutLineFromAllBeam(vdRender render)
        {
            for (int i = 0; i < verBeams.Count; ++i)
            {
                verBeams[i].DrawOutLines(render);
            }
            for (int i = 0; i < horBeams.Count; ++i)
            {
                horBeams[i].DrawOutLines(render);
            }
        }
        public void RefreshAllBeam()
        {
            for (int i = 0; i < verBeams.Count; ++i)
            {
                verBeams[i].UpdateBaseLine();
                collisionCalculator.CheckCollisionVerToHor(verBeams[i]);
            }
            for (int i = 0; i < horBeams.Count; ++i)
            {
                horBeams[i].UpdateBaseLine();
                collisionCalculator.CheckCollisionHorToVer(horBeams[i]);
            }
                
            document.Redraw(true);
        }
        public void AddNewHorBeam(Beam beam)
        {
            horBeams.Add(beam);
            RefreshAllBeam();
        }
        public void AddNewVerBeam(Beam beam)
        {
            verBeams.Add(beam);
            RefreshAllBeam();
        }
        public void CuttingBeam_VerticalUp()
        {
            // Figure Refresh
            for (int verIdx = 0; verIdx < verBeams.Count; ++verIdx)
                verBeams[verIdx].RemoveAllFigures();

            // Cutting
            for (int horIDX = 0; horIDX < horBeams.Count; ++horIDX)
            {
                horBeams[horIDX].RemoveAllFigures();
                List<Beam> calcBeams = horBeams[horIDX].CalcTargetBeams;
                for (int verIDX = 0; verIDX < calcBeams.Count; ++verIDX)
                {
                    UpCuttingResult result = upCutting.GetCuttingResult(calcBeams[verIDX].Left, calcBeams[verIDX].Right, horBeams[horIDX].Left, horBeams[horIDX].Right,
                        calcBeams[verIDX].BeamHeight, horBeams[horIDX].BeamHeight, calcBeams[verIDX].BeamWidth, horBeams[horIDX].BeamWidth);
                    CreateUpCuttingFigure(result, horBeams[horIDX]);
                }
            }
            RefreshAllBeam();
        }
        public void CuttingBeam_HorizontalUp()
        {
            // Figure Refresh
            for (int horIDX = 0; horIDX < horBeams.Count; ++horIDX)
                horBeams[horIDX].RemoveAllFigures();

            // Cutting
            for (int verIdx = 0; verIdx < verBeams.Count; ++verIdx)
            {
                verBeams[verIdx].RemoveAllFigures();
                List<Beam> calcBeams = verBeams[verIdx].CalcTargetBeams;
                for (int horIdx = 0; horIdx < calcBeams.Count; ++horIdx)
                {
                    UpCuttingResult result = upCutting.GetCuttingResult(calcBeams[horIdx].Left, calcBeams[horIdx].Right, verBeams[verIdx].Left, verBeams[verIdx].Right,
                        calcBeams[horIdx].BeamHeight, verBeams[verIdx].BeamHeight, calcBeams[horIdx].BeamWidth, verBeams[verIdx].BeamWidth);
                    CreateUpCuttingFigure(result, verBeams[verIdx]);
                }
            }
            RefreshAllBeam();
        }
        private void CreateUpCuttingFigure(UpCuttingResult cuttingResult, Beam targetBeam)
        {
            // Result에 따른 처리
            if (cuttingResult.ResultCode != 0)
                return;

            if (cuttingResult.HasExtend)
            {
                gPoint[] extendPoints = new gPoint[4];
                if (cuttingResult.ExtendDir.Equals("LEFT"))
                {
                    Vector rt2ltUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.LeftTop, targetBeam.RightTop);
                    Vector rb2lbUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.LeftBottom, targetBeam.RightBottom);
                    gPoint extendLT = MathSupporter.Instance.GetExtendPoint(targetBeam.LeftTop, rt2ltUnit * cuttingResult.ExtendLength);
                    gPoint extendLB = MathSupporter.Instance.GetExtendPoint(targetBeam.LeftBottom, rb2lbUnit * cuttingResult.ExtendLength);

                    extendPoints[0] = new gPoint(targetBeam.LeftTop);
                    extendPoints[1] = new gPoint(extendLT);
                    extendPoints[2] = new gPoint(extendLB);
                    extendPoints[3] = new gPoint(targetBeam.LeftBottom);
                }
                else
                {
                    Vector lt2rtUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.RightTop, targetBeam.LeftTop);
                    Vector lb2rbUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.RightBottom, targetBeam.LeftBottom);
                    gPoint extendRT = MathSupporter.Instance.GetExtendPoint(targetBeam.RightTop, lt2rtUnit * cuttingResult.ExtendLength);
                    gPoint extendRB = MathSupporter.Instance.GetExtendPoint(targetBeam.RightBottom, lb2rbUnit * cuttingResult.ExtendLength);

                    extendPoints[0] = new gPoint(targetBeam.RightTop);
                    extendPoints[1] = new gPoint(extendRT);
                    extendPoints[2] = new gPoint(extendRB);
                    extendPoints[3] = new gPoint(targetBeam.RightBottom);
                }
                targetBeam.AddExtendFigure(extendPoints, Color.Green, 0.5f);
            }
            List<gPoint> cuttingPoints = new List<gPoint>(2);
            cuttingPoints.Add(cuttingResult.FirstCutPoint);
            cuttingPoints.Add(cuttingResult.SecondCutPoint);
            targetBeam.AddCuttingFigure(cuttingPoints, Color.Yellow, 0.75f);
        }
        private void CreateMidlineExtendFigure(MidlineCuttingResult result, Beam targetBeam, string barType)
        {
            if (result.ResultCode != 0)
                return;

            gPoint[] extendPoints = new gPoint[4];

            if (barType.Equals("A"))    // Bar A
            {
                
                if (result.BarA_ExtendDir.Equals("LEFT"))
                {
                    Vector rt2ltUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.LeftTop, targetBeam.RightTop);
                    Vector rb2lbUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.LeftBottom, targetBeam.RightBottom);
                    gPoint extendLT = MathSupporter.Instance.GetExtendPoint(targetBeam.LeftTop, rt2ltUnit * result.BarA_ExtendLength);
                    gPoint extendLB = MathSupporter.Instance.GetExtendPoint(targetBeam.LeftBottom, rb2lbUnit * result.BarA_ExtendLength);

                    extendPoints[0] = new gPoint(targetBeam.LeftTop);
                    extendPoints[1] = new gPoint(extendLT);
                    extendPoints[2] = new gPoint(extendLB);
                    extendPoints[3] = new gPoint(targetBeam.LeftBottom);
                }
                else
                {
                    Vector lt2rtUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.RightTop, targetBeam.LeftTop);
                    Vector lb2rbUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.RightBottom, targetBeam.LeftBottom);
                    gPoint extendRT = MathSupporter.Instance.GetExtendPoint(targetBeam.RightTop, lt2rtUnit * result.BarA_ExtendLength);
                    gPoint extendRB = MathSupporter.Instance.GetExtendPoint(targetBeam.RightBottom, lb2rbUnit * result.BarA_ExtendLength);

                    extendPoints[0] = new gPoint(targetBeam.RightTop);
                    extendPoints[1] = new gPoint(extendRT);
                    extendPoints[2] = new gPoint(extendRB);
                    extendPoints[3] = new gPoint(targetBeam.RightBottom);
                }
            }
            else  // Bar B
            {
                if (result.BarB_ExtendDir.Equals("LEFT"))
                {
                    Vector rt2ltUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.LeftTop, targetBeam.RightTop);
                    Vector rb2lbUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.LeftBottom, targetBeam.RightBottom);
                    gPoint extendLT = MathSupporter.Instance.GetExtendPoint(targetBeam.LeftTop, rt2ltUnit * result.BarB_ExtendLength);
                    gPoint extendLB = MathSupporter.Instance.GetExtendPoint(targetBeam.LeftBottom, rb2lbUnit * result.BarB_ExtendLength);

                    extendPoints[0] = new gPoint(targetBeam.LeftTop);
                    extendPoints[1] = new gPoint(extendLT);
                    extendPoints[2] = new gPoint(extendLB);
                    extendPoints[3] = new gPoint(targetBeam.LeftBottom);
                }
                else
                {
                    Vector lt2rtUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.RightTop, targetBeam.LeftTop);
                    Vector lb2rbUnit = MathSupporter.Instance.GetUnitVecBy2Point(targetBeam.RightBottom, targetBeam.LeftBottom);
                    gPoint extendRT = MathSupporter.Instance.GetExtendPoint(targetBeam.RightTop, lt2rtUnit * result.BarB_ExtendLength);
                    gPoint extendRB = MathSupporter.Instance.GetExtendPoint(targetBeam.RightBottom, lb2rbUnit * result.BarB_ExtendLength);

                    extendPoints[0] = new gPoint(targetBeam.RightTop);
                    extendPoints[1] = new gPoint(extendRT);
                    extendPoints[2] = new gPoint(extendRB);
                    extendPoints[3] = new gPoint(targetBeam.RightBottom);
                }
            }
            targetBeam.AddExtendFigure(extendPoints, Color.Green, 0.5f);
        }
        private void CreateMidlineCuttingFigure(MidlineCuttingResult result, Beam beamA, Beam beamB)
        {
            if (result.ResultCode != 0)
                return;

            CreateMidlineExtendFigure(result, beamA, "A");
            CreateMidlineExtendFigure(result, beamB, "B");


            // Cutting Figure
            List<gPoint> cuttingPoints = new List<gPoint>(2);
            cuttingPoints.Add(result.FirstCutPoint);
            cuttingPoints.Add(result.SecondCutPoint);
            beamA.AddCuttingFigure(cuttingPoints, Color.Yellow, 0.75f);
        }
        public void CuttingBeam_MidlineCutting()
        {
            // Figure Refresh
            for (int verIdx = 0; verIdx < verBeams.Count; ++verIdx)
                verBeams[verIdx].RemoveAllFigures();
            for (int horIDX = 0; horIDX < horBeams.Count; ++horIDX)
                horBeams[horIDX].RemoveAllFigures();

            // Cutting
            for (int horIDX = 0; horIDX < horBeams.Count; ++horIDX)
            {
                List<Beam> calcBeams = horBeams[horIDX].CalcTargetBeams;
                for (int verIDX = 0; verIDX < calcBeams.Count; ++verIDX)
                {
                    MidlineCuttingResult result = midLineCutting.GetCuttingResult(horBeams[horIDX].Left, horBeams[horIDX].Right, horBeams[horIDX].BeamHeight, horBeams[horIDX].BeamWidth,
                        calcBeams[verIDX].Left, calcBeams[verIDX].Right, calcBeams[verIDX].BeamHeight, calcBeams[verIDX].BeamWidth);
                    CreateMidlineCuttingFigure(result, horBeams[horIDX], calcBeams[verIDX]);
                }
            }
            RefreshAllBeam();
        }
    }
}
