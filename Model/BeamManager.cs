using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spatial;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using System.Drawing;

namespace MathPractice.Model
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

        private BeamCollisionCalculator collisionCalculator;
        private VectorDrawConfigure drawConfigure;

        public BeamManager()
        {

        }
        public void Initialize(vdDocument document, BeamCollisionCalculator collisionCalculator,
            VectorDrawConfigure configure)
        {
            this.document = document;
            this.collisionCalculator = collisionCalculator;
            this.drawConfigure = configure;

            InitBaseLine();
            InitBeams();
            RedrawAllBeam();
        }
        private void InitBeams()
        {
            verBeams = new List<Beam>();
            horBeams = new List<Beam>();

            verBeams.Add(new Beam(new gPoint(-25, 100), document, BeamBuilder.DEFAULT_BEAM_WIDTH, BeamBuilder.DEFAULT_BEAM_HEIGHT, Color.Blue,
                Color.Blue, 90, "verBeam1"));

            verBeams.Add(new Beam(new gPoint(-50, 200), document, BeamBuilder.DEFAULT_BEAM_WIDTH, BeamBuilder.DEFAULT_BEAM_HEIGHT, Color.Blue,
                Color.Blue, 90, "verBeam2"));

            horBeams.Add(new Beam(new gPoint(100, 100), document, BeamBuilder.DEFAULT_BEAM_WIDTH, BeamBuilder.DEFAULT_BEAM_HEIGHT, Color.Red,
                Color.Red, 45, "HorBeam1"));

            for (int i = 0; i < horBeams.Count; ++i)
            {
                collisionCalculator.CollisionCheck(horBeams[i]);
            }
        }
        private void InitBaseLine()
        {
            drawConfigure.AddLineToDocument(new gPoint(-1000, 0), new gPoint(1000, 0));
            drawConfigure.AddLineToDocument(new gPoint(0, -1000), new gPoint(0, 1000));
        }
        private void RedrawAllBeam()
        {
            for (int i = 0; i < verBeams.Count; ++i)
                verBeams[i].DrawBeam();
            for (int i = 0; i < horBeams.Count; ++i)
                horBeams[i].DrawBeam();

            document.Redraw(true);
        }

        // Event Handler
        public void RotateBeam(double degree)
        {
            for (int i = 0; i < horBeams.Count; ++i)
                horBeams[i].RotateBeam(degree);
            for (int i = 0; i < horBeams.Count; ++i)
                collisionCalculator.CollisionCheck(horBeams[i]);
            RedrawAllBeam();
        }
        public void CuttingBeam_HorizontalUp()
        {
            for (int horIDX = 0; horIDX < horBeams.Count; ++horIDX)
            {
                horBeams[horIDX].RemoveAllFigures();
                List<Beam> calcBeams = horBeams[horIDX].CalcTargetBeams;
                for (int verIDX = 0; verIDX < calcBeams.Count; ++verIDX)
                {
                    collisionCalculator.CalcCuttingRect_CrossAlgorithm(calcBeams[verIDX], horBeams[horIDX]);
                }
            }
            RedrawAllBeam();
        }

    }
}
