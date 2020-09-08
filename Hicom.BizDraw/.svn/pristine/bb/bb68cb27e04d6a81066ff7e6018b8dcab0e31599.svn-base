using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VectorDraw.WinMessages;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdCommandLine;
using VectorDraw.Professional.Control;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.Actions;
using VectorDraw.Generics;
using VectorDraw.Geometry;
using VectorDraw.Actions;

using Hicom.BizDraw.Command;
using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;

namespace Hicom.BizDraw.DrawControls
{
    public partial class BizDrawCtrl : UserControl
    {
        #region Event
        #region Event delegate
        public delegate void CommandStartEventHandler(string commandname, bool isDefaultImplemented, ref bool success, ref bool continuous, ref List<object> figures);
        public delegate void CommandFinishedEventHandler(object sender, object listFigure, string command);
        public delegate void ProcessCmdKeyEventHandler(ref Message msg, Keys keyData, ref bool continuous);
        public delegate void BeforeModifyFigureEventHandler(vdFigure figure, string propertyName, object newValue, ref bool cancel);
        public delegate void AfterModifyFigureEventHandler(vdFigure figure, string propertyName);
        public delegate void MouseDoubleClickEventHandler(object sender, MouseEventArgs e, ref bool continuous);
        public delegate void AfterNewDocumentEventHandler(object sender);
        #endregion

        #region EventHandler
        [Category("BizDrawCtrl")]
        public event CommandStartEventHandler EventHandlerCommandStart;
        [Category("BizDrawCtrl")]
        public event CommandFinishedEventHandler EventHandlerCommandFinished;
        [Category("BizDrawCtrl")]
        public event ProcessCmdKeyEventHandler EventHandlerProcessCmdKey;
        [Category("BizDrawCtrl")]
        public event BeforeModifyFigureEventHandler EventHandlerBeforeModifyFigure;
        [Category("BizDrawCtrl")]
        public event AfterModifyFigureEventHandler EventHandlerAfterModifyFigure;
        [Category("BizDrawCtrl")]
        public event MouseDoubleClickEventHandler EventHandlerMouseDoubleClick;
        [Category("BizDdrawCtrl")]
        public event AfterNewDocumentEventHandler EventHandlerAfterNewDocument;
        #endregion
        #endregion
        #region Control Show/Hide
        private bool IsShowCommand = true;
        [Description("Show or Hide command control"), Category("BizDrawCtrl")]
        public bool ShowCommand
        {
            get
            {
                return IsShowCommand;
            }
            set
            {
                this.IsShowCommand = value;
                if (this.IsShowCommand)
                {
                    this.vdCommandLine.Show();
                    this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                }
                else
                {
                    this.vdCommandLine.Hide();
                    this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                }
            }
        }

        private bool IsShowProperty = true;
        [Description("Show or Hide property control"), Category("BizDrawCtrl")]
        public bool ShowProperty
        {
            get
            {
                return this.IsShowProperty;
            }
            set
            {
                this.IsShowProperty = value;
                if (this.IsShowProperty)
                {
                    this.vdPropertyGrid.Show();
                    this.splitContainerControl2.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                    this.FillPropertyGrid(this.ActiveDocument);
                }
                else
                {
                    this.vdPropertyGrid.Hide();
                    this.splitContainerControl2.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                }
            }
        }
        #endregion

