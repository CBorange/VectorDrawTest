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
using Spatial;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using MathPractice.View;
using MathPractice.Controller;
using MathPractice.Model;

namespace MathPractice
{
    public partial class Form1 : Form, IMainView
    {
        // Controller
        private MainController mainController;

        // Model
        private VectorDrawConfigure drawConfigure;
        private BeamManager beamManager;
        private BeamCollisionCalculator collisionCalculator;
        private BeamBuilder beamBuilder;

        public Form1()
        {
            InitializeComponent();
            
        }
        public void InitializeAllModels(VectorDrawConfigure drawConfigure, BeamManager beamManager,
            BeamCollisionCalculator collisionCalculator, BeamBuilder beamBuilder)
        {
            this.drawConfigure = drawConfigure;
            this.beamManager = beamManager;
            this.collisionCalculator = collisionCalculator;
            this.beamBuilder = beamBuilder;
        }
        public void AttachController(MainController controller)
        {
            mainController = controller;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            drawConfigure.InitializeSystem(vectorDrawBaseControl1.ActiveDocument, vdCommandLine1);
            collisionCalculator.Initialize(vectorDrawBaseControl1.ActiveDocument, beamManager);
            beamManager.Initialize(vectorDrawBaseControl1.ActiveDocument, collisionCalculator, drawConfigure);
            beamBuilder.Initialize(vectorDrawBaseControl1.ActiveDocument, beamManager);
        }

        private void PlusRotate_Click(object sender, EventArgs e)
        {
            mainController.RotateBeam_Plus();
        }

        private void MinusRotate_Click(object sender, EventArgs e)
        {
            mainController.RotateBeam_Minus();
        }

        private void CuttingByHorizontal_Click(object sender, EventArgs e)
        {
            mainController.CuttingBeam_HorizontalUp();
        }

        private void CreateHorBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                Debug.Write("null");
        }

        private void CreateVerBeam(object sender, EventArgs e)
        {
            gPoint point;
            vectorDrawBaseControl1.ActiveDocument.ActionUtility.getUserPoint(out point);
            if (point == null)
                Debug.Write("null");
        }
    }
}
