using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectordrawTest.Model;
using VectorDraw.Geometry;
using VectordrawTest.Model.Manager;
using VectordrawTest.Model.CuttingAlgorithm;
using VectordrawTest.Model.CustomFigure;

namespace VectordrawTest
{
    public partial class Form1 : Form
    {
        // Model
        private VectorDrawConfigure drawConfigure;
        private BeamManager beamManager;
        private BeamCutter beamCutter;
        private BeamBuilder beamBuilder;
        private CollisionLineCalculator colLineCalculator;

        public Form1()
        {
            InitializeComponent();
            
        }
        public void InitializeAllModels(VectorDrawConfigure drawConfigure, BeamManager beamManager,
            BeamCutter beamCutter, BeamBuilder beamBuilder, CollisionLineCalculator colLineCalculator)
        {
            this.drawConfigure = drawConfigure;
            this.beamManager = beamManager;
            this.beamCutter = beamCutter;
            this.beamBuilder = beamBuilder;
            this.colLineCalculator = colLineCalculator;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            drawConfigure.InitializeSystem(vectorDrawBaseControl1.ActiveDocument, vdCommandLine1, beamManager);
            beamCutter.Initialize(vectorDrawBaseControl1.ActiveDocument, beamManager);
            beamManager.Initialize(vectorDrawBaseControl1.ActiveDocument, beamCutter, drawConfigure);
            beamBuilder.Initialize(vectorDrawBaseControl1.ActiveDocument, beamManager);
        }

        private void UpBeam_ForHorizontal(object sender, EventArgs e)
        {
            beamManager.CuttingBeam_HorizontalUp();
        }
        private void UpBeam_ForVertical(object sender, EventArgs e)
        {
            beamManager.CuttingBeam_VerticalUp();
        }
        private void CuttingBeam_Midline(object sender, EventArgs e)
        {
            beamManager.CuttingBeam_MidlineCutting();
        }

        private void CreateHorBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                return;

            Beam newBeam = beamBuilder.CreateHorBeam(point, 0);
            beamManager.AddNewHorBeam(newBeam);
        }

        private void CreateVerBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                return;

            Beam newBeam = beamBuilder.CreateVerBeam(point, 90);
            beamManager.AddNewVerBeam(newBeam);
        }

        private void CalcCollisionLines(object sender, EventArgs e)
        {
            List<linesegment> beamLinesegments = new List<linesegment>();
            for (int i = 0; i < beamManager.HorBeams.Count; ++i)
            {
                linesegment horBeam = new linesegment(beamManager.HorBeams[i].Left, beamManager.HorBeams[i].Right);
                beamLinesegments.Add(horBeam);
            }
            for (int i = 0; i < beamManager.VerBeams.Count; ++i)
            {
                linesegment verBeam = new linesegment(beamManager.VerBeams[i].Left, beamManager.VerBeams[i].Right);
                beamLinesegments.Add(verBeam);
            }
            List<LineCollisionDataSet> colDataSet = colLineCalculator.GetLinesCollisionDataSet(beamLinesegments);
            return;
        }
    }
}
