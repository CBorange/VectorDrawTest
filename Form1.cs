using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectordrawTest.View;
using VectordrawTest.Controller;
using VectordrawTest.Model;
using VectordrawTest.Model.Manager;
using VectordrawTest.Model.CuttingAlgorithm;

namespace VectordrawTest
{
    public partial class Form1 : Form, IMainView
    {
        // Controller
        private MainController mainController;

        // Model
        private VectorDrawConfigure drawConfigure;
        private BeamManager beamManager;
        private BeamCutter beamCutter;
        private BeamBuilder beamBuilder;

        public Form1()
        {
            InitializeComponent();
            
        }
        public void InitializeAllModels(VectorDrawConfigure drawConfigure, BeamManager beamManager,
            BeamCutter beamCutter, BeamBuilder beamBuilder)
        {
            this.drawConfigure = drawConfigure;
            this.beamManager = beamManager;
            this.beamCutter = beamCutter;
            this.beamBuilder = beamBuilder;
        }
        public void AttachController(MainController controller)
        {
            mainController = controller;
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
            mainController.UpBeam_HorizontalUp();
        }
        private void UpBeam_ForVertical(object sender, EventArgs e)
        {
            mainController.UpBeam_Vertical();
        }

        private void CreateHorBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                return;
            mainController.CreateNewHorBeam(point);
        }

        private void CreateVerBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                return;
            mainController.CreateNewVerBeam(point);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VectorDraw.Professional.ActionUtilities.vdCommandAction.LineEx(vectorDrawBaseControl1.ActiveDocument);
        }
    }
}
