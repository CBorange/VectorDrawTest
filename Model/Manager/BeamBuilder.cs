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

            //AddCurtainWallBeam();
            //AddDefaultBeams();
            AddColPointTestBeams();
        }
        private void AddCurtainWallBeam()
        {
            // 테스트 용
            //수직
            gPoint point1 = new gPoint(73098.88097591, 20901.80130121, 0);
            gPoint point2 = new gPoint(73098.88097489, 24221.80129747, 0);
            linesegment seg1 = new linesegment(point1, point2);
            double width1 = 60;

            // 수평
            gPoint point3 = new gPoint(72108.4636168, 20901.80130063, 0);
            gPoint point4 = new gPoint(73161.98241116, 20901.80130125, 0);
            linesegment seg2 = new linesegment(point3, point4);
            double width2 = 60;

            beamManager.AddNewVerBeam(new Beam(point1, point2, document, Color.Blue, seg1.length, width1, "VerBeam_1"));
            beamManager.AddNewHorBeam(new Beam(point3, point4, document, Color.Red, seg2.length, width2, "HorBeam_1"));
        }
        private void AddDefaultBeams()
        {
            double horRot = 90;
            double verRot = 328.7458454;
            beamManager.AddNewHorBeam(CreateHorBeam(new gPoint(Math.Cos(Globals.DegreesToRadians(horRot)) * DEFAULT_BEAM_LENGTH / 2,
                Math.Sin(Globals.DegreesToRadians(horRot)) * DEFAULT_BEAM_LENGTH / 2), horRot));
            beamManager.AddNewVerBeam(CreateVerBeam(new gPoint(Math.Cos(Globals.DegreesToRadians(verRot)) * DEFAULT_BEAM_LENGTH / 2,
                Math.Sin(Globals.DegreesToRadians(verRot)) * DEFAULT_BEAM_LENGTH / 2), verRot));
        }
        private void AddColPointTestBeams()
        {
            // 수직 바 추가
            double ver1Length = CurtainWallMath.GetLengthBy2Point(new gPoint(-200, -200), new gPoint(200, 500));
            beamManager.AddNewVerBeam(new Beam(new gPoint(-200, -200), new gPoint(200, 500), document, Color.Blue, ver1Length, 50, "VerBeam_1"));

            double ver2Length = CurtainWallMath.GetLengthBy2Point(new gPoint(200, -200), new gPoint(200, 500));
            beamManager.AddNewVerBeam(new Beam(new gPoint(200, -200), new gPoint(200, 500), document, Color.Blue, ver2Length, 50, "VerBeam_2"));

            double ver3Length = CurtainWallMath.GetLengthBy2Point(new gPoint(550, -200), new gPoint(550, 500));
            beamManager.AddNewVerBeam(new Beam(new gPoint(550, -200), new gPoint(550, 500), document, Color.Blue, ver3Length, 50, "VerBeam_3"));

            double ver4Length = CurtainWallMath.GetLengthBy2Point(new gPoint(900, -200), new gPoint(900, 500));
            beamManager.AddNewVerBeam(new Beam(new gPoint(900, -200), new gPoint(900, 500), document, Color.Blue, ver4Length, 50, "VerBeam_4"));

            // 수평 바 추가
            Vector ver1S2E = CurtainWallMath.GetUnitVecBy2Point(new gPoint(200, 500), new gPoint(-200, -200));
            double barDis = ver1Length / 3;

            gPoint hor1StartP = CurtainWallMath.GetExtendPoint(new gPoint(-200, -200), ver1S2E * (barDis * 0));
            gPoint hor1EndP = new gPoint(900, hor1StartP.y);
            gPoint hor2StartP = CurtainWallMath.GetExtendPoint(new gPoint(-200, -200), ver1S2E * (barDis * 1));
            gPoint hor2EndP = new gPoint(900, hor2StartP.y);
            gPoint hor3StartP = CurtainWallMath.GetExtendPoint(new gPoint(-200, -200), ver1S2E * (barDis * 2));
            gPoint hor3EndP = new gPoint(900, hor3StartP.y);
            gPoint hor4StartP = CurtainWallMath.GetExtendPoint(new gPoint(-200, -200), ver1S2E * (barDis * 3));
            gPoint hor4EndP = new gPoint(900, hor4StartP.y);

            beamManager.AddNewHorBeam(new Beam(hor1StartP, hor1EndP, document, Color.Red, CurtainWallMath.GetLengthBy2Point(hor1StartP, hor1EndP), 50, "HorBeam_1"));
            beamManager.AddNewHorBeam(new Beam(hor2StartP, hor2EndP, document, Color.Red, CurtainWallMath.GetLengthBy2Point(hor2StartP, hor2EndP), 50, "HorBeam_2"));
            beamManager.AddNewHorBeam(new Beam(hor3StartP, hor3EndP, document, Color.Red, CurtainWallMath.GetLengthBy2Point(hor3StartP, hor3EndP), 50, "HorBeam_3"));
            beamManager.AddNewHorBeam(new Beam(hor4StartP, hor4EndP, document, Color.Red, CurtainWallMath.GetLengthBy2Point(hor4StartP, hor4EndP), 50, "HorBeam_3"));


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
