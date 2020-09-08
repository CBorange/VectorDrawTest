using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Hicom.BizDraw.PlanDraw;
using Hicom.BizDraw.Entity;

using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;
using VectorDraw.Actions;
using VectorDraw.Serialize;

namespace Hicom.BizDraw.Browser
{
    public partial class MainForm : Form
    {
        private const string LAYER_ENT_YELLOW = "LAYER_ENT_YELLOW";
        private const string LAYER_ENT_GREEN = "LAYER_ENT_GREEN";
        private const string LAYER_DIM_WHITE = "LAYER_DIM_WHITE";
        private const string LAYER_DIM_CYAN = "LAYER_DIM_CYAN";
        private const string LAYER_TEXT_PURPLE = "LAYER_TEXT_PURPLE";
        private const string TEXT_STYLE = "TEXT";

        private vdDocument Document { get { return this.BizDrawCtrl.ActiveDocument; } }
        private DrawHandler DrawHandler { get { return this.BizDrawCtrl.DrawHandler; } }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.BizDrawCtrl.Initialize();
            this.BizDrawCtrl.OsnapMode = OsnapMode.END | OsnapMode.MID | OsnapMode.CEN | OsnapMode.INTERS | OsnapMode.EXTENSION;
        }

        private void EventCommandFinished(object sender, List<object> resultObject, string command)
        {

        }