        [Description("ActiveDocument"), Category("BizDrawCtrl")]
        public vdDocument ActiveDocument { get { return this.vdScrollableControl.BaseControl.ActiveDocument; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OsnapMode OsnapMode
        {
            get
            {
                return this.ActiveDocument.osnapMode;
            }
            set
            {
                if (this.vdScrollableControl != null && this.vdScrollableControl.BaseControl != null && this.vdScrollableControl.BaseControl.ActiveDocument != null)
                    this.ActiveDocument.osnapMode = value;
            }
        }

        [Description("ActiveDocument"), Category("BizDrawCtrl")]
        public bool ShowProgress { get; set; }
        [DefaultValue(true), Description("ActiveDocument"), Category("BizDrawCtrl")]
        public bool ViewerMode { get; set; }

        public DrawHandler DrawHandler { get { return this._drawHandler; } }
        public ContextMenu DrawContextMenu { get { return this.vdScrollableControl.BaseControl.ContextMenu; } }

        private RunCommand _command;
        private ProgressForm _progressForm;
        private DrawHandler _drawHandler;
        private TextCtrl _textCtrl;
        protected VectorDrawBaseControl DrawBaseControl
        {
            get
            {
                return vdScrollableControl.BaseControl;
            }
        }

        public BizDrawCtrl()
        {
            InitializeComponent();
        }
        
        public void Initialize()
        {
            string filePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            bool load = vdCommandLine.LoadCommands(filePath, "Commands.txt");
            vdCommandLine.SelectDocument(this.ActiveDocument);
            this.vdCommandLine.Focus();
            this.ShowProgress = false;
            this.ActiveDocument.FileName = "Untitled";
        }

        protected virtual void BizDrawCtrl_Load(object sender, EventArgs e)
        {
            InitializeVectorDraw();
        }

        protected void InitializeVectorDraw()
        {
            _textCtrl = new TextCtrl();
            _textCtrl.EventFinished += new EventHandler(EventTextFinished);
            _textCtrl.Dock = DockStyle.Bottom;
            this.vdScrollableControl.BaseControl.Controls.Add(_textCtrl);
            _textCtrl.Visible = false;

            this.ActiveDocument.FileName = "Untitled";
            this.ActiveDocument.OsnapModePreserve = true;

            BizDrawContextMenuHandler contextMenuHandler = new BizDrawContextMenuHandler(vdScrollableControl.BaseControl, vdCommandLine, ActiveDocument);
            this.vdScrollableControl.BaseControl.ContextMenu = contextMenuHandler.BizDrawContextMenu;

            _drawHandler = new DrawHandler(ActiveDocument);
            _command = new RunCommand(ActiveDocument, _drawHandler.Dimension.Options);
            _progressForm = new ProgressForm();

            _command.EventHandlerWriteCommdLine += new RunCommand.WriteCommdLineEventHandler(AddpendCommandText);
            _command.EventHandlerProgress += new RunCommand.ProgressEventHandler(EventHandlerProgress);

            vdCommandLine.CommandExecute += new CommandExecuteEventHandler(EventCommandExecute);
            vdCommandLine.UserText.TextChanged += new EventHandler(UserText_TextChanged);

            this.ActiveDocument.OnActionDraw += new vdDocument.ActionDrawEventHandler(ActiveDocument_OnActionDraw);
            this.ActiveDocument.OnBeforeModifyObject += new vdDocument.BeforeModifyObjectEventHandler(ActiveDocument_OnBeforeModifyObject);
            this.ActiveDocument.OnAfterModifyObject += new vdDocument.AfterModifyObjectEventHandler(ActiveDocument_OnAfterModifyObject);
            this.ActiveDocument.FreezeEntityDrawEvents.Push(false);
            this.ActiveDocument.GlobalRenderProperties.SelectionPreview = VectorDraw.Render.vdRenderGlobalProperties.SelectionPreviewFlags.ON;
            this.ActiveDocument.GlobalRenderProperties.SelectionPreviewColor = Color.Transparent;
            this.ActiveDocument.OnProgressStart += new VectorDraw.Professional.Utilities.ProgressStartEventHandler(ActiveDocument_OnProgressStart);
            this.ActiveDocument.OnProgress += new VectorDraw.Professional.Utilities.ProgressEventHandler(ActiveDocument_OnProgress);
            this.ActiveDocument.OnProgressStop += new VectorDraw.Professional.Utilities.ProgressStopEventHandler(ActiveDocument_OnProgressStop);
            this.ActiveDocument.OnAfterNewDocument += new vdDocument.AfterNewDocument(BaseControl_AfterNewDocument);

            this.vdScrollableControl.BaseControl.vdDragEnter += new DragEnterEventHandler(BaseControl_DragEnter);
            this.vdScrollableControl.BaseControl.vdDragDrop += new DragDropEventHandler(BaseControl_DragDrop);
            this.vdScrollableControl.BaseControl.vdKeyDown += new KeyDownEventHandler(BaseControl_vdKeyDown);
            this.vdScrollableControl.BaseControl.GripSelectionModified += new GripSelectionModifiedEventHandler(BaseControl_GripSelectionModified);
            this.vdScrollableControl.BaseControl.ActionLayoutActivated += new ActionLayoutActivatedEventHandler(BaseControl_ActionLayoutActivated);
            this.vdScrollableControl.BaseControl.AfterOpenDocument += new AfterOpenDocumentEventHandler(BaseControl_AfterOpenDocument);
            this.vdScrollableControl.BaseControl.MouseDoubleClick += new MouseEventHandler(this.BaseControl_MouseDoubleClick);
            this.vdScrollableControl.BaseControl.MouseClick += new MouseEventHandler(BaseControl_MouseClick);

            this.FillPropertyGrid(this.ActiveDocument);
        }

        private void ActiveDocument_OnProgressStart(object sender, string jobDescription, long meterLimit)
        {
            if (ShowProgress)
                this._progressForm.ShowProgress(100);
        }

        private void ActiveDocument_OnProgress(object sender, long percent, string jobDescription)
        {
            if (ShowProgress)
                this._progressForm.SetProgress(percent);
        }

        private void ActiveDocument_OnProgressStop(object sender, string jobDescription)
        {
            if (ShowProgress)
                this._progressForm.HideProgress();
        }

        private void UserText_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            
            if (!string.IsNullOrEmpty(textBox.Text) && textBox.Text.Last().Equals(' '))
            {
                textBox.Text = textBox.Text.TrimEnd();

                MessageManager.MSG m = MessageManager.MSG.Create(textBox.Handle, (int)MessageManager.Messages.WM_KEYDOWN, (IntPtr)Keys.Enter, IntPtr.Zero);
                MessageManager.TranslateMessage(ref m);
                MessageManager.DispatchMessage(ref m);
            }
        }
        
        private void EventCommandExecute(string commandname, bool isDefaultImplemented, ref bool success)
        {
            if (ViewerMode)
                return;
            bool result = true;
            bool continuous = true;
            List<object> resultObject = new List<object>();

            if (EventHandlerCommandStart != null)
                EventHandlerCommandStart(commandname, isDefaultImplemented, ref success, ref continuous, ref resultObject);
            bool finished = true;
            if (continuous == true)
            {
                switch (commandname.ToLower())
                {
                    case Hicom.BizDraw.Command.Command.IMPORT:
                        List<vdFigure> import = this.CommandImport();
                        if (import != null)
                            resultObject.AddRange(import);
                        break;
                    case Hicom.BizDraw.Command.Command.OSNAP_WINDOW:
                        CommandOsnapW();
                        break;
                    case Hicom.BizDraw.Command.Command.LAYER:
                        CommandLayer();
                        break;
                    case Hicom.BizDraw.Command.Command.PROPERTY:
                        ShowProperty = !this.ShowProperty;
                        break;
                    case Hicom.BizDraw.Command.Command.DIM_OPTIONS:
                        DimensionOptions();
                        break;
                    case Command.Command.TEXT:
                        finished = CommandText();
                        break;
                    case Command.Command.MTEXT:
                        finished = CommandText(true);
                        break;
                    default:
                        result = _command.Run(commandname, this.ActiveDocument);
                        resultObject.AddRange(_command.Figures);
                        break;
                }

                success = result;
            }
            
            if(finished)
            {
                if (EventHandlerCommandFinished != null)
                    EventHandlerCommandFinished(ActiveDocument, resultObject, commandname);

                _command.CommandFinish();
            }
        }

        private void DimensionOptions()
        {
            DimensionOptionForm form = new DimensionOptionForm(_drawHandler.Dimension.Options);
            if(form.ShowDialog() == DialogResult.OK)
            {
                _drawHandler.Dimension.SetDimensionOptions(form.Options);

                vdSelection selection = GetSelection();
                foreach(vdFigure fig in selection)
                {
                    if(fig is vdDimension)
                    {
                        vdDimension dim = fig as vdDimension;
                        _drawHandler.Dimension.Options.ApplyOptions(dim);
                    }
                }
                if (selection.Count > 0)
                    Redraw();
            }
        }
        
        private void ActiveDocument_OnActionDraw(object sender, object action, bool isHideMode, ref bool cancel)
        {
            if (action != null && action is ActionBase)
            {
                ActionBase actionBase = action as ActionBase;
                this._command.ActionDraw(actionBase);
            }
        }

        private void ActiveDocument_OnBeforeModifyObject(object sender, string propertyname, object newvalue, ref bool cancel)
        {
            if (!ViewerMode && sender is vdFigure)
            {
                vdFigure figure = sender as vdFigure;
                if (this.EventHandlerBeforeModifyFigure != null)
                    this.EventHandlerBeforeModifyFigure(figure, propertyname, newvalue, ref cancel);
            }
        }

        private void ActiveDocument_OnAfterModifyObject(object sender, string propertyname)
        {
            if (sender is vdFigure)
            {
                vdFigure figure = sender as vdFigure;
                if (this.EventHandlerAfterModifyFigure != null)
                    this.EventHandlerAfterModifyFigure(figure, propertyname);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool continuous = true;
            if (this.EventHandlerProcessCmdKey != null)
                this.EventHandlerProcessCmdKey(ref msg, keyData, ref continuous);
            if (continuous == false)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            bool isRun = true;
            switch (keyData)
            {
                case Keys.Delete:
                    this.ExecuteCommand(Hicom.BizDraw.Command.Command.DELETE);
                    break;
                case Keys.F8:
                    this.CommandOrtho();
                    break;
                case Keys.Control | Keys.O:
                    this.ExecuteCommand(Hicom.BizDraw.Command.Command.OPEN);
                    break;
                case Keys.Control | Keys.S:
                    this.ExecuteCommand(Hicom.BizDraw.Command.Command.SAVE);
                    break;
                case Keys.Control | Keys.L:
                    this.ExecuteCommand(Hicom.BizDraw.Command.Command.LAYER);
                    break;
                case Keys.Control | Keys.P:
                    this.ExecuteCommand(Hicom.BizDraw.Command.Command.PRINT);
                    break;
                case Keys.Control | Keys.Z:
                    this.ExecuteCommand("undo");
                    break;
                case Keys.Control | Keys.Y:
                    this.ExecuteCommand("redo");
                    break;
                case Keys.Shift | Keys.Control | Keys.C:
                    this.ExecuteCommand(Command.Command.XCOPY);
                    break;
                case Keys.Shift | Keys.Control | Keys.V:
                    ExecuteCommand(Command.Command.XMerge);
                    break;
                default:
                    isRun = false;
                    break;
            }
            if (isRun == false)
                return base.ProcessCmdKey(ref msg, keyData);
            else
                return true;
        }

        public void AddpendCommandText(string commandLine)
        {
            vdCommandLine.History.AppendText(Environment.NewLine);
            vdCommandLine.History.AppendText(commandLine);
        }

        private void EventHandlerProgress(bool show)
        {
            ShowProgress = show;
        }

        private void BaseControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool continuous = true;
            if (EventHandlerMouseDoubleClick != null && e.Clicks == 2)
            {
                Point mousePt = ActiveDocument.ActiveLayOut.ActiveAction.GdiMousePos;
                vdFigure fig = ActiveDocument.ActiveLayOut.GetEntityFromPoint(mousePt, ActiveDocument.ActiveLayOut.Render.GlobalProperties.PickSize, false);
                EventHandlerMouseDoubleClick(fig, e, ref continuous);
            }
                
            if (continuous == false)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (e.Clicks == 2)
                    {
                        Point mousePt = ActiveDocument.ActiveLayOut.ActiveAction.GdiMousePos;
                        vdFigure fig = ActiveDocument.ActiveLayOut.GetEntityFromPoint(mousePt, ActiveDocument.ActiveLayOut.Render.GlobalProperties.PickSize, false);
                        //if(fig is vdText)
                        //{
                        //    _textCtrl.Show(ActiveDocument, fig as vdText);
                        //}
                        if((fig is vdText) || (fig is vdMText))
                        {
                            _textCtrl.Show(ActiveDocument, fig);
                        }
                        
                        //this.ExecuteCommand(Hicom.BizDraw.Command.Command.DOUBLECLICK);
                    }   
                    break;
                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    if (e.Clicks == 2)
                        this.ExecuteCommand(Hicom.BizDraw.Command.Command.ZOOM);
                    break;
                //case MouseButtons.XButton1:
                //    break;
                //case MouseButtons.XButton2:
                //    break;
                default:
                    break;
            }
        }

