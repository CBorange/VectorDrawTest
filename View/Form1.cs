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
using System.Diagnostics;

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

        // Beam
        private Beam selectBeam;

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
            selectBeam = null;
        }

        private void UpBeam_ForHorizontal(object sender, EventArgs e)
        {
            beamManager.CuttingBeam_HorizontalUp();
            beamManager.RefreshAllBeam();
        }
        private void UpBeam_ForVertical(object sender, EventArgs e)
        {
            beamManager.CuttingBeam_VerticalUp();
            beamManager.RefreshAllBeam();
        }
        private void CuttingBeam_Midline(object sender, EventArgs e)
        {
            beamManager.CuttingBeam_MidlineCutting();
            beamManager.RefreshAllBeam();
        }

        private void CreateHorBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                return;

            Beam newBeam = beamBuilder.CreateHorBeam(point, 0);
            beamManager.AddNewHorBeam(newBeam);
            beamManager.RefreshAllBeam();
        }

        private void CreateVerBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                return;

            Beam newBeam = beamBuilder.CreateVerBeam(point, 90);
            beamManager.AddNewVerBeam(newBeam);
            beamManager.RefreshAllBeam();
        }

        private void CalcCollisionLines(object sender, EventArgs e)
        {
            // Make Input Parameter
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

            // Modify Output Data
            int colDataIdx = 0;
            for (int i = 0; i < beamManager.HorBeams.Count; ++i)
            {
                beamManager.HorBeams[i].AttachCollisionData(colDataSet[colDataIdx]);
                colDataIdx += 1;
            }
            for (int i = 0; i < beamManager.VerBeams.Count; ++i)
            {
                beamManager.VerBeams[i].AttachCollisionData(colDataSet[colDataIdx]);
                colDataIdx += 1;
            }
            MessageBox.Show("Collision Calculate Complete");
        }

        private void CheckBeamInfo(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                return;

            Beam foundBeam = beamManager.FoundBeam(point, true);

            // Modify Control
            BarName_Text.Text = foundBeam.BeamName;
            Bar_StartPoint_Text.Text = $"{string.Format("{0:f5}", foundBeam.Left.x)}, {string.Format("{0:f5}", foundBeam.Left.y)}";
            Bar_EndPoint_Text.Text = $"{string.Format("{0:f5}", foundBeam.Right.x)}, {string.Format("{0:f5}", foundBeam.Right.y)}";
            Bar_Rotation_Text.Text = $"{string.Format("{0:f5}", foundBeam.Rotation)}";
            Bar_Width_Text.Text = $"{string.Format("{0:f5}", foundBeam.BeamWidth)}";
            Bar_Length_Text.Text = $"{string.Format("{0:f5}", foundBeam.BeamLength)}";

            Bar_LT_Text.Text = $"{string.Format("{0:f5}", foundBeam.LeftTop.x)}, {string.Format("{0:f5}", foundBeam.LeftTop.y)}";
            Bar_RT_Text.Text = $"{string.Format("{0:f5}", foundBeam.RightTop.x)}, {string.Format("{0:f5}", foundBeam.RightTop.y)}";
            Bar_LB_Text.Text = $"{string.Format("{0:f5}", foundBeam.LeftBottom.x)}, {string.Format("{0:f5}", foundBeam.LeftBottom.y)}";
            Bar_RB_Text.Text = $"{string.Format("{0:f5}", foundBeam.RightBottom.x)}, {string.Format("{0:f5}", foundBeam.RightBottom.y)}";
            Bar_Left_Text.Text = $"{string.Format("{0:f5}", foundBeam.Left.x)}, {string.Format("{0:f5}", foundBeam.Left.y)}";
            Bar_Right_Text.Text = $"{string.Format("{0:f5}", foundBeam.Right.x)}, {string.Format("{0:f5}", foundBeam.Right.y)}";
            Bar_Center_Text.Text = $"{string.Format("{0:f5}", foundBeam.Center.x)}, {string.Format("{0:f5}", foundBeam.Center.y)}";

            // Modify Collision Info
            if (foundBeam.CollisionData == null) return;

            Bar_Collision_TreeView.Nodes.Clear();
            Bar_Collision_TreeView.Nodes.Add("CurrentLine", $"현재 라인 : {string.Format("{0:f5}", foundBeam.CollisionData.CurrentLine.StartPoint)}," +
                $"{ string.Format("{0:f5}", foundBeam.CollisionData.CurrentLine.EndPoint)}");
            Bar_Collision_TreeView.Nodes.Add("CollisionList", "충돌 라인 리스트");


            var collisionTree = Bar_Collision_TreeView.Nodes["CollisionList"];
            for (int i = 0; i < foundBeam.CollisionData.CollisionList.Count; ++i)
            {
                CollisionInfo info = foundBeam.CollisionData.CollisionList[i];
                collisionTree.Nodes.Add($"Collision_{i}", $"충돌 정보_{i}");

                var colInfoNode = collisionTree.Nodes[$"Collision_{i}"].Nodes;
                colInfoNode.Add($"ColPoint", $"충돌 위치 : {string.Format("{0:f5}", info.CollisionPoint.x)}, {string.Format("{0:f5}", info.CollisionPoint.y)}");
                colInfoNode.Add($"ColidedLine", $"충돌한 라인 : {string.Format("{0:f5}", info.CollidedLine.StartPoint)}, {string.Format("{0:f5}", info.CollidedLine.EndPoint)}");
                colInfoNode.Add($"Angle", $"충돌 각도 : {string.Format("{0:f5}", info.Angle)}");
            }
        }
    }
}
