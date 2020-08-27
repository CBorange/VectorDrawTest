using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdCommandLine;
using System.Windows.Forms;
using System.Drawing;
using MathPractice.Model.CustomFigure;

namespace MathPractice.Model.Manager
{
    public class BeamBuilder
    {
        // Const
        public const int DEFAULT_BEAM_WIDTH = 200;
        public const int DEFAULT_BEAM_HEIGHT = 50;
        private vdDocument document;
        public vdDocument Document { get; }

        private BeamManager beamManager;
        public BeamBuilder()
        {
        }

        public void Initialize(vdDocument document, BeamManager beamManager)
        {
            this.document = document;
            this.beamManager = beamManager;

            beamManager.AddNewHorBeam(CreateHorBeam(new gPoint(100, 0)));
            beamManager.AddNewVerBeam(CreateVerBeam(new gPoint(0, 0)));
        }
        public Beam CreateHorBeam(gPoint newPoint)
        {
            return new Beam(newPoint, document, Color.Red, DEFAULT_BEAM_WIDTH, DEFAULT_BEAM_HEIGHT, 90, $"horBeam_{beamManager.HorBeams.Count}");
        }
        public Beam CreateVerBeam(gPoint newPoint)
        {
            return new Beam(newPoint, document, Color.Blue, DEFAULT_BEAM_WIDTH, DEFAULT_BEAM_HEIGHT, 0, $"verBeam_{beamManager.VerBeams.Count}");
        }
    }
}
