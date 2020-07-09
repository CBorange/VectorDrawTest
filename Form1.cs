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

        // Culling Triangle
        private bool triangleVisible;
        private Point2 trianglePoint1;
        private Point2 trianglePoint2;
        private Point2 trianglePoint3;
        public Form1()
        {
            triangleVisible = false;
            InitializeComponent();
            InitPoint();
            InitBeam();
            mathSupporter = MathSupporter.Instance;
            mathSupporter.SetClientRect(PANEL_WIDTH, PANEL_HEIGHT);
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
            SideBarRotLabel.Text = $"측면 바 각도 : {sideBeam.Rotation}";
        }
        private void DrawBaseLine(Graphics g)
        {
            Pen baseLinePen = new Pen(Color.Black, 1);
            baseLinePen.DashStyle = DashStyle.DashDotDot;
            g.DrawLine(baseLinePen, mathSupporter.TransformToLeftHand(baseLineStart_Y),
                mathSupporter.TransformToLeftHand(baseLineEnd_Y));
            g.DrawLine(baseLinePen, mathSupporter.TransformToLeftHand(baseLineStart_X),
                mathSupporter.TransformToLeftHand(baseLineEnd_X));
        }
        private void DrawBeam(Graphics g)
        {
            baseBeam.DrawBeam(g);
            sideBeam.DrawBeam(g);
        }
        private void DrawTraingle(Graphics g)
        {
            if (!triangleVisible)
                return;
            Pen trianglePen = new Pen(Color.Green, 3);
            g.DrawLine(trianglePen, mathSupporter.TransformToLeftHand(trianglePoint1),
                mathSupporter.TransformToLeftHand(trianglePoint2));
            g.DrawLine(trianglePen, mathSupporter.TransformToLeftHand(trianglePoint2),
                mathSupporter.TransformToLeftHand(trianglePoint3));
            g.DrawLine(trianglePen, mathSupporter.TransformToLeftHand(trianglePoint3),
                mathSupporter.TransformToLeftHand(trianglePoint1));
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawBaseLine(g);
            DrawBeam(g);
            DrawTraingle(g);        
        }
        private void button2_Click(object sender, EventArgs e)
        {
            sideBeam.RotateBeam(10);
            Point2 temp = new Point2(SIDEBEAM_HEIGHT * Math.Cos(Vector2.DegreesToRadians * sideBeam.Rotation), 
                SIDEBEAM_HEIGHT * Math.Sin(Vector2.DegreesToRadians * sideBeam.Rotation));
            sideBeam.SetPosition(new Point2(temp.X * 0.5f, temp.Y * 0.5f));
            SideBarRotLabel.Text = $"측면 바 각도 : {sideBeam.Rotation}";
            Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sideBeam.RotateBeam(-10);
            Point2 temp = new Point2(SIDEBEAM_HEIGHT * Math.Cos(Vector2.DegreesToRadians * sideBeam.Rotation),
                SIDEBEAM_HEIGHT * Math.Sin(Vector2.DegreesToRadians * sideBeam.Rotation));
            sideBeam.SetPosition(new Point2(temp.X * 0.5f, temp.Y * 0.5f));
            SideBarRotLabel.Text = $"측면 바 각도 : {sideBeam.Rotation}";
            Refresh();
        }
    }
}