        #region File
        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.OPEN);
        }
        private void MenuItemSave_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.SAVE);
        }
        private void MenuItemSaveAs_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.SAVEAS);
        }
        private void MenuItemPrint_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.PRINT);
        }
        private void MenuItemImport_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.IMPORT);
        }
        #endregion

        #region Edit
        private void MenuItemRedraw_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.REDRAW);
        }
        private void MenuItemRegen_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.REGEN);
        }
        private void MenuItemZoomAll_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ZOOM);
        }
        private void MenuItemZoomIn_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ZOOM_IN);
        }
        private void MenuItemZoomOut_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ZOOM_OUT);
        }
        private void MenuItemZoomWindow_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ZOOM_WINDOW);
        }
        private void MenuItemOsnap_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.OSNAP_WINDOW);
        }
        #endregion

        #region Format
        private void MenuItemLayer_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.LAYER);
        }
        #endregion

        #region Draw
        private void MenuItemLine_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.AddRepeatCommand(Hicom.BizDraw.Command.Command.LINE);
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.LINE);
            this.BizDrawCtrl.RemoveRepeatCommand(Hicom.BizDraw.Command.Command.LINE);
        }
        private void MenuItemCircle_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.CIRCLE);
        }
        private void MenuItemCircle2P_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.CIRCLE_2P);
        }
        private void MenuItemCircle3P_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.CIRCLE_3P);
        }
        private void MenuItemArc_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ARC);
        }
        private void MenuItemArc3P_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ARC_3P);
        }
        private void MenuItemArcAngle_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ARC_2PA);
        }
        private void MenuItemArcLength_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemRectangle_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ARC_2PA);
        }
        private void MenuItemPolyline_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemText_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemMultiText_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemLeader_Click(object sender, EventArgs e)
        {
            //
        }
        #endregion

        #region Modify
        private void MenuItemMove_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.MOVE);
        }
        private void MenuItemCopy_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.COPY);
        }
        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.DELETE);
        }
        private void MenuItemRotate_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.ROTATE);
        }
        private void MenuItemExtend_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemTrim_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemExplode_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemExplodeAll_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemEditText_Click(object sender, EventArgs e)
        {
            //
        }
        private void MenuItemToBlock_Click(object sender, EventArgs e)
        {
            //
        }
        #endregion

        #region Dimension
        private void MenuItemVertical_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.DIM_VERTICAL);
        }
        private void MenuItemHorizontal_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.DIM_HORIZONTAL);
        }
        private void MenuItemAligned_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.DIM_ALIGNED);
        }
        private void MenuItemAngle_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.DIM_ANGULAR);
        }
        #endregion

        private void MenuItemTextEntity_Click(object sender, EventArgs e)
        {
            this.DrawHandler.Text.SetActiveLayer("TEXT");
            this.DrawHandler.Text.TextStyle = "TEXT";            
            this.DrawHandler.Text.InsertText(new gPoint(0, 0), "HICOMTECH CURTAINWALL PRO");            
            this.DrawHandler.Redraw();
        }

        private void MenuItemMultiTextEntity_Click(object sender, EventArgs e)
        {
            this.DrawHandler.Text.SetActiveLayer("TEXT");
            this.DrawHandler.Text.TextStyle = "TEXT";
            this.DrawHandler.Text.InsertMText(new gPoint(0, 0), "HICOMTECH\nCURTAINWALL PRO");
            this.DrawHandler.Redraw();
        }

        private void MenuItemLeaderEntity_Click(object sender, EventArgs e)
        {
            this.DrawHandler.Text.SetActiveLayer("TEXT");
            this.DrawHandler.Text.TextStyle = "TEXT";
            this.DrawHandler.Text.HorJust = VectorDraw.Professional.Constants.VdConstHorJust.VdTextHorLeft;
            this.DrawHandler.Text.VerJust = VectorDraw.Professional.Constants.VdConstVerJust.VdTextVerCen;
            this.DrawHandler.Text.InsertLeaderText(new gPoint(0, 0), new gPoint(50, 50), new gPoint(200, 100), "HICOMTECH\nCURTAINWALL PRO", DrawText.eTextPosition.Middle);
            this.DrawHandler.Redraw();
        }
        
        private void MenuItemEntities_Click(object sender, EventArgs e)
        {
            gPoint stt = new gPoint(0, 0);
            gPoint end = new gPoint(1500, 0);

            this.DrawHandler.Object.SetActiveLayer(LAYER_ENT_YELLOW);
            for (int ix = 0; ix < 5; ix++)
            {
                this.DrawHandler.Object.DrawLine(stt, end);
                stt = stt + Geometry.Geometry.Yaxis * 500;
                end = stt + Geometry.Geometry.Xaxis * 1500;
            }

            stt = new gPoint(0, 0);
            end = new gPoint(0, 2000);
            for (int ix = 0; ix < 4; ix++)
            {
                this.DrawHandler.Object.DrawLine(stt, end);
                stt = stt + Geometry.Geometry.Xaxis * 500;
                end = stt + Geometry.Geometry.Yaxis * 2000;
            }

            this.DrawHandler.Object.SetActiveLayer(LAYER_ENT_GREEN);
            this.DrawHandler.Object.HatchProperties.FillMode = VectorDraw.Professional.Constants.VdConstFill.VdFillModeNone;
            HCircle circle = this.DrawHandler.Object.DrawCircle(new gPoint(0, 0), 500);
            circle.UnRemove = true;
            this.DrawHandler.Object.HatchProperties.FillMode = VectorDraw.Professional.Constants.VdConstFill.VdFillModeNone;
            HArc arc = this.DrawHandler.Object.DrawArc(new gPoint(0, 0), 300, 45, 135);
            arc.UnRemove = true;
            this.DrawHandler.Object.DrawRectangle(new gPoint(0, 0), 300, 400);

            this.DrawHandler.Object.DrawLine(new gPoint(2000, 0), new gPoint(3500, 500));
            this.DrawHandler.Object.DrawLine(new gPoint(2000, 0), new gPoint(2500, 1500));
            this.DrawHandler.Object.DrawLine(new gPoint(2000, 0), new gPoint(2500, 2800));

            this.DrawHandler.Redraw();
            this.BizDrawCtrl.ZoomAll();
        }

        /// <summary>
        /// Dimension TEST code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDrawDimension_Click(object sender, EventArgs e)
        {
            gPoint stt = new gPoint(0, 0);
            gPoint end = new gPoint(0, 500);
            // Line 객체 생성
            this.DrawHandler.Dimension.ArrowBlock = DrawDimension.ARROWTYPE_CIRCLE;
            this.DrawHandler.Dimension.ArrowBlock2 = DrawDimension.ARROWTYPE_CIRCLE;

            this.DrawHandler.Dimension.SetActiveLayer(LAYER_DIM_WHITE);
            this.DrawHandler.Dimension.SetLevel(1);
            this.DrawHandler.Dimension.SetDirection(DrawDimension.eDirection.VerLeft);
            this.DrawHandler.Dimension.Options.ScaleFactor = 30;

            this.DrawHandler.Dimension.Draw(stt, end);
            for (int ix = 0; ix < 3; ix++)
                this.DrawHandler.Dimension.DrawTo(500);
            
            this.DrawHandler.Dimension.SetDirection(DrawDimension.eDirection.HorLeft);
            this.DrawHandler.Dimension.Draw(new gPoint(0, 2000), new gPoint(500, 2000));
            for (int ix = 0; ix < 2; ix++)
                this.DrawHandler.Dimension.DrawTo(500);

            this.DrawHandler.Dimension.SetActiveLayer(LAYER_DIM_CYAN);
            this.DrawHandler.Dimension.ArrowBlock = DrawDimension.ARROWTYPE_DIAGONAL;
            this.DrawHandler.Dimension.ArrowBlock2 = DrawDimension.ARROWTYPE_DIAGONAL;
            // Overall dimension
            this.DrawHandler.Dimension.SetLevel(2);
            this.DrawHandler.Dimension.SetDirection(DrawDimension.eDirection.VerLeft);
            this.DrawHandler.Dimension.Draw(new gPoint(0, 0), new gPoint(0, 2000));
            this.DrawHandler.Dimension.SetDirection(DrawDimension.eDirection.HorLeft);
            this.DrawHandler.Dimension.Draw(new gPoint(0, 2000), new gPoint(1500, 2000));

            this.DrawHandler.Dimension.SetLevel(1);
            this.DrawHandler.Dimension.SetDirection(DrawDimension.eDirection.Right);
            this.DrawHandler.Dimension.DrawRectangular(new gPoint(0, 0), 300, 400);

            gPoint center = new gPoint(2000, 0);
            gPoint p2 = new gPoint(3500, 500);
            gPoint p3 = new gPoint(2500, 1500);
            gPoint p4 = new gPoint(2500, 2800);
            this.DrawHandler.Dimension.DrawAngular(center, p2, p3);
            Geometry.Line line1 = new Geometry.Line(center, p2);
            Geometry.Line line2 = new Geometry.Line(center, p4);

            this.DrawHandler.Dimension.DrawAngular(line1, line2, 1000);

            this.DrawHandler.Redraw();
            this.BizDrawCtrl.ZoomAll();
        }

        private void MenuItemLayerAdd_Click(object sender, EventArgs e)
        {
            this.BizDrawCtrl.AddLayer(LAYER_ENT_YELLOW, Color.Yellow, "SOLID", VectorDraw.Professional.Constants.VdConstLineWeight.LW_35);
            this.BizDrawCtrl.AddLayer(LAYER_ENT_GREEN, Color.Green, "SOLID", VectorDraw.Professional.Constants.VdConstLineWeight.LW_35);
            this.BizDrawCtrl.AddLayer(LAYER_DIM_WHITE, Color.White, "SOLID", VectorDraw.Professional.Constants.VdConstLineWeight.LW_20);
            this.BizDrawCtrl.AddLayer(LAYER_DIM_WHITE, Color.Cyan, "SOLID", VectorDraw.Professional.Constants.VdConstLineWeight.LW_20);
            this.BizDrawCtrl.AddLayer(LAYER_TEXT_PURPLE, Color.Purple, "SOLID", VectorDraw.Professional.Constants.VdConstLineWeight.LW_30);            
            this.BizDrawCtrl.AddTextStyle(TEXT_STYLE, "Verdana", 30);
        }

        private void MenuItemSTLOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                VectorDraw.Professional.Serialize.vdSTL_Format stl = new VectorDraw.Professional.Serialize.vdSTL_Format();
                stl.Open(dlg.FileName, this.BizDrawCtrl.ActiveDocument);
            }
        }

        private void MenuItemPNTOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select PNT File";
            dlg.Filter = "PNT|*.pnt|All Files|*.*";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                Vertexes points = new Vertexes();
                using (FileStream stream = new FileStream(dlg.FileName, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);
                    string line = string.Empty;
                    while((line = reader.ReadLine()) != null)
                    {
                        if(line.Contains("UNIT"))
                        {

                        }
                        else if(line.Contains("POINT"))
                        {
                            string[] split = line.Split(',');
                            gPoint point = new gPoint();
                            SetPoint(ref point, split[1]);
                            SetPoint(ref point, split[2]);
                            SetPoint(ref point, split[3]);
                            points.Add(point);
                        }
                    }

                    reader.Close();
                }

                DrawHandler.Object.DrawPolyline(points, VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE);
                DrawHandler.Redraw();
                BizDrawCtrl.ZoomAll();
            }
        }

        private void SetPoint(ref gPoint point, string value)
        {
            string[] split = value.Split('=');
            if(split.Length > 1)
            {
                switch(split[0].ToUpper())
                {
                    case "X":
                        point.x = ToDouble(split[1]);
                        break;
                    case "Y":
                        point.y = ToDouble(split[1]);
                        break;
                    case "Z":
                        point.z = ToDouble(split[1]);
                        break;
                }
            }
        }

        private double ToDouble(string value)
        {
            double result = 0;
            if (double.TryParse(value, out result))
                return result;
            return 0;
        }

        private void BizDrawCtrl_EventHandlerCommandStart(string commandname, bool isDefaultImplemented, ref bool success, ref bool continuous, ref List<object> figures)
        {
            switch(commandname.ToLower())
            {
                case "a":
                    InsertArcEx();
                    break;
                case "h":
                    InsertHollowCircle();
                    break;
                case "cut":
                    CutLineWidth();
                    continuous = false;
                    break;
                case Command.Command.DOUBLECLICK:
                    break;
            }
        }

        private void InsertHollowCircle()
        {
            vdCircle circle1 = new vdCircle(Document, new gPoint(0, 0), 1000);
            vdCircle circle2 = new vdCircle(Document, new gPoint(0, 0), 900);

            vdPolyline polyline1 = new vdPolyline(Document);
            polyline1.JoinEntity(circle1, 0.001);
            vdPolyline polyline2 = new vdPolyline(Document);
            polyline2.JoinEntity(circle2, 0.001);

            vdCurves curves = new vdCurves();
            curves.AddItem(polyline1);
            curves.AddItem(polyline2);

            vdPolyCurves polycurve = new vdPolyCurves(new vdCurves[] { curves } );

            vdPolyhatch polyHatch = new vdPolyhatch(Document, polycurve, new vdColor(Color.AliceBlue), false);
            polyHatch.Layer = Document.ActiveLayer;

            Document.ActiveLayOut.Entities.Add(polyHatch);
            
            Document.Redraw(true);
        }

        private void InsertArcEx()
        {
            double radius = 500;
            double width = 50;
            double startAngle = 17 * Math.PI / 180;
            double endAngle = 86 * Math.PI / 180;
            gPoint center = new gPoint(0, 0);

            for(int ix = 0; ix < 10; ix ++)
            {
                vdArc vArc1 = new vdArc(Document, center, radius + width / 2, startAngle, endAngle);
                vArc1.Layer = Document.ActiveLayer;

                vdArc vArc2 = new vdArc(Document, center, radius - width / 2, startAngle, endAngle);
                vArc2.Layer = Document.ActiveLayer;

                vdLine vL1 = new vdLine(vArc1.getStartPoint(), vArc2.getStartPoint());
                vdLine vL2 = new vdLine(vArc1.getEndPoint(), vArc2.getEndPoint());

                vdSelection selection = new vdSelection("my_selection");
                selection.AddItem(vArc1, false, vdSelection.AddItemCheck.Nochecking);
                selection.AddItem(vArc2, false, vdSelection.AddItemCheck.Nochecking);
                selection.AddItem(vL1, false, vdSelection.AddItemCheck.Nochecking);
                selection.AddItem(vL2, false, vdSelection.AddItemCheck.Nochecking);

                vdPolyline polyline1 = new vdPolyline(Document);
                polyline1.JoinEntities(selection, 0.0001);
                Document.ActiveLayOut.Entities.Add(polyline1);

                //polyline1.VertexList.Add(center);
                //polyline1.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;

                //vdPolyline polyline2 = vArc2.AsPolyline();
                //polyline2.VertexList.Add(center);
                //polyline2.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;

                //vdCurves result = polyline1.Combine(polyline2, vdPolyline.CombineMode.Exclude);

                //if (result.Count > 0)
                //{
                //    foreach (vdCurve vCurve in result)
                //    {
                //        vdPolyline vp = vCurve as vdPolyline;

                //        if(vp.VertexList.Count > 10)
                //            Document.ActiveLayOut.Entities.Add(vCurve);
                //    }

                //}

                radius += 100;
            }

            
            Document.Redraw(true);
        }

        private void CutLineWidth()
        {
            //this.Document.Prompt("Select LineWidth Entity.");
            //vdFigure fig1;
            //gPoint retpt;
            //StatusCode scode = this.Document.ActionUtility.getUserEntity(out fig1, out retpt);
            //if(scode == StatusCode.Success)
            //{
            //    this.Document.Prompt("Draw cutting line start point.");
            //    gPoint p1;
            //    if(this.Document.ActionUtility.getUserPoint(out p1) == StatusCode.Success)
            //    {
            //        this.Document.Prompt("Draw cutting line end point.");
            //        gPoint p2;
            //        if(this.Document.ActionUtility.getUserRefPoint(p1, out p2) == StatusCode.Success)
            //        {
            //            this.Document.Prompt("Cutting Start(s), End(e).");
            //            string dir = this.Document.ActionUtility.getUserString();
            //            if (string.IsNullOrEmpty(dir) == false)
            //            {
            //                HLineWidth hLine = fig1 as HLineWidth;
            //                Geometry.Line line = new Geometry.Line(p1, p2);
            //                if (dir == "s")
            //                    hLine.StartCutLine = line;
            //                else if (dir == "e")
            //                    hLine.EndCutLine = line;
            //                else
            //                    return;
            //                hLine.Update();
            //                this.BizDrawCtrl.Redraw();
            //            }
            //        }
            //    }
            //}
        }

        private void BizDrawCtrl_EventHandlerCommandFinished(object sender, object listFigure, string command)
        {
        
        }
        
        private void BizDrawCtrl_EventHandlerBeforeModifyFigure(vdFigure figure, string propertyName, object newValue, ref bool cancel)
        {
            switch (propertyName)
            {
                case "ShowGrips":
                    break;
                case "Deleted":
                    cancel = false;
                    break;
                case "StartPoint": // Move시 같은 figure가 StartPoint, EndPoint로 두번 들어옴 // Rotate도 마찬가지
                    cancel = false;
                    break;
                case "EndPoint":
                    cancel = false;
                    break;
                default:
                    break;
            }
        }

        private void BizDrawCtrl_EventHandlerAfterModifyFigure(vdFigure figure, string propertyName)
        {
            switch (propertyName)
            {
                case "ShowGrips":
                    break;
                case "Deleted":
                    break;
                case "StartPoint": // Move시 같은 figure가 StartPoint, EndPoint로 두번 들어옴
                    break;
                case "EndPoint":
                    break;
                default:
                    break;
            }
        }

        private void MenuItemTest_Click(object sender, EventArgs e)
        {
            vdDocument doc = Converter.ToVDF(Document);
            Hicom.BizDraw.DrawControls.ExportPreviewFrom form = new DrawControls.ExportPreviewFrom(doc.ToStream());
            if(form.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void BizDrawCtrl_EventHandlerMouseDoubleClick(object sender, MouseEventArgs e, ref bool continuous)
        {
            if(sender is vdDimension)
            {
                //BizDrawCtrl.ExecuteCommand(Command.Command.DOUBLECLICK_DEVELOPER, "s");
                BizDrawCtrl.ExecuteCommand(Command.Command.DOUBLECLICK);
                continuous = false;
            }
        }

        private void MenuItemNew_Click(object sender, EventArgs e)
        {
            BizDrawCtrl.NewDocument();
        }

        private void MenuItemArea_Click(object sender, EventArgs e)
        {
            Document.Prompt("Select Entity.");
            vdFigure retfig;
            gPoint retpt;
            StatusCode scode = Document.ActionUtility.getUserEntityOneClick(out retfig, out retpt, vdDocument.LockLayerMethodEnum.Default, true);
            if(scode == StatusCode.Success)
            {
                if(retfig is vdPolyline)
                {
                    vdPolyline polyline = retfig as vdPolyline;
                    vdCircle circle = new vdCircle();
                    
                    double area = polyline.Area();
                }
            }
            
        }
    }
}
