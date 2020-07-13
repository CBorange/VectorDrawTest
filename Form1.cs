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
        private int rotateAngle;
        private const int HALF_WIDTH = 325;
        private const int HALF_HEIGHT = 325;
        private const int PANEL_WIDTH = 650;
        private const int PANEL_HEIGHT = 650;
        private const int SIDEBEAM_ROT = 50;
        private const int SIDEBEAM_WIDTH = 200;
        private const int SIDEBEAM_HEIGHT = 50;

        private Beam verticalBar;
        private Beam horizontalBar;
        private CuttingRect cuttingRect;
        private vdText degreeText;
        private MathSupporter math;

        public Form1()
        {
            InitializeComponent();
            math = MathSupporter.Instance;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            vectorDrawBaseControl1.ActiveDocument.ShowUCSAxis = false;
            vectorDrawBaseControl1.ActiveDocument.ActiveLayOut.ZoomWindow(new gPoint(-HALF_WIDTH, -HALF_HEIGHT), new gPoint(HALF_WIDTH, HALF_HEIGHT));
            vdCommandLine1.LoadCommands(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Commands.txt");

            rotateAngle = 0;
            InitBeam();
            DrawBaseLine();
            DrawBeam();
            InitDegreeText();
            InitCuttingRect();
            DrawCuttingRect();
        }
        private void InitBeam()
        {
            verticalBar = new Beam(new gPoint(0, 0), vectorDrawBaseControl1.ActiveDocument, SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, Color.Blue,
                Color.Blue, 90, "BaseBeam");

            horizontalBar = new Beam(new gPoint(0, 0), vectorDrawBaseControl1.ActiveDocument, SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, Color.Red,
                Color.Red, 45, "SideBeam");
        }

        // DrawBaseLine
        private void DrawBaseLine()
        {
            AddLineToDocument(new gPoint(-HALF_WIDTH, 0), new gPoint(HALF_WIDTH, 0));
            AddLineToDocument(new gPoint(0, -HALF_HEIGHT), new gPoint(0, HALF_HEIGHT));

            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void InitTestLine()
        {
            AddLineToDocument(new gPoint(-200, 100), new gPoint(200, 100));
            AddLineToDocument(new gPoint(-50, 200), new gPoint(150, -100));

            gPoint crossPoint = math.GetCrossPoint(new gPoint(-200, 100), new gPoint(200, 100), new gPoint(-50, 200), new gPoint(150, -100));

        }
        private void AddCircleToDocument(gPoint point)
        {
            vdCircle cross = new vdCircle(vectorDrawBaseControl1.ActiveDocument, point, 1);
            vectorDrawBaseControl1.ActiveDocument.Model.Entities.Add(cross);
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void AddLineToDocument(gPoint startPoint, gPoint endPoint)
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
            gPoint temp = new gPoint(SIDEBEAM_WIDTH * Math.Cos(Vector2.DegreesToRadians * (horizontalBar.Rotation)),
                SIDEBEAM_WIDTH * Math.Sin(Vector2.DegreesToRadians * (horizontalBar.Rotation)));
            horizontalBar.SetPosition(new gPoint(temp.x * 0.5f, temp.y * 0.5f));

            horizontalBar.DrawBeam(vectorDrawBaseControl1.ActiveDocument);
            verticalBar.DrawBeam(vectorDrawBaseControl1.ActiveDocument);
        }
        private void RotateBeam(int degree)
        {
            horizontalBar.RotateBeam(degree);
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
            degreeText.TextString = $"각도 : {horizontalBar.Rotation}";
            degreeText.Height = 10;
            vectorDrawBaseControl1.ActiveDocument.Model.Entities.AddItem(degreeText);
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void DrawDegreeText()
        {
            degreeText.TextString = $"각도 : {horizontalBar.Rotation}";
            degreeText.Height = 10;
            degreeText.Update();
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void InitCuttingRect()
        {
            cuttingRect = new CuttingRect(vectorDrawBaseControl1.ActiveDocument);
        }
        private void DrawCuttingRect()
        {
            cuttingRect.DrawCuttingRect();
        }

        private void PlusRotate_Click(object sender, EventArgs e)
        {
            RotateBeam(5);
        }

        private void MinusRotate_Click(object sender, EventArgs e)
        {
            RotateBeam(-5);
        }

        private void CuttingByHorizontal_Click(object sender, EventArgs e)
        {
            // 유효성 검사
            if (horizontalBar.Rotation == 90 || horizontalBar.Rotation == 270)
            {
                MessageBox.Show("수평바가 수직과 평행합니다.", "Cutting 검사 불가", MessageBoxButtons.OK);
                cuttingRect.Visible = false;
                return;
            }
            //CalcCuttingRect_CrossAlgorithm();
            //cuttingRect.Visible = true;
            CalcCuttingRect_RotationAlgorithm();
            DrawCuttingRect();
        }
        private void CalcCuttingRect_RotationAlgorithm()
        {
            Triangle miniTriangle = new Triangle();
            Triangle bigTriangle = new Triangle();

            // miniTriangle
            double center2ASheta = Globals.DegreesToRadians(180 - (horizontalBar.Rotation + 90));
            double Height = (horizontalBar.BeamHeight * 0.5f) * Math.Tan(center2ASheta);
            miniTriangle.PointA = new gPoint(verticalBar.Center.x - horizontalBar.BeamHeight * 0.5f, verticalBar.Center.y);
            miniTriangle.PointA.y = miniTriangle.PointA.y + Height;

            Vector2 center2PointA_Unit = new Vector2(miniTriangle.PointA.x - horizontalBar.Left.x,
                miniTriangle.PointA.y - horizontalBar.Left.y).Normalize();
            miniTriangle.PointB = new gPoint(center2PointA_Unit.X * horizontalBar.BeamHeight * 0.5f,
                center2PointA_Unit.Y * horizontalBar.BeamHeight * 0.5f);

            double a2bLenghth = new Vector2(miniTriangle.PointA.x - miniTriangle.PointB.x,
                miniTriangle.PointA.y - miniTriangle.PointB.y).Length();

            double shetaC = Globals.DegreesToRadians(180 - (horizontalBar.Rotation + 90));
            double b2cLength = a2bLenghth / Math.Tan(shetaC);

            Vector2 r2lUnit = new Vector2(horizontalBar.Left.x - horizontalBar.Right.x, 
                horizontalBar.Left.y - horizontalBar.Right.y).Normalize();
            Vector2 rt2PointC = (b2cLength + horizontalBar.BeamWidth) * r2lUnit;
            miniTriangle.PointC = new gPoint(horizontalBar.RightTop.x + rt2PointC.X,
                horizontalBar.RightTop.y + rt2PointC.Y);

            // bigTriangle
            double bigHeight = a2bLenghth + horizontalBar.BeamHeight;
            double rb2PointCLength = bigHeight / Math.Tan(shetaC);
            Vector2 rb2PointC = (horizontalBar.BeamWidth + rb2PointCLength) * r2lUnit;
            bigTriangle.PointC = new gPoint(horizontalBar.RightBottom.x + rb2PointC.X,
                horizontalBar.RightBottom.y + rb2PointC.Y);

            AddCircleToDocument(miniTriangle.PointC);
            AddCircleToDocument(bigTriangle.PointC);
        }
        private void CalcCuttingRect_CrossAlgorithm()
        {
            List<gPoint> pointList = new List<gPoint>();
            // 우측
            if (horizontalBar.Center.x > -1)
            {
                gPoint lt = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.LeftTop, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));
                if (lt.y > verticalBar.RightTop.y)
                {
                    lt = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.RightBottom, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));

                    pointList.Add(verticalBar.RightTop);
                }
                pointList.Add(lt);

                gPoint rt = math.GetCrossPoint(verticalBar.RightBottom, math.GetExpandedPointBy2Points(verticalBar.RightBottom, verticalBar.LeftBottom, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));
                if (rt.y > verticalBar.RightBottom.y)
                {
                    rt = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.RightBottom, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));

                    pointList.Add(rt);
                    pointList.Add(verticalBar.RightBottom);
                }
                else
                    pointList.Add(rt);

                gPoint rb = math.GetCrossPoint(verticalBar.RightBottom, math.GetExpandedPointBy2Points(verticalBar.RightBottom, verticalBar.LeftBottom, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));
                if (rb.y < verticalBar.LeftBottom.y)
                {
                    rb = math.GetCrossPoint(verticalBar.LeftTop, math.GetExpandedPointBy2Points(verticalBar.LeftTop, verticalBar.LeftBottom, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));

                    pointList.Add(verticalBar.LeftBottom);
                }
                pointList.Add(rb);

                gPoint lb = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.LeftTop, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));
                if (lb.y < verticalBar.LeftTop.y)
                {
                    lb = math.GetCrossPoint(verticalBar.LeftTop, math.GetExpandedPointBy2Points(verticalBar.LeftTop, verticalBar.LeftBottom, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));

                    pointList.Add(lb);
                    pointList.Add(verticalBar.LeftTop);
                }
                else
                    pointList.Add(lb);
            }
            // 좌측
            else if (horizontalBar.Center.x <= -1)
            {
                gPoint lt = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.LeftTop, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));
                if (lt.y > verticalBar.RightTop.y)
                {
                    lt = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.RightBottom, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));

                    pointList.Add(verticalBar.RightTop);
                }
                pointList.Add(lt);

                gPoint rt = math.GetCrossPoint(verticalBar.RightBottom, math.GetExpandedPointBy2Points(verticalBar.RightBottom, verticalBar.LeftBottom, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));
                if (rt.y > verticalBar.RightBottom.y)
                {
                    rt = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.RightBottom, 1),
                    horizontalBar.RightBottom, math.GetExpandedPointBy2Points(horizontalBar.RightBottom, horizontalBar.LeftBottom, 1));

                    pointList.Add(rt);
                    pointList.Add(verticalBar.RightBottom);
                }
                else
                    pointList.Add(rt);

                gPoint rb = math.GetCrossPoint(verticalBar.RightBottom, math.GetExpandedPointBy2Points(verticalBar.RightBottom, verticalBar.LeftBottom, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));
                if (rb.y < verticalBar.LeftBottom.y)
                {
                    rb = math.GetCrossPoint(verticalBar.LeftTop, math.GetExpandedPointBy2Points(verticalBar.LeftTop, verticalBar.LeftBottom, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));

                    pointList.Add(verticalBar.LeftBottom);
                }
                pointList.Add(rb);

                gPoint lb = math.GetCrossPoint(verticalBar.RightTop, math.GetExpandedPointBy2Points(verticalBar.RightTop, verticalBar.LeftTop, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));
                if (lb.y < verticalBar.LeftTop.y)
                {
                    lb = math.GetCrossPoint(verticalBar.LeftTop, math.GetExpandedPointBy2Points(verticalBar.LeftTop, verticalBar.LeftBottom, 1),
                    horizontalBar.RightTop, math.GetExpandedPointBy2Points(horizontalBar.RightTop, horizontalBar.LeftTop, 1));

                    pointList.Add(lb);
                    pointList.Add(verticalBar.LeftTop);
                }
                else
                    pointList.Add(lb);

            }
            gPoint[] points = pointList.ToArray();
            cuttingRect.SetPoints(points);
        }

        private void CuttingByVertical_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            vectorDrawBaseControl1.ActiveDocument.CommandAction.CmdLine("USER");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            vectorDrawBaseControl1.ActiveDocument.CommandAction.CmdErase(null);
        }
    }
}
