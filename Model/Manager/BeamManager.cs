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
        private CurtainWallCutting curtainwallCutting;

        public BeamManager()
        {

        }
        public void Initialize(vdDocument document, BeamCutter collisionCalculator,
            VectorDrawConfigure configure)
        {
            this.document = document;
            this.collisionCalculator = collisionCalculator;
            this.drawConfigure = configure;
            curtainwallCutting = new CurtainWallCutting();

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
            for (int verIdx = 0; verIdx < verBeams.Count; ++verIdx)
                verBeams[verIdx].RemoveAllFigures();

            for (int horIDX = 0; horIDX < horBeams.Count; ++horIDX)
            {
                horBeams[horIDX].RemoveAllFigures();
                List<Beam> calcBeams = horBeams[horIDX].CalcTargetBeams;
                for (int verIDX = 0; verIDX < calcBeams.Count; ++verIDX)
                {
                    //collisionCalculator.CalcCuttingRect_CrossAlgorithm(calcBeams[verIDX], horBeams[horIDX]);
                    CuttingResult result = curtainwallCutting.GetCuttingResult(calcBeams[verIDX].Left, calcBeams[verIDX].Right, horBeams[horIDX].Left, horBeams[horIDX].Right,
                        calcBeams[verIDX].BeamHeight, horBeams[horIDX].BeamHeight, calcBeams[verIDX].BeamWidth, horBeams[horIDX].BeamWidth);

                    // Result에 따른 처리
                    if (result.ResultCode != 0) 
                        continue;

                    if (result.HasExtend)
                    {
                        gPoint[] extendPoints = new gPoint[4];
                        if (result.ExtendDir.Equals("LEFT"))
                        {
                            Vector rt2ltUnit = MathSupporter.Instance.GetUnitVecBy2Point(horBeams[horIDX].LeftTop, horBeams[horIDX].RightTop);
                            Vector rb2lbUnit = MathSupporter.Instance.GetUnitVecBy2Point(horBeams[horIDX].LeftBottom, horBeams[horIDX].RightBottom);
                            gPoint extendLT = MathSupporter.Instance.GetExtendPoint(horBeams[horIDX].LeftTop, rt2ltUnit * result.ExtendLength);
                            gPoint extendLB = MathSupporter.Instance.GetExtendPoint(horBeams[horIDX].LeftBottom, rb2lbUnit * result.ExtendLength);

                            extendPoints[0] = new gPoint(horBeams[horIDX].LeftTop);
                            extendPoints[1] = new gPoint(extendLT);
                            extendPoints[2] = new gPoint(extendLB);
                            extendPoints[3] = new gPoint(horBeams[horIDX].LeftBottom);
                        }
                        else
                        {
                            Vector lt2rtUnit = MathSupporter.Instance.GetUnitVecBy2Point(horBeams[horIDX].RightTop, horBeams[horIDX].LeftTop);
                            Vector lb2rbUnit = MathSupporter.Instance.GetUnitVecBy2Point(horBeams[horIDX].RightBottom, horBeams[horIDX].LeftBottom);
                            gPoint extendRT = MathSupporter.Instance.GetExtendPoint(horBeams[horIDX].RightTop, lt2rtUnit * result.ExtendLength);
                            gPoint extendRB = MathSupporter.Instance.GetExtendPoint(horBeams[horIDX].RightBottom, lb2rbUnit * result.ExtendLength);

                            extendPoints[0] = new gPoint(horBeams[horIDX].RightTop);
                            extendPoints[1] = new gPoint(extendRT);
                            extendPoints[2] = new gPoint(extendRB);
                            extendPoints[3] = new gPoint(horBeams[horIDX].RightBottom);
                        }
                        horBeams[horIDX].AddExtendFigure(extendPoints, Color.Green, 0.5f);
                    }
                    List<gPoint> cuttingPoints = new List<gPoint>(2);
                    cuttingPoints.Add(result.FirstCutPoint);
                    cuttingPoints.Add(result.SecondCutPoint);
                    horBeams[horIDX].AddCuttingFigure(cuttingPoints, Color.Yellow, 0.75f);
                }
            }
            RefreshAllBeam();
        }
        public void CuttingBeam_HorizontalUp()
        {
            for (int horIDX = 0; horIDX < horBeams.Count; ++horIDX)
                horBeams[horIDX].RemoveAllFigures();
            for (int verIdx = 0; verIdx < verBeams.Count; ++verIdx)
            {
                verBeams[verIdx].RemoveAllFigures();
                List<Beam> calcBeams = verBeams[verIdx].CalcTargetBeams;
                for (int horIdx = 0; horIdx < calcBeams.Count; ++horIdx)
                {
                    //collisionCalculator.CalcCuttingRect_CrossAlgorithm(calcBeams[horIdx], verBeams[verIdx]);
                    CuttingResult result = curtainwallCutting.GetCuttingResult(calcBeams[horIdx].Left, calcBeams[horIdx].Right, verBeams[verIdx].Left, verBeams[verIdx].Right,
                        calcBeams[horIdx].BeamHeight, verBeams[verIdx].BeamHeight, calcBeams[horIdx].BeamWidth, verBeams[verIdx].BeamWidth);
                    // Result에 따른 처리
                    if (result.ResultCode != 0)
                        continue;

                    if (result.HasExtend)
                    {
                        gPoint[] extendPoints = new gPoint[4];
                        if (result.ExtendDir.Equals("LEFT"))
                        {
                            Vector rt2ltUnit = MathSupporter.Instance.GetUnitVecBy2Point(verBeams[verIdx].LeftTop, verBeams[verIdx].RightTop);
                            Vector rb2lbUnit = MathSupporter.Instance.GetUnitVecBy2Point(verBeams[verIdx].LeftBottom, verBeams[verIdx].RightBottom);
                            gPoint extendLT = MathSupporter.Instance.GetExtendPoint(verBeams[verIdx].LeftTop, rt2ltUnit * result.ExtendLength);
                            gPoint extendLB = MathSupporter.Instance.GetExtendPoint(verBeams[verIdx].LeftBottom, rb2lbUnit * result.ExtendLength);

                            extendPoints[0] = new gPoint(verBeams[verIdx].LeftTop);
                            extendPoints[1] = new gPoint(extendLT);
                            extendPoints[2] = new gPoint(extendLB);
                            extendPoints[3] = new gPoint(verBeams[verIdx].LeftBottom);
                        }
                        else
                        {
                            Vector lt2rtUnit = MathSupporter.Instance.GetUnitVecBy2Point(verBeams[verIdx].RightTop, verBeams[verIdx].LeftTop);
                            Vector lb2rbUnit = MathSupporter.Instance.GetUnitVecBy2Point(verBeams[verIdx].RightBottom, verBeams[verIdx].LeftBottom);
                            gPoint extendRT = MathSupporter.Instance.GetExtendPoint(verBeams[verIdx].RightTop, lt2rtUnit * result.ExtendLength);
                            gPoint extendRB = MathSupporter.Instance.GetExtendPoint(verBeams[verIdx].RightBottom, lb2rbUnit * result.ExtendLength);

                            extendPoints[0] = new gPoint(verBeams[verIdx].RightTop);
                            extendPoints[1] = new gPoint(extendRT);
                            extendPoints[2] = new gPoint(extendRB);
                            extendPoints[3] = new gPoint(verBeams[verIdx].RightBottom);
                        }
                        verBeams[verIdx].AddExtendFigure(extendPoints, Color.Green, 0.5f);
                    }
                    List<gPoint> cuttingPoints = new List<gPoint>(2);
                    cuttingPoints.Add(result.FirstCutPoint);
                    cuttingPoints.Add(result.SecondCutPoint);
                    verBeams[verIdx].AddCuttingFigure(cuttingPoints, Color.Yellow, 0.75f);
                }
            }
            RefreshAllBeam();
        }
    }
}
