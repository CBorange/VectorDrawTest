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
using MathPractice.Model.CustomFigure;
using MathPractice.Model.CuttingAlgorithm;

namespace MathPractice.Model.Manager
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

        public BeamManager()
        {

        }
        public void Initialize(vdDocument document, BeamCutter collisionCalculator,
            VectorDrawConfigure configure)
        {
            this.document = document;
            this.collisionCalculator = collisionCalculator;
            this.drawConfigure = configure;

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
                    collisionCalculator.CalcCuttingRect_CrossAlgorithm(calcBeams[verIDX], horBeams[horIDX]);
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
                    collisionCalculator.CalcCuttingRect_CrossAlgorithm(calcBeams[horIdx], verBeams[verIdx]);
                }
            }
            RefreshAllBeam();
        }
    }
}
