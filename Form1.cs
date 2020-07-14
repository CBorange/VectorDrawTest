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

        private List<Beam> verBeams;
        private List<Beam> horBeams;
        private Beam vertical_Beam1;
        private Beam vertical_Beam2;
        private Beam horizontal_Beam;
        private vdText degreeText;
        private MathSupporter math;

        public Form1()
        {
            InitializeComponent();
            math = MathSupporter.Instance;
            verBeams = new List<Beam>();
            horBeams = new List<Beam>();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            vectorDrawBaseControl1.ActiveDocument.ShowUCSAxis = false;
            vectorDrawBaseControl1.ActiveDocument.ActiveLayOut.ZoomWindow(new gPoint(-HALF_WIDTH, -HALF_HEIGHT), new gPoint(HALF_WIDTH, HALF_HEIGHT));
            vdCommandLine1.LoadCommands(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Commands.txt");

            rotateAngle = 0;
            DrawBaseLine();
            InitBeam();
            
            DrawBeam();
            InitCuttingRect();
        }
        private void InitBeam()
        {
            vertical_Beam1 = new Beam(new gPoint(-50, 100), vectorDrawBaseControl1.ActiveDocument, SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, Color.Blue,
                Color.Blue, 90, "BaseBeam");
            vertical_Beam2 = new Beam(new gPoint(150, 100), vectorDrawBaseControl1.ActiveDocument, SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, Color.Blue,
                Color.Blue, 90, "BaseBeam2");

            horizontal_Beam = new Beam(new gPoint(50, 100), vectorDrawBaseControl1.ActiveDocument, SIDEBEAM_WIDTH, SIDEBEAM_HEIGHT, Color.Red,
                Color.Red, 135, "SideBeam");

            verBeams.Add(vertical_Beam1);
            verBeams.Add(vertical_Beam2);
            horBeams.Add(horizontal_Beam);

        }

        // DrawBaseLine
        private void DrawBaseLine()
        {
            AddLineToDocument(new gPoint(-1000, 0), new gPoint(1000, 0));
            AddLineToDocument(new gPoint(0, -1000), new gPoint(0, 1000));

            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void InitTestLine()
        {
            AddLineToDocument(new gPoint(-200, 100), new gPoint(200, 100));
            AddLineToDocument(new gPoint(-50, 200), new gPoint(150, -100));
        }
        private void AddCircleToDocument(gPoint point)
        {
            vdCircle cross = new vdCircle(vectorDrawBaseControl1.ActiveDocument, point, 2);
            cross.PenColor.SystemColor = Color.Yellow;
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

        private void DrawBeam()
        {
            for (int i = 0; i < verBeams.Count; ++i)
                verBeams[i].DrawBeam(vectorDrawBaseControl1.ActiveDocument);
            for (int i = 0; i < horBeams.Count; ++i)
                horBeams[i].DrawBeam(vectorDrawBaseControl1.ActiveDocument);
        }
        private void RotateBeam(int degree)
        {
            for (int i = 0; i < horBeams.Count; ++i)
                horBeams[i].RotateBeam(degree);
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
            degreeText.Height = 10;
            vectorDrawBaseControl1.ActiveDocument.Model.Entities.AddItem(degreeText);
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void DrawDegreeText()
        {
            degreeText.Height = 10;
            degreeText.Update();
            vectorDrawBaseControl1.ActiveDocument.Redraw(true);
        }
        private void InitCuttingRect()
        {

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
            horizontal_Beam.RemoveAllFigures();
            CalcCuttingRect_CrossAlgorithm(vertical_Beam1, horizontal_Beam);
            CalcCuttingRect_CrossAlgorithm(vertical_Beam2, horizontal_Beam);
        }
        private void CalcCuttingRect_RotationAlgorithm()
        {
            //Triangle miniTriangle = new Triangle();
            //Triangle bigTriangle = new Triangle();

            //// miniTriangle
            //double center2ASheta = Globals.DegreesToRadians(180 - (horizontalBar.Rotation + 90));
            //double Height = (horizontalBar.BeamHeight * 0.5f) * Math.Tan(center2ASheta);
            //miniTriangle.PointA = new gPoint(verticalBar.Center.x - horizontalBar.BeamHeight * 0.5f, verticalBar.Center.y);
            //miniTriangle.PointA.y = miniTriangle.PointA.y + Height;

            //Vector2 center2PointA_Unit = new Vector2(miniTriangle.PointA.x - horizontalBar.Left.x,
            //    miniTriangle.PointA.y - horizontalBar.Left.y).Normalize();
            //miniTriangle.PointB = new gPoint(center2PointA_Unit.X * horizontalBar.BeamHeight * 0.5f,
            //    center2PointA_Unit.Y * horizontalBar.BeamHeight * 0.5f);

            //double a2bLenghth = new Vector2(miniTriangle.PointA.x - miniTriangle.PointB.x,
            //    miniTriangle.PointA.y - miniTriangle.PointB.y).Length();

            //double shetaC = Globals.DegreesToRadians(180 - (horizontalBar.Rotation + 90));
            //double b2cLength = a2bLenghth / Math.Tan(shetaC);

            //Vector2 r2lUnit = new Vector2(horizontalBar.Left.x - horizontalBar.Right.x, 
            //    horizontalBar.Left.y - horizontalBar.Right.y).Normalize();
            //Vector2 rt2PointC = (b2cLength + horizontalBar.BeamWidth) * r2lUnit;
            //miniTriangle.PointC = new gPoint(horizontalBar.RightTop.x + rt2PointC.X,
            //    horizontalBar.RightTop.y + rt2PointC.Y);

            //// bigTriangle
            //double bigHeight = a2bLenghth + horizontalBar.BeamHeight;
            //double rb2PointCLength = bigHeight / Math.Tan(shetaC);
            //Vector2 rb2PointC = (horizontalBar.BeamWidth + rb2PointCLength) * r2lUnit;
            //bigTriangle.PointC = new gPoint(horizontalBar.RightBottom.x + rb2PointC.X,
            //    horizontalBar.RightBottom.y + rb2PointC.Y);

            //AddCircleToDocument(miniTriangle.PointC);
            //AddCircleToDocument(bigTriangle.PointC);
        }
        private void CalcCuttingRect_CrossAlgorithm(Beam verBeam, Beam horBeam)
        {
            List<gPoint> cuttingPointList = new List<gPoint>();
            gPoint[] expandedPoints = new gPoint[4];
            // VerBar Point
            gPoint verEX_RT2LT = math.GetExpandedPointBy2Points(verBeam.RightTop, verBeam.LeftTop, 5);
            gPoint verEX_RB2LB = math.GetExpandedPointBy2Points(verBeam.RightBottom, verBeam.LeftBottom, 5);
            gPoint verEX_RT2RB = math.GetExpandedPointBy2Points(verBeam.RightTop, verBeam.RightBottom, 5);
            gPoint verEX_LT2LB = math.GetExpandedPointBy2Points(verBeam.LeftTop, verBeam.LeftBottom, 5);

            // HorBar Point
            gPoint horEX_RT2LT = math.GetExpandedPointBy2Points(horBeam.RightTop, horBeam.LeftTop, 5);
            gPoint horEX_RB2LB = math.GetExpandedPointBy2Points(horBeam.RightBottom, horBeam.LeftBottom, 5);

            // 우측
            if (horBeam.Center.x > verBeam.Center.x - 1) 
            {
                double horRB2LB_Length = 0;
                double horRT2LT_Length = 0;

                // LT
                gPoint lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.RightTop, horEX_RT2LT);
                horRT2LT_Length = math.GetLengthBy2Point(horBeam.RightTop, lt);
                if (lt.y > verBeam.RightTop.y)
                {
                    lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.RightTop, horEX_RT2LT);

                    cuttingPointList.Add(verBeam.RightTop);
                    cuttingPointList.Add(lt);
                    horRT2LT_Length = math.GetLengthBy2Point(horBeam.RightTop, lt) +
                        math.GetLengthBy2Point(verBeam.RightTop, lt);
                }
                cuttingPointList.Add(lt);

                // RT
                gPoint rt = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.RightTop, horEX_RT2LT);
                if (rt.y > verBeam.RightBottom.y)
                {
                    rt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.RightTop, horEX_RT2LT);

                    cuttingPointList.Add(rt);
                    cuttingPointList.Add(verBeam.RightBottom);
                }
                else
                    cuttingPointList.Add(rt);

                // RB
                gPoint rb = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.RightBottom, horEX_RB2LB);
                if (rb.y < verBeam.LeftBottom.y)
                {
                    rb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.RightBottom, horEX_RB2LB);

                    cuttingPointList.Add(verBeam.LeftBottom);
                }
                cuttingPointList.Add(rb);

                // LB
                gPoint lb = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.RightBottom, horEX_RB2LB);
                horRB2LB_Length = math.GetLengthBy2Point(horBeam.RightBottom, lb);
                if (lb.y < verBeam.LeftTop.y)
                {
                    lb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.RightBottom, horEX_RB2LB);

                    cuttingPointList.Add(lb);
                    cuttingPointList.Add(verBeam.LeftTop);
                    horRB2LB_Length = math.GetLengthBy2Point(horBeam.RightBottom, lb) +
                        math.GetLengthBy2Point(verBeam.LeftTop, lb);
                }
                else
                    cuttingPointList.Add(lb);

                // Cutting Length에 따른 Expanded Point 계산
                if (horRT2LT_Length > horRB2LB_Length)
                {
                    
                    Vector2 horRT2LT_Unit = math.GetUnitVecBy2Point(lt, horBeam.RightTop);
                    Vector2 horRT2LTVec = horRT2LT_Unit * horRT2LT_Length;

                    Vector2 horRB2LB_Unit = math.GetUnitVecBy2Point(lb, horBeam.RightBottom);
                    Vector2 horRB2LBVec = horRB2LB_Unit * horRT2LT_Length;

                    expandedPoints[0] = lt;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.RightTop, horRT2LTVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.RightBottom, horRB2LBVec);
                    expandedPoints[3] = lb;
                }
                else
                {
                    Vector2 horRB2LB_Unit = math.GetUnitVecBy2Point(lb, horBeam.RightBottom);
                    Vector2 horRB2LBVec = horRB2LB_Unit * horRB2LB_Length;

                    Vector2 horRT2LT_Unit = math.GetUnitVecBy2Point(lt, horBeam.RightTop);
                    Vector2 horRT2LTVec = horRT2LT_Unit * horRB2LB_Length;

                    expandedPoints[0] = lb;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.RightBottom, horRB2LBVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.RightTop, horRT2LTVec);
                    expandedPoints[3] = lt;
                }
            }
            // 좌측
            else if (horBeam.Center.x <= verBeam.Center.x - 1)
            {
                double horRB2RT_Length = 0;
                double horRT2RB_Length = 0;

                // LT
                gPoint lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.RightBottom, horEX_RB2LB);
                if (lt.y > verBeam.RightTop.y)
                {
                    lt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.RightBottom, horEX_RB2LB);

                    cuttingPointList.Add(verBeam.RightTop);
                }
                cuttingPointList.Add(lt);

                // RT
                gPoint rt = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.RightBottom, horEX_RB2LB);
                horRB2RT_Length = math.GetLengthBy2Point(rt, horBeam.RightBottom);
                if (rt.y > verBeam.RightBottom.y)
                {
                    rt = math.GetCrossPoint(verBeam.RightTop, verEX_RT2RB, horBeam.RightBottom, horEX_RB2LB);

                    cuttingPointList.Add(rt);
                    cuttingPointList.Add(verBeam.RightBottom);
                    horRB2RT_Length = math.GetLengthBy2Point(horBeam.RightBottom, rt) +
                        math.GetLengthBy2Point(verBeam.RightBottom, rt);
                }
                else
                    cuttingPointList.Add(rt);

                // RB
                gPoint rb = math.GetCrossPoint(verBeam.RightBottom, verEX_RB2LB, horBeam.RightTop, horEX_RT2LT);
                horRT2RB_Length = math.GetLengthBy2Point(horBeam.RightTop, rb);
                if (rb.y < verBeam.LeftBottom.y)
                {
                    rb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.RightTop, horEX_RT2LT);

                    cuttingPointList.Add(verBeam.LeftBottom);
                    horRT2RB_Length = math.GetLengthBy2Point(horBeam.RightTop, rb) +
                        math.GetLengthBy2Point(verBeam.LeftBottom, rb);
                }
                cuttingPointList.Add(rb);

                // LB
                gPoint lb = math.GetCrossPoint(verBeam.RightTop, verEX_RT2LT, horBeam.RightTop, horEX_RT2LT);
                if (lb.y < verBeam.LeftTop.y)
                {
                    lb = math.GetCrossPoint(verBeam.LeftTop, verEX_LT2LB, horBeam.RightTop, horEX_RT2LT);

                    cuttingPointList.Add(lb);
                    cuttingPointList.Add(verBeam.LeftTop);
                }
                else
                    cuttingPointList.Add(lb);

                // Cutting Length에 따른 Expanded Point 계산
                if (horRB2RT_Length > horRT2RB_Length)
                {

                    Vector2 horRB2RT_Unit = math.GetUnitVecBy2Point(rt, horBeam.RightBottom);
                    Vector2 horRB2RTVec = horRB2RT_Unit * horRB2RT_Length;

                    Vector2 horRT2RB_Unit = math.GetUnitVecBy2Point(rb, horBeam.RightTop);
                    Vector2 horRT2RBVec = horRT2RB_Unit * horRB2RT_Length;

                    expandedPoints[0] = rt;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.RightBottom, horRB2RTVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.RightTop, horRT2RBVec);
                    expandedPoints[3] = rb;
                }
                else
                {
                    Vector2 horRT2RB_Unit = math.GetUnitVecBy2Point(rb, horBeam.RightTop);
                    Vector2 horRT2RBVec = horRT2RB_Unit * horRT2RB_Length;

                    Vector2 horRB2RT_Unit = math.GetUnitVecBy2Point(rt, horBeam.RightBottom);
                    Vector2 horRB2RTVec = horRB2RT_Unit * horRT2RB_Length;

                    expandedPoints[0] = rb;
                    expandedPoints[1] = math.GetExpandPoint(horBeam.RightTop, horRT2RBVec);
                    expandedPoints[2] = math.GetExpandPoint(horBeam.RightBottom, horRB2RTVec);
                    expandedPoints[3] = rt;
                }
            }

            gPoint[] cuttingPoints = cuttingPointList.ToArray();

            FigureDrawer cuttingFigure = new FigureDrawer(vectorDrawBaseControl1.ActiveDocument);
            cuttingFigure.Visible = true;
            cuttingFigure.DrawColor = Color.Green;
            cuttingFigure.SetPoints(cuttingPoints);
            horBeam.CuttingFigures.Add(cuttingFigure);

            FigureDrawer expandFigure = new FigureDrawer(vectorDrawBaseControl1.ActiveDocument);
            expandFigure.Visible = true;
            expandFigure.DrawColor = Color.Yellow;
            expandFigure.SetPoints(expandedPoints);
            horBeam.ExpandFigures.Add(expandFigure);

            DrawBeam();
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
