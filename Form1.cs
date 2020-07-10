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

namespace MathPractice
{
    public partial class Form1 : Form
    {
        private const int HALF_WIDTH = 325;
        private const int HALF_HEIGHT = 325;
        private const int PANEL_WIDTH = 650;
        private const int PANEL_HEIGHT = 650;
        private const int SIDEBEAM_ROT = 50;
        private const int SIDEBEAM_HEIGHT = 200;
        private const int SIDEBEAM_WIDTH = 50;

        private Beam baseBeam;
        private Beam sideBeam;
        private vdText degreeText;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            vectorDrawBaseControl1.ActiveDocument.ShowUCSAxis = false;
            vectorDrawBaseControl1.ActiveDocument.ActiveLayOut.ZoomWindow(new gPoint(-HALF_WIDTH, -HALF_HEIGHT), new gPoint(HALF_WIDTH, HALF_HEIGHT));

            InitBeam();
            DrawBaseLine();
            DrawBeam();
            InitDegreeText();
        }
        private void InitBeam()
        {
            baseBeam = new Beam(new gPoint(0, 0), vectorDrawBaseControl1.ActiveDocument, SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, 2, 0,
                "BaseBeam");
            

            int rot = 45;
            sideBeam = new Beam(new gPoint(0, 0), vectorDrawBaseControl1.ActiveDocument, SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, 4, rot,
                "SideBeam");
        }

        // DrawBaseLine
        private void DrawBaseLine()
        {
            AddBaseLineToDocument(new gPoint(-HALF_WIDTH, 0), new gPoint(HALF_WIDTH, 0));
            AddBaseLineToDocument(new gPoint(0, -HALF_HEIGHT), new gPoint(0, HALF_HEIGHT));
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void AddBaseLineToDocument(gPoint startPoint, gPoint endPoint)
        {
            vdLine newLine = new vdLine();
            newLine.SetUnRegisterDocument(vectorDrawBaseControl1.ActiveDocument);
            newLine.setDocumentDefaults();

            newLine.StartPoint = startPoint;
            newLine.EndPoint = endPoint;
            newLine.PenColor.ColorIndex = 3;
            newLine.PenWidth = 1;
            newLine.LineType = vectorDrawBaseControl1.ActiveDocument.LineTypes.FindName("DASHDOT0");

            vectorDrawBaseControl1.ActiveDocument.Model.Entities.AddItem(newLine);
        }

        // DrawBeam
        private void DrawBeam()
        {
            gPoint temp = new gPoint(SIDEBEAM_HEIGHT * Math.Cos(Vector2.DegreesToRadians * (sideBeam.Rotation + 90)),
                SIDEBEAM_HEIGHT * Math.Sin(Vector2.DegreesToRadians * (sideBeam.Rotation + 90)));
            sideBeam.SetPosition(new gPoint(temp.x * 0.5f, temp.y * 0.5f));

            baseBeam.DrawBeam(vectorDrawBaseControl1.ActiveDocument);
            sideBeam.DrawBeam(vectorDrawBaseControl1.ActiveDocument);
        }
        private void RotateBeam(int degree)
        {
            sideBeam.RotateBeam(degree);
            DrawDegreeText();
            DrawBeam();
        }
        private void InitDegreeText()
        {
            degreeText = new vdText();
            degreeText.SetUnRegisterDocument(vectorDrawBaseControl1.ActiveDocument);
            degreeText.setDocumentDefaults();

            degreeText.PenColor.ColorIndex = 3;
            vectorDrawBaseControl1.ActiveDocument.TextStyles.Standard.FontFile = "Verdana";
            degreeText.InsertionPoint = new gPoint(-HALF_WIDTH + 50, HALF_HEIGHT - 50);
            degreeText.TextString = $"각도 : {sideBeam.Rotation}";
            degreeText.Height = 10;
            vectorDrawBaseControl1.ActiveDocument.Model.Entities.AddItem(degreeText);
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void DrawDegreeText()
        {
            degreeText.TextString = $"각도 : {sideBeam.Rotation}";
            degreeText.Height = 10;
            degreeText.Update();
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RotateBeam(10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RotateBeam(-10);
        }
    }
}
