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
        public const int DEFAULT_BEAM_LENGTH = 200;
        public const int DEFAULT_BEAM_WIDTH = 50;
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

            double horRot = -23.215125;
            double verRot = 89;

            // 테스트 용
            //// 수직
            //gPoint point1 = new gPoint(40913.32670452, 13377.83499133, 0);
            //gPoint point2 = new gPoint(40913.32670453, 17411.85172475, 0);
            //linesegment seg1 = new linesegment(point1, point2);
            //double width1 = 60;

            //// 수평
            //gPoint point3 = new gPoint(36347.85030018, 15938.10177558, 0);
            //gPoint point4 = new gPoint(40927.85030017, 13404.08504218, 0);
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
