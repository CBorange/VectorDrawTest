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

namespace MathPractice
{
    public partial class Form1 : Form
    {
        private const int HALF_WIDTH = 390;
        private const int HALF_HEIGHT = 215;
        private const int PANEL_WIDTH = 780;
        private const int PANEL_HEIGHT = 430;
        private const int SIDEBEAM_ROT = 50;
        private const int SIDEBEAM_HEIGHT = 200;
        private const int SIDEBEAM_WIDTH = 50;
        private MathSupporter mathSupporter;

        // BaseLine
        private Point2 CenterPos;
        private Point2 baseLineStart_Y;
        private Point2 baseLineEnd_Y;
        private Point2 baseLineStart_X;
        private Point2 baseLineEnd_X;

        private Beam baseBeam;
        private Beam sideBeam;

        public Form1()
        {
            InitializeComponent();
            InitPoint();
            InitBeam();
            mathSupporter = MathSupporter.Instance;
            mathSupporter.SetClientRect(PANEL_WIDTH, PANEL_HEIGHT);
            VectorDrawTest();
        }
        private void InitPoint()
        {
            CenterPos = new Point2(0, 0);
            baseLineStart_Y = new Point2(0, -HALF_HEIGHT);
            baseLineEnd_Y = new Point2(0, HALF_HEIGHT);
            baseLineStart_X = new Point2(-HALF_WIDTH, 0);
            baseLineEnd_X = new Point2(HALF_WIDTH, 0);
        }
        private void InitBeam()
        {
            baseBeam = new Beam(new Point2(0, 0), SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, Color.Blue, 90);

            int rot = 60;
            sideBeam = new Beam(new Point2(0, 0), SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, Color.Red, rot);
            Point2 temp = new Point2(SIDEBEAM_HEIGHT * Math.Cos(Vector2.DegreesToRadians * rot), SIDEBEAM_HEIGHT * Math.Sin(Vector2.DegreesToRadians * rot));
            sideBeam.SetPosition(new Point2(temp.X * 0.5f, temp.Y * 0.5f));
        }
        private void VectorDrawTest()
        {
        }
    }
}
