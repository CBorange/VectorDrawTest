using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.Actions;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;
using VectorDraw.Actions;

using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;
using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.Command
{
    public partial class RunCommand
    {
        #region Event delegate
        public delegate void WriteCommdLineEventHandler(string commandLine);
        public delegate void ProgressEventHandler(bool show);
        #endregion

        #region EventHandler
        public event WriteCommdLineEventHandler EventHandlerWriteCommdLine;
        public event ProgressEventHandler EventHandlerProgress;
        #endregion

        
        public string CommandParam { get; set; }
        public string CommandText { get; set; }
        public List<object> Figures { get; set; }
        private vdDocument Document { get; set; }
        private DrawDimension.DimensionOptions DimOptions { get; set; }
        private List<string> RepeatCommand { get; set; }

        private RunCommand()
        {
            this.Figures = new List<object>();
            this.RepeatCommand = new List<string>();
            this.CommandText = string.Empty;
            this.CommandParam = string.Empty;
        }

        public RunCommand(vdDocument doc, DrawDimension.DimensionOptions opt) : this()
        {
            this.Document = doc;
            this.DimOptions = opt;
            this.Initialize(doc);
        }

        public void Initialize(vdDocument doc)
        {
            Document = doc;
            this.AddLayer(CommonActionString.ACTION_REF_LINE, new vdColor(Color.DarkGray), "DOT0", VdConstLineWeight.LW_20, false, false, true, false);
            this.AddLayer(CommonActionString.ACTION_STRAIGHT_DIM, new vdColor(Color.DarkGray), "DOT0", VdConstLineWeight.LW_20, false, false, true, false);
            this.AddLayer(CommonActionString.ACTION_ANGLE_DIM, new vdColor(Color.DarkGray), "DOT0", VdConstLineWeight.LW_20, false, false, true, false);
            this.AddLayer(CommonActionString.ACTION_TEXT, new vdColor(Color.DarkGray), "DOT0", VdConstLineWeight.LW_20, false, false, true, false);
            this.AddTextStyle(CommonActionString.ACTION_DIM_TEXTSTYLE, "Arial", 120);
            vdTextstyle style = Document.TextStyles.FindName("STANDARD");
            if(style != null) style.Height = 100;
        }

        public bool AddRepeatCommand(string command)
        {
            if (this.RepeatCommand.Contains(command.ToLower()) == false)
            {
                this.RepeatCommand.Add(command);
                return true;
            }
            return false;
        }

        public bool RemoveRepeatCommand(string command)
        {
            return this.RepeatCommand.Remove(command.ToLower());
        }

        private bool IsRepeatCommand(string command)
        {
            List<string> result = this.RepeatCommand.Where(item => item == command.ToLower()).ToList();
            return result.Count > 0 ? true : false;
        }

        public void ActionDraw(ActionBase action)
        {
            action.DrawReference(action.OrthoPoint, action.Render);
        }

        public void CommandFinish()
        {
            this.Figures.Clear();
            this.CommandText = string.Empty;
            this.CommandParam = string.Empty;
            this.Document.Prompt("Command:");
            this.Document.Redraw(true);
        }

        private bool AddLayer(string name, vdColor color, string lineType, VdConstLineWeight lineWeight, bool bFrozen = false, bool bLock = false, bool bOn = true, bool bVisible = true)
        {
            if (this.Document.Layers.FindName(name) == null)
            {
                vdLayer layer = new vdLayer(this.Document, name);
                layer.PenColor = color;
                layer.LineType = this.Document.LineTypes.FindName(lineType);
                layer.LineWeight = lineWeight;
                layer.Frozen = bFrozen;
                layer.Lock = bLock;
                layer.On = bOn;
                layer.VisibleOnForms = bVisible;
                this.Document.Layers.AddItem(layer);
            }
            return false;
        }

        private bool AddTextStyle(string name, string font, double height, bool bold = false, bool italic = false, bool underline = false, bool strikeOut = false, bool overline = false)
        {
            if (this.Document.TextStyles.FindName(name) == null)
            {
                vdTextstyle style = new vdTextstyle(this.Document, name);
                style.FontFile = font;
                style.Height = height;
                style.Bold = bold;
                style.IsItalic = italic;
                style.IsUnderLine = underline;
                style.IsStrikeOut = strikeOut;
                style.IsOverLine = overline;
                style.VisibleOnForms = true;
                this.Document.TextStyles.AddItem(style);
            }
            return false;
        }

        private void WriteCommandTextLine(string text)
        {
            if (this.EventHandlerWriteCommdLine != null)
                this.EventHandlerWriteCommdLine(text);
        }

        public bool Run(string command, vdDocument doc)
        {
            this.CommandText = command;
            bool repeat = this.IsRepeatCommand(command);
            bool result = true;
            switch (this.CommandText.ToLower())
            {
                #region File
                case Command.NEW:
                    doc.New();
                    break;
                case Command.OPEN:
                    result = this.CmdOpen();
                    break;
                case Command.SAVE:
                    result = this.CmdSave();
                    break;
                case Command.SAVEAS:
                    result = this.CmdSave(true);
                    break;
                #endregion
                #region Edit
                case Command.OSNAP:
                case Command.OSNAP_END:
                case Command.OSNAP_MID:
                case Command.OSNAP_CEN:
                case Command.OSNAP_NEA:
                    this.CmdOsnap(command);
                    break;
                case Command.DIST:
                    List<string> messages = CmdDistance.Run(this.Document, this.DimOptions);
                    for(int ix = 0; ix < messages.Count; ix++)
                        this.WriteCommandTextLine(messages[ix]);
                    break;
                #endregion
                #region Draw
                case Command.LINE:
                    this.Figures.AddRange(CmdLine.Run(this.Document, repeat));
                    break;
                case Command.CIRCLE:
                    this.Figures.AddRange(CmdCircle.Run(this.Document, CmdCircle.eMethod.SelectMethod));
                    break;
                case Command.CIRCLE_3P:
                    this.Figures.AddRange(CmdCircle.Run(this.Document, CmdCircle.eMethod.Pick3Point));
                    break;
                case Command.CIRCLE_CEN:
                    this.Figures.AddRange(CmdCircle.Run(this.Document, CmdCircle.eMethod.PickCenterPoint));
                    break;
                case Command.CIRCLE_2P:
                    this.Figures.AddRange(CmdCircle.Run(this.Document, CmdCircle.eMethod.Pick2Point));
                    break;
                case Command.ARC:
                    this.Figures.AddRange(CmdArc.Run(this.Document, CmdArc.eArcMethod.SelectMethod));
                    break;
                case Command.ARC_3P:
                    this.Figures.AddRange(CmdArc.Run(this.Document, CmdArc.eArcMethod.Pick3PointOnArc));
                    break;
                case Command.ARC_3PC:
                    this.Figures.AddRange(CmdArc.Run(this.Document, CmdArc.eArcMethod.PickCenterPoint));
                    break;
                case Command.ARC_2PA:
                    this.Figures.AddRange(CmdArc.Run(this.Document, CmdArc.eArcMethod.Pick2PointAngle));
                    break;
                case Command.RECTANG:
                    this.Figures.AddRange(CmdRectang.Run(this.Document));
                    break;
                case Command.POLYLINE:
                    this.Figures.AddRange(CmdPolyline.Run(this.Document));
                    break;
                case Command.LEADER:
                    this.Figures.AddRange(CmdLeader.Run(this.Document, this.DimOptions));
                    break;
                case Command.TEXT:
                    this.Figures.AddRange(CmdText.Run(this.Document));
                    break;
                case Command.MTEXT:
                    this.Figures.AddRange(CmdText.Run(this.Document, true));
                    break;
                #endregion
                #region Dimension
                case Command.DIM_ALIGNED:
                    this.Figures.AddRange(CmdDimension.Run(this.Document, CmdDimension.eMethod.Aligned, this.DimOptions));
                    break;
                case Command.DIM_VERTICAL:
                    this.Figures.AddRange(CmdDimension.Run(this.Document, CmdDimension.eMethod.Vertical, this.DimOptions));
                    break;
                case Command.DIM_HORIZONTAL:
                    this.Figures.AddRange(CmdDimension.Run(this.Document, CmdDimension.eMethod.Horizontal, this.DimOptions));
                    break;
                case Command.DIM_ANGULAR:
                    this.Figures.AddRange(CmdDimension.Run(this.Document, CmdDimension.eMethod.Angular, this.DimOptions));
                    break;
                case Command.DIM_SCALE:
                    this.CmdDimScale();
                    break;
                #endregion
                #region Modify
                case Command.MOVE:
                    this.CmdMove();
                    break; ;
                case Command.COPY:
                    this.CmdCopy();
                    break;
                case Command.DELETE_DEVELOPER:
                case Command.DELETE:
                    this.CmdDelete();
                    break;
                case Command.ROTATE:
                    this.CmdRotate();
                    break;
                case Command.EXTEND:
                    vdSelection boundSelection = GetSelectionOrUserSelection();
                    if (boundSelection != null && boundSelection.Count > 0)
                        this.Figures.AddRange(CmdExtendTrim.Run(doc, boundSelection, true, repeat));
                    break;
                case Command.TRIM:
                    boundSelection = GetSelectionOrUserSelection();
                    if (boundSelection != null && boundSelection.Count > 0)
                        this.Figures.AddRange(CmdExtendTrim.Run(doc, boundSelection, false, repeat));
                    break;
                case Command.EXPLODE:
                    this.CmdExplode();
                    break;
                case Command.EXPLODEALL:
                    this.CmdExplode(true);
                    break;
                case Command.BLOCK:
                    this.CmdBlock();
                    break;
                case Command.BLOCK_WRITE:
                    // 해당 부분만 추출하여 Block으로 Export 하는 기능
                    doc.CommandAction.CmdWriteBlock(null, null, null);
                    break;
                case Command.INSERT:
                    this.CmdInsert();
                    break;
                case Command.DOUBLECLICK:
                    this.Figures.AddRange(CmdDoubleClick.Run(doc, CommandParam));
                    break;
                #endregion
                #region Etc
                case Command.ORTHO:
                    this.CmdOrtho();
                    break;
                #endregion
                //case "linewidth":
                //    this.Figures.AddRange(CmdLineWidth.Run(this.Document, repeat));
                //    break;
                case "xcopy":
                    BizCommand.CmdXCopy(Document);
                    break;
                case "xmerge":
                    BizCommand.CmdXMerge(Document);
                    this.Figures.AddRange(CmdSelectionMerge.SelectionMerge.Figures);
                    CmdSelectionMerge.SelectionMerge.Figures.Clear();
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
        
        #region File
        /// <summary>
        /// Dialog를 이용한 파일열기 기능
        /// </summary>
        /// <returns></returns>
        private bool CmdOpen()
        {
            bool ret = true;

            EventHandlerProgress(true);
            object openFileName = Document.GetOpenFileNameDlg(0, "", 0);
            if (openFileName != null)
            {
                ret = Document.TryOpen(openFileName.ToString());
            }
            else
                ret = false;

            if (ret)
                Document.Redraw(true);

            EventHandlerProgress(false);
            return ret;
        }

        /// <summary>
        /// 작업한 내용을 저장하는 기능 (Save As시 Dialog)
        /// </summary>
        /// <param name="saveas">다른이름으로 저장</param>
        /// <returns></returns>
        private bool CmdSave(bool saveas = false)
        {
            bool ret = false;

            string version = string.Empty;
            string dirName = string.IsNullOrEmpty(this.Document.FileName) ? string.Empty : Path.GetDirectoryName(this.Document.FileName);
            if (dirName == string.Empty)
                saveas = true;
            string saveFileName = saveas ? this.Document.GetSaveFileNameDlg(this.Document.FileName, out version) : this.Document.FileName;
            if (!string.IsNullOrEmpty(saveFileName) && !string.IsNullOrWhiteSpace(saveFileName))
            {
                Image image = GetSaveImage();
                string extension = Path.GetExtension(saveFileName).ToLowerInvariant();
                switch(extension)
                {
                    case ".dxf":
                    case ".dwg":
                        ret = ConvertSaveas(saveFileName, image, version);
                        break;
                    default:
                        ret = this.Document.SaveAs(saveFileName, image, version);
                        break;
                }
            }

            return ret;
        }
        #endregion   
        private Image GetSaveImage()
        {
            Bitmap image = new Bitmap(250, 250);
            Graphics graphics = Graphics.FromImage(image);
            Document.ActiveLayOut.RenderToGraphics(graphics, null, image.Width, image.Height);
            graphics.Dispose();
            return image;
        }

        private bool ConvertSaveas(string filePath, Image image, string version)
        {
            bool result = false;

            MemoryStream stream = Document.ToStream();

            VectorDraw.Professional.Components.vdDocumentComponent comp = new VectorDraw.Professional.Components.vdDocumentComponent();
            comp.Document.LoadFromMemory(stream);
            
            Converter.ToVDF(comp.Document.ActiveLayOut);

            if (!string.IsNullOrEmpty(filePath))
            {
                result = comp.Document.SaveAs(filePath, image, version);
            }
            Document.Redraw(true);

            stream.Close();
            stream.Dispose();
            return result;
        }
        #region Edit
        /// <summary>
        /// Osnamp모드(켜기&끄기) 조정하는 기능
        /// 끝점, 중간점, 중앙(원, 호), 근접점, 수직점, 교차점, 확장선
        /// </summary>
        /// <param name="command">명령어</param>
        /// <returns></returns>
        private void CmdOsnap(string command = Command.OSNAP)
        {
            string strOsnapMode = string.Empty;

            if (string.Equals(command, Command.OSNAP, StringComparison.OrdinalIgnoreCase))
            {
                this.WriteCommandTextLine(string.Format("Current OsnapMode : {0}", this.Document.osnapMode));

                this.Document.Prompt("Osnap [End(e) Mid(m) Cen(c) Near(n) Perpendicular(p) Intersection(i) Extension(x)]");
                string end = string.Format("{0};e;", Command.OSNAP_END);
                string mid = string.Format("{0};m;", Command.OSNAP_MID);
                string cen = string.Format("{0};c;", Command.OSNAP_CEN);
                string nea = string.Format("{0};n;", Command.OSNAP_NEA);
                string per = string.Format("{0};p;", Command.OSNAP_PER);
                string inters = string.Format("{0};i;", Command.OSNAP_INTERS);
                string extension = string.Format("{0};x;", Command.OSNAP_EXTENSION);
                this.Document.ActionUtility.SetAcceptedStringValues(new string[] { end, mid, cen, nea, per, inters, extension }, Command.OSNAP_END);
                command = this.Document.ActionUtility.getUserString();
            }

            if (!string.IsNullOrEmpty(command))
            {
                OsnapMode osnapMode = OsnapMode.NONE;
                switch (command.ToLower())
                {
                    case Command.OSNAP_ALL:
                        this.Document.osnapMode = (OsnapMode.END | OsnapMode.MID | OsnapMode.CEN | OsnapMode.NEA | OsnapMode.PER | OsnapMode.INTERS | OsnapMode.EXTENSION);
                        break;
                    case Command.OSNAP_NON:
                        this.Document.osnapMode = OsnapMode.NONE;
                        break;
                    case Command.OSNAP_END:
                        this.Document.osnapMode ^= OsnapMode.END;
                        break;
                    case Command.OSNAP_MID:
                        this.Document.osnapMode ^= OsnapMode.MID;
                        break;
                    case Command.OSNAP_CEN:
                        this.Document.osnapMode ^= OsnapMode.CEN;
                        break;
                    case Command.OSNAP_NEA:
                        this.Document.osnapMode ^= OsnapMode.NEA;
                        break;
                    case Command.OSNAP_PER:
                        this.Document.osnapMode ^= OsnapMode.PER;
                        break;
                    case Command.OSNAP_INTERS:
                        this.Document.osnapMode ^= OsnapMode.INTERS;
                        break;
                    case Command.OSNAP_EXTENSION:
                        this.Document.osnapMode ^= OsnapMode.EXTENSION;
                        break;
                }
                this.Document.osnapMode ^= osnapMode;
            }
        }
        #endregion        

        #region Modify
        private void CmdDimScale()
        {
            double scale = this.DimOptions.ScaleFactor;
            this.Document.Prompt(string.Concat("Dimension scale(", this.DimOptions.ScaleFactor, ")"));
            StatusCode status = this.Document.ActionUtility.getUserDouble(out scale);
            if (status == StatusCode.Success)
            {
                this.DimOptions.ScaleFactor = scale;
            }
        }

        /// <summary>
        /// 도면내 선택된 Entities를 가져오는 기능
        /// </summary>
        /// <returns></returns>
        private vdSelection GetSelectionEntities()
        {
            StringBuilder sbSelectionName = new StringBuilder("VDGRIPSET_");
            sbSelectionName.Append(this.Document.ActiveLayOut.Handle.ToStringValue());
            if (this.Document.ActiveLayOut.ActiveViewPort != null)
                sbSelectionName.Append(this.Document.ActiveLayOut.ActiveViewPort.Handle.ToString());

            vdSelection selection = this.Document.Selections.FindName(sbSelectionName.ToString());
            if (selection != null)
            {
                for (int ix = selection.Count - 1; ix > -1; ix--)
                {
                    vdFigure figure = selection[ix];
                    // Entity 선택조건
                    if (figure.IsVisible() == false)
                        selection.RemoveAt(ix);
                }
            }

            return selection;
        }

        /// <summary>
        /// 도면내 Entities를 선택하고 선택한 Entities를 가져오는 기능
        /// </summary>
        /// <returns></returns>
        private vdSelection GetUserSelectionEntities()
        {
            this.Document.Prompt("Select Entities");
            vdSelection selection = this.Document.ActionUtility.getUserSelection();
            if (selection != null)
            {
                for (int ix = selection.Count - 1; ix > -1; ix--)
                {
                    vdFigure figure = selection[ix];
                    // Entity 선택조건
                    if (figure.IsVisible() == false)
                        selection.RemoveAt(ix);
                }
            }

            return selection;
        }

        /// <summary>
        /// 도면내 선택된 Entities를 가져오고 없으면 Entities를 선택하여 가져오는 기능
        /// </summary>
        /// <returns></returns>
        private vdSelection GetSelectionOrUserSelection()
        {
            vdSelection selection = this.GetSelectionEntities();
            if (selection == null || selection.Count < 1)
                selection = this.GetUserSelectionEntities();
            return selection;
        }

        /// <summary>
        /// 선택된 Entities를 이동시키는 기능
        /// </summary>
        private void CmdMove()
        {
            vdSelection selection = GetSelectionOrUserSelection();
            if (selection != null && selection.Count > 0)
            {
                if (this.Document.CommandAction.CmdMove(selection, null, null))
                {
                    for (int ix = 0; ix < selection.Count; ix++)
                    {
                        vdFigure figure = selection[ix];
                        this.Figures.Add(figure);
                    }
                }
            }
        }

        /// <summary>
        /// 선택된 Entities를 복사하는 기능
        /// </summary>
        private void CmdCopy()
        {
            vdSelection selection = GetSelectionOrUserSelection();
            if (selection != null && selection.Count > 0)
            {
                int beforCopyEntitiesCount = this.Document.ActiveLayOut.Entities.Count;
                if (this.Document.CommandAction.CmdCopy(selection, null, null))
                {
                    int afterCopyEntitiesCount = this.Document.ActiveLayOut.Entities.Count;
                    for (int i = (afterCopyEntitiesCount - 1); i > (beforCopyEntitiesCount - 1); i--)
                    {
                        vdFigure fig = this.Document.ActiveLayOut.Entities[i];
                        this.Figures.Add(fig);
                    }
                }
            }
        }
        
        /// <summary>
        /// 선택된 Entities를 삭제하는 기능
        /// </summary>
        private void CmdDelete()
        {
            vdSelection selection = GetSelectionOrUserSelection();
            if (selection != null && selection.Count > 0)
            {
                for (int ix = 0; ix < selection.Count; ix++)
                {
                    vdFigure figure = selection[ix];
                    if(figure != null)
                    {
                        figure.Deleted = true; // Modify Event 실행후 figure.Delete 변경됨
                        if (figure.Deleted) // Modify Event에서 Cancel 했을 경우 삭제되지 않음.
                        {
                            figure.Invalidate();
                            figure.Update();
                            this.Document.ActiveLayOut.Entities.RemoveItem(figure);
                            this.Figures.Add(figure); // 삭제된 Entity를 리턴
                        }
                    }
                }
                selection.ShowGrips(false);
                selection.RemoveAll();
            }
        }
        
        /// <summary>
        /// 선택된 Entities를 회전시키는 기능
        /// </summary>
        private void CmdRotate()
        {
            vdSelection selection = GetSelectionOrUserSelection();
            if (selection != null && selection.Count > 0)
            {
                this.Document.CommandAction.CmdRotate(selection, null, null);
                for (int ix = 0; ix < selection.Count; ix++)
                {
                    vdFigure figure = selection[ix];
                    this.Figures.Add(figure);
                }
            }
        }

        /// <summary>
        /// Block, Rectangle, PolyLine 등 분해가능한 Entity를 1번 분해하는 기능
        /// </summary>
        /// <param name="allEntities">전체 Entities 분해</param>
        private void CmdExplode(bool allEntities = false)
        {
            vdSelection selection = allEntities ? new vdSelection() : GetSelectionOrUserSelection();
            
            if (allEntities)
                selection.AddRange(this.Document.ActiveLayOut.Entities, vdSelection.AddItemCheck.RemoveInVisibleAndLockLayer);
            
            if (selection != null && selection.Count > 0)
            {
                for (int ix = 0; ix < selection.Count; ix++)
                {
                    vdFigure fig = selection[ix];
                    vdEntities entities = fig.Explode();
                    if (entities != null && entities.Count > 0)
                    {
                        fig.Invalidate();
                        fig.Deleted = true;
                        fig.Update();
                        this.Document.ActiveLayOut.Entities.RemoveItem(fig);
                        for (int iy = 0; iy < entities.Count; iy++)
                        {
                            this.Document.ActiveLayOut.Entities.AddItem(entities[iy]);
                            this.Document.ActionDrawFigure(entities[iy]);
                            this.Figures.Add(entities[iy]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Block을 생성하는 기능 (this.Figures는 Block을 생성하기 위해 포함되었던 Figure들)
        /// </summary>
        private void CmdBlock()
        {
            this.Document.Prompt("BlockName Input");
            string blockName = this.Document.ActionUtility.getUserString();
            if (string.IsNullOrEmpty(blockName))
                return;

            gPoint insertPoint = null;
            if (this.Document.ActionUtility.getUserPoint(out insertPoint) != StatusCode.Success)
                return;

            vdSelection selection =  this.GetSelectionOrUserSelection();
            if (selection == null || selection.Count < 1)
                return;

            if (this.Document.CommandAction.CmdMakeBlock(blockName, insertPoint, selection))
            {
                for (int ix = 0; ix < selection.Count; ix++)
                {
                    this.Figures.Add(selection[ix]);
                }
            }
            else
            {
                this.WriteCommandTextLine("Create Block Fail");
            }
        }

        /// <summary>
        /// Block 등 Insert Dialog를 Show
        /// </summary>
        private void CmdInsert()
        {
            if(this.Document.CommandAction.CmdInsertBlockDialog())
            //if (this.Document.CommandAction.CmdInsert(null, null, null, null, null))
            {
                this.Figures.Add(this.Document.ActiveLayOut.Entities.Last);
            }
        }
        #endregion

        #region Etc
        /// <summary>
        /// 직교 기능 켜기&끄기
        /// </summary>
        /// <returns></returns>
        public void CmdOrtho()
        {
            this.Document.OrthoMode = !this.Document.OrthoMode;
            this.WriteCommandTextLine(string.Format("Ortho : {0}", this.Document.OrthoMode ? "On" : "Off"));
        }
        #endregion
    }
}
