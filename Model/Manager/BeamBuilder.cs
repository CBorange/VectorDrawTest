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

            double horRot = 0.215125215;
            double verRot = 90;

            // 테스트 용
            // 수직
            //gPoint point1 = new gPoint(8367.69335652, 11791.59235794);
            //gPoint point2 = new gPoint(8367.69335652, 17241.59235794);
            //linesegment seg1 = new linesegment(point1, point2);
            //gPoint center1 = new gPoint((point1.x + point2.x) / 2, (point1.y + point2.y) / 2);
            //double length1 = seg1.length;
            //double width1 = 60;

            //// 수평
            //gPoint point3 = new gPoint(8337.69335652, 12591.33126133);
            //gPoint point4 = new gPoint(9787.69335652, 12590.57408118);
            //linesegment seg2 = new linesegment(point3, point4);
            //gPoint center2 = new gPoint((point3.x + point4.x) / 2, (point3.y + point4.y) / 2);
            //double length2 = seg2.length;
            //double width2 = 60;

            //beamManager.AddNewVerBeam(new Beam(center1, document, Color.Red, length1, width1, 90, "VerBeam_1"));
            //beamManager.AddNewHorBeam(new Beam(center2, document, Color.Blue, length2, width2, 0, "HorBeam_1"));

            beamManager.AddNewHorBeam(CreateHorBeam(new gPoint(Math.Cos(Globals.DegreesToRadians(horRot)) * DEFAULT_BEAM_WIDTH / 2,
                Math.Sin(Globals.DegreesToRadians(horRot)) * DEFAULT_BEAM_WIDTH / 2), horRot));
            beamManager.AddNewVerBeam(CreateVerBeam(new gPoint(Math.Cos(Globals.DegreesToRadians(verRot)) * DEFAULT_BEAM_WIDTH / 2,
                Math.Sin(Globals.DegreesToRadians(verRot)) * DEFAULT_BEAM_WIDTH / 2), verRot));
        }
        public Beam CreateHorBeam(gPoint newPoint, double rot)
        {
            return new Beam(newPoint, document, Color.Red, DEFAULT_BEAM_WIDTH, DEFAULT_BEAM_HEIGHT, rot, $"horBeam_{beamManager.HorBeams.Count}");
        }
        public Beam CreateVerBeam(gPoint newPoint, double rot)
        {
            return new Beam(newPoint, document, Color.Blue, DEFAULT_BEAM_WIDTH, DEFAULT_BEAM_HEIGHT, rot, $"verBeam_{beamManager.VerBeams.Count}");
        }
    }
}
