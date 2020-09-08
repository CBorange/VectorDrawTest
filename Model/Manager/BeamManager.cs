using System;
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

        // Cutting Methods
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
                        calcBeams[verIDX].BeamWidth, horBeams[horIDX].BeamWidth, calcBeams[verIDX].BeamLength, horBeams[horIDX].BeamLength);

                    if (result.ResultCode != 0)
                        continue;

                    CreateUpCuttingFigure(result, horBeams[horIDX], calcBeams[verIDX]);
                    CreateUpCuttingExtendFigure(result.UpBar_ExtendPoint_First, result.UpBar_ExtendPoint_Second, calcBeams[verIDX]);
                    CreateUpCuttingExtendFigure(result.CutBar_ExtendPoint_First, result.CutBar_ExtendPoint_Second, horBeams[horIDX]);
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
                        calcBeams[horIdx].BeamWidth, verBeams[verIdx].BeamWidth, calcBeams[horIdx].BeamLength, verBeams[verIdx].BeamLength);

                    if (result.ResultCode != 0)
                        continue;

                    CreateUpCuttingFigure(result, verBeams[verIdx], calcBeams[horIdx]);
                    CreateUpCuttingExtendFigure(result.UpBar_ExtendPoint_First, result.UpBar_ExtendPoint_Second, calcBeams[horIdx]);
                    CreateUpCuttingExtendFigure(result.CutBar_ExtendPoint_First, result.CutBar_ExtendPoint_Second, verBeams[verIdx]);
                }
            }
            RefreshAllBeam();
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
                    MidlineCuttingResult result = midLineCutting.GetCuttingResult(horBeams[horIDX].Left, horBeams[horIDX].Right, horBeams[horIDX].BeamWidth, horBeams[horIDX].BeamLength,
                        calcBeams[verIDX].Left, calcBeams[verIDX].Right, calcBeams[verIDX].BeamWidth, calcBeams[verIDX].BeamLength);
                    CreateMidlineCuttingFigure(result, horBeams[horIDX], calcBeams[verIDX]);
                }
            }
            RefreshAllBeam();
        }

        // Cutting Figure(CutPoint, ExtendPoint based Graphics Object Create)
        private void CreateUpCuttingFigure(UpCuttingResult result, Beam cutBar, Beam upBar)
        {
            List<gPoint> cutBarCuttingPoints = new List<gPoint>(2);
            cutBarCuttingPoints.Add(result.CutBar_CutPoint_First);
            cutBarCuttingPoints.Add(result.CutBar_CutPoint_Second);
            cutBar.AddCuttingFigure(cutBarCuttingPoints, Color.Yellow, 0.75f);

            List<gPoint> upBarCuttingPoints = new List<gPoint>(2);
            upBarCuttingPoints.Add(result.UpBar_CutPoint_First);
            upBarCuttingPoints.Add(result.UpBar_CutPoint_Second);
            upBar.AddCuttingFigure(upBarCuttingPoints, Color.Magenta, 0.75f);
        }
        private void CreateUpCuttingExtendFigure(gPoint firstExtendPoint, gPoint secondExtendPoint, Beam targetBeam)
        {
            if ((firstExtendPoint != null) && (secondExtendPoint != null)) 
            {
                gPoint[] extendPoints = new gPoint[2];
                extendPoints[0] = firstExtendPoint;
                extendPoints[1] = secondExtendPoint;
                targetBeam.AddExtendFigure(extendPoints, Color.Green, 0.5f);
            }
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
    }
}