        private void BaseControl_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    break;
                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    break;
                //case MouseButtons.XButton1:
                //    break;
                //case MouseButtons.XButton2:
                //    break;
                default:
                    break;
            }
        }

        private bool CommandText(bool isMultiline = false)
        {
            bool finish = true;
            ActiveDocument.Prompt("Insertion point.");
            gPoint insert;
            StatusCode sCode = ActiveDocument.ActionUtility.getUserPoint(out insert);
            if(sCode == StatusCode.Success)
            {
                double textHeight = ActiveDocument.ActiveTextStyle.Height;                
                ActiveDocument.Prompt(string.Concat("Text Size(", textHeight, ")"));
                sCode = ActiveDocument.ActionUtility.getUserDouble(out textHeight);
                if (textHeight.AreEqual(0))
                    textHeight = ActiveDocument.ActiveTextStyle.Height;
                ActiveDocument.ActionUtility.SetAcceptedStringValues(new string[] { "Rotation" }, "0");
                ActiveDocument.Prompt("Rotation(0)");
                string rot = ActiveDocument.ActionUtility.getUserString();
                double rotation = 0;
                if (!string.IsNullOrEmpty(rot) && double.TryParse(rot, out rotation))
                {
                    ActiveDocument.Prompt("Input Text.");
                    finish = false;
                    _textCtrl.Show(ActiveDocument, insert, textHeight, rotation, isMultiline);
                }
            }
            return finish;
        }
                
        private void EventTextFinished(object sender, EventArgs e)
        {
            vdCommandLine.Focus();
            ActiveDocument.Prompt("Command:");
        }

        private string IsExistClipboard()
        {
            string tempPath = Path.GetTempPath();
            string[] files = Directory.GetFiles(tempPath, "A$*.dwg");

            DateTime time1 = new DateTime();
            string file = string.Empty;
            for ( int ix = 0; ix < files.Length; ++ix)
            {
                DateTime time2 = File.GetLastAccessTime(files[ix]);

                if ( time1 < time2 )
                {
                    time1 = time2;
                    file = files[ix];
                }
            }
            return file;
        }

        private void PasteAutoCad()
        {
            string clipboardFile = IsExistClipboard();

            if ( string.IsNullOrEmpty(clipboardFile) )
            {
                this.AddpendCommandText("Found not clipboard file");
            }
            else
            {
                ActiveDocument.Prompt("Pick insert point.");
                gPoint retpt = new gPoint();
                if (ActiveDocument.ActionUtility.getUserPoint(out retpt) == VectorDraw.Actions.StatusCode.Success)
                {
                    VectorDraw.Professional.Components.vdDocumentComponent doc = new VectorDraw.Professional.Components.vdDocumentComponent();
                    //if (doc.Document.Open(string.Concat(@"D:\Work\1.커튼월\일람표\궁동 창호 수정-160901(견적용).dwg").ToString()) == true)
                    if (doc.Document.TryOpen(clipboardFile) )
                    {
                        vdSelection selection = new vdSelection("section");
                        selection.SetUnRegisterDocument(doc.Document);

                        foreach (vdFigure fig in doc.Document.ActiveLayOut.Entities)
                            selection.AddItem(fig, false, vdSelection.AddItemCheck.Nochecking);
                        
                        Vector vec = new Vector(retpt - selection.GetBoundingBox().Min);

                        Matrix matrix = new Matrix();
                        matrix.TranslateMatrix(vec);
                        selection.Transformby(matrix);
                        selection.SetUnRegisterDocument(this.ActiveDocument);

                        this.ActiveDocument.MergeSelection(selection);
                        this.ActiveDocument.Redraw(true);
                    }

                }
                ActiveDocument.Prompt("Command:");
            }
        }

        private void BaseControl_vdKeyDown(KeyEventArgs e, ref bool cancel)
        {
            if ( e.KeyCode == Keys.V && e.Control )
            {
                PasteAutoCad();
                return;
            }

            if (e.KeyCode == Keys.F8)
                this.CommandOrtho();

            BaseAction action = this.vdScrollableControl.BaseControl.ActiveDocument.ActiveLayOut.OverAllActiveAction;
            if (!action.SendKeyEvents) return;

            vdCommandLine cl = this.vdCommandLine;
            if (cl.Visible == false) return;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) return;
            Message msg = new Message();
            msg.HWnd = cl.Handle;
            msg.Msg = (int)VectorDraw.WinMessages.MessageManager.Messages.WM_KEYDOWN;
            msg.WParam = (IntPtr)e.KeyCode;
            cl.vdProcessKeyMessage(ref msg);
        }

        private void BaseControl_DragEnter(DragEventArgs drgevent, ref bool cancel)
        {
            drgevent.Effect = DragDropEffects.Copy;
        }

        private void BaseControl_DragDrop(DragEventArgs drgevent, ref bool cancel)
        {

        }

        private void BaseControl_GripSelectionModified(object sender, vdLayout layout, vdSelection gripSelection)
        {
            if (gripSelection.Count == 0)
                FillPropertyGrid(this.ActiveDocument);
            else
                FillPropertyGrid(gripSelection);
        }

        private void BaseControl_ActionLayoutActivated(object sender, vdLayout deactivated, vdLayout activated)
        {
            FillPropertyGrid(this.ActiveDocument);
        }

        private void BaseControl_AfterNewDocument(object sender)
        {
            Initialize();
            OsnapMode = OsnapMode.ALL;//OsnapMode.END | OsnapMode.MID | OsnapMode.CEN | OsnapMode.INTERS | OsnapMode.EXTENSION;
            _command.Initialize(ActiveDocument);
            FillPropertyGrid(this.ActiveDocument);
        }

        private void BaseControl_AfterOpenDocument(object sender)
        {
            Initialize();
            _command.Initialize(ActiveDocument);
            DrawHandler.Initialize(ActiveDocument);
            OsnapMode = OsnapMode.ALL;//OsnapMode.END | OsnapMode.MID | OsnapMode.CEN | OsnapMode.INTERS | OsnapMode.EXTENSION;
            FillPropertyGrid(this.ActiveDocument);
        }

        public void PrintToCommandLine(string msg)
        {
            try
            {
                this.vdCommandLine.History.AppendText("\r\n\r\n");
                this.vdCommandLine.History.AppendText(msg);
            }catch(ObjectDisposedException e)
            {

            }
            
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            this.Dispose();
            base.OnHandleDestroyed(e);
        }
    }
}
