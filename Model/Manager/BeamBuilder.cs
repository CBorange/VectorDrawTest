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
using VectordrawTest.Model.CustomFigure;
using VectordrawTest.Model.CuttingAlgorithm;

namespace VectordrawTest.Model.Manager
{
    public class BeamBuilder
    {
        // Const
        public const int DEFAULT_BEAM_LENGTH = 300;
        public const int DEFAULT_BEAM_WIDTH = 60;
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

            double horRot = 0;
            double verRot = 135;

            // 테스트 용
            // 수직
            //gPoint point1 = new gPoint(73098.88097591, 20901.80130121, 0);
            //gPoint point2 = new gPoint(73098.88097489, 24221.80129747, 0);
            //linesegment seg1 = new linesegment(point1, point2);
            //double width1 = 60;

            //// 수평
            //gPoint point3 = new gPoint(72108.4636168, 20901.80130063, 0);
            //gPoint point4 = new gPoint(73161.98241116, 20901.80130125, 0);
            //linesegment seg2 = new linesegment(point3, point4);
            //double width2 = 60;

            //beamManager.AddNewVerBeam(new Beam(point1, point2, document, Color.Blue, seg1.length, width1, "VerBeam_1"));
            //beamManager.AddNewHorBeam(new Beam(point3, point4, document, Color.Red, seg2.length, width2, "HorBeam_1"));

            beamManager.AddNewHorBeam(CreateHorBeam(new gPoint(Math.Cos(Globals.DegreesToRadians(horRot)) * DEFAULT_BEAM_LENGTH / 2,
                Math.Sin(Globals.DegreesToRadians(horRot)) * DEFAULT_BEAM_LENGTH / 2), horRot));
            beamManager.AddNewVerBeam(CreateVerBeam(new gPoint(Math.Cos(Globals.DegreesToRadians(verRot)) * DEFAULT_BEAM_LENGTH / 2,
                Math.Sin(Globals.DegreesToRadians(verRot)) * DEFAULT_BEAM_LENGTH / 2), verRot));
        }
        public Beam CreateHorBeam(gPoint newPoint, double rot)
        {
            return new Beam(newPoint, document, Color.Red, DEFAULT_BEAM_LENGTH, DEFAULT_BEAM_WIDTH, rot, $"horBeam_{beamManager.HorBeams.Count}");
        }
        public Beam CreateVerBeam(gPoint newPoint, double rot)
        {
            return new Beam(newPoint, document, Color.Blue, DEFAULT_BEAM_LENGTH, DEFAULT_BEAM_WIDTH, rot, $"verBeam_{beamManager.VerBeams.Count}");
        }
    }
}
