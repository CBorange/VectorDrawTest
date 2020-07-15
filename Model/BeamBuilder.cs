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
using VectorDraw.Professional.vdCommandLine;
using System.Windows.Forms;
using System.Drawing;

namespace MathPractice.Model
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
        }
        public Beam CreateHorBeam()
        {
            return new Beam(new gPoint(-25, 100), document, DEFAULT_BEAM_WIDTH, DEFAULT_BEAM_HEIGHT, Color.Blue,
                Color.Blue, 90, $"verBeam_{beamManager.HorBeams.Count}");
        }
        public Beam CreateVerBeam()
        {
            return new Beam(new gPoint(-25, 100), document, DEFAULT_BEAM_WIDTH, DEFAULT_BEAM_HEIGHT, Color.Blue,
                Color.Blue, 90, $"verBeam_{beamManager.VerBeams.Count}");
        }
    }
}
