using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdCommandLine;
using VectorDraw.Professional.Control;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.Actions;
using VectorDraw.Generics;
using VectorDraw.Actions;
using VectorDraw.Geometry;

using Hicom.BizDraw.Command;
using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;

namespace Hicom.BizDraw.DrawControls
{
    public static class VDrawMethod
    {
        public static void ClearDocument(this vdDocument doc)
        {
            doc.ActiveLayOut.Entities.RemoveAll();
            doc.ActiveLayOut.Dispose();
            doc.ActiveLayOut = null;
            doc.LayOuts.RemoveAll();
            doc.ClearAll();
            doc.Dispose();
        }
    }

    public partial class BizDrawCtrl
    {
        #region File
        public virtual void NewDocument(string fileName = "untitle")
        {
            ActiveDocument.New();
            ActiveDocument.FileName = fileName;
            DrawHandler.Initialize(ActiveDocument);
            if (EventHandlerAfterNewDocument != null)
                EventHandlerAfterNewDocument(this);
        }

        public bool OpenDocument(string filePath)
        {
            //ShowProgress = true;
            bool open = this.ActiveDocument.TryOpen(filePath);
            if (open)
            {
                _command.Initialize(ActiveDocument);
                this.DrawHandler.Initialize(ActiveDocument);
            }
            //ShowProgress = false;
            return open;
        }

        public virtual bool OpenDocument(MemoryStream stream)
        {
            //ShowProgress = true;

            bool open = this.ActiveDocument.LoadFromMemory(stream);
            this.ActiveDocument.ActiveLayOut.DisableShowPrinterPaper = true; // vectordraw 8.008 이상일때 기본 값 변경해줌
            if (open)
            {
                //this.ActiveDocument.ActiveLayOut.
                this.DrawHandler.SetDocument(this.ActiveDocument);
            }
            //ShowProgress = false;
            return open;
        }

        public bool SaveDocument(string filePath)
        {
            return this.ActiveDocument.SaveAs(filePath);
        }
        #endregion

        #region Edit
        public void ZoomAll()
        {
            this.ExecuteCommand(BizDraw.Command.Command.ZOOM);
        }

        public void ZoomWindow()
        {
            this.ExecuteCommand(BizDraw.Command.Command.ZOOM_WINDOW);
        }

        public void ZoomWindow(gPoint p1, gPoint p2)
        {
            this.ActiveDocument.CommandAction.Zoom("w", p1, p2);
        }

        public void Redraw()
        {
            ActiveDocument.Redraw(true);
        }
        #endregion

        #region Format
        public void AddLayer(vdLayer layer)
        {
            if (this.ActiveDocument.Layers.FindName(layer.Name) != null)
                this.ActiveDocument.Layers.RemoveItem(layer);
            
            this.ActiveDocument.Layers.AddItem(layer);
        }

        public void AddLayer(string name, Color color, string lineType, VdConstLineWeight lineWeight, bool bFrozen = false, bool bLock = false, bool bOn = true, bool bVisible = true)
        {
            vdLayer layer = this.ActiveDocument.Layers.FindName(name);
            if (layer != null)
                this.ActiveDocument.Layers.RemoveItem(layer);
            
            layer = new vdLayer(this.ActiveDocument, name);
            layer.PenColor = new vdColor(color);
            layer.LineType = this.ActiveDocument.LineTypes.FindName(lineType);
            layer.LineWeight = lineWeight;
            layer.Frozen = bFrozen;
            layer.Lock = bLock;
            layer.On = bOn;
            layer.VisibleOnForms = bVisible;
            this.ActiveDocument.Layers.AddItem(layer);
        }

        public void AddTextStyle(string name, string font, double height, bool bold = false, bool italic = false, bool underline = false, bool strikeOut = false, bool overline = false)
        {
            vdTextstyle style = this.ActiveDocument.TextStyles.FindName(name);
            if (style != null)
                this.ActiveDocument.TextStyles.RemoveItem(style);
            
            style = new vdTextstyle(this.ActiveDocument, name);
            style.FontFile = font;
            style.Height = height;
            style.Bold = bold;
            style.IsItalic = italic;
            style.IsUnderLine = underline;
            style.IsStrikeOut = strikeOut;
            style.IsOverLine = overline;                
            this.ActiveDocument.TextStyles.AddItem(style);
        }

        public bool AddRepeatCommand(string command)
        {
            return this._command.AddRepeatCommand(command);
        }
        
        public bool RemoveRepeatCommand(string command)
        {
            return this._command.RemoveRepeatCommand(command);
        }

        public bool DelLayer(vdLayer layer)
        {
            if (layer != null)
                return this.ActiveDocument.Layers.RemoveItem(layer);
            else
                return false;
        }

        public bool DelLayer(string name)
        {
            return DelLayer(this.ActiveDocument.Layers.FindName(name));
        }
        #endregion

        public bool ExecuteCommand(string command, string param = "")
        {
            _command.CommandParam = param;
            
            return vdCommandLine.ExecuteCommand(command);
        }
        
        /// <summary>
        /// vdDocument를 MemoryStresm으로 변환
        /// </summary>
        public MemoryStream ToMemoryStream()
        {
            return this.ActiveDocument != null ? this.ActiveDocument.ToStream() : null;
        }

        public Image ExportToImage(int width, int height)
        {
            Box box = this.ActiveDocument.ActionLayout.GetExtents();
            Image image = new Bitmap(width, height);
            Graphics graph = Graphics.FromImage(image);
            this.ActiveDocument.ActiveLayOut.RenderToGraphics(graph, box, image.Width, image.Height);

            return image;
        }

        private void FillPropertyGrid(object obj)
        {
            vdPropertyGrid.SelectedObject = obj;
        }

        public List<vdFigure> MergeSelection(vdSelection selection)
        {          
            List<vdFigure> beforeAllEntities = this.GetFigures();
            this.ActiveDocument.MergeSelection(selection);
            List<vdFigure> afterAllEntities = this.GetFigures();            
            List<vdFigure> list = afterAllEntities.Where(item => beforeAllEntities.Contains(item) == false).ToList();

            return list;
        }

        private List<vdFigure> GetFigures()
        {
            List<vdFigure> entities = new List<vdFigure>();
            for (int ix = 0; ix < this.ActiveDocument.ActiveLayOut.Entities.Count; ix++)
                entities.Add(this.ActiveDocument.ActiveLayOut.Entities[ix]);

            return entities;
        }

        public List<vdFigure> CommandImport()
        {
            List<vdFigure> result = null;
            using (EntityImportForm form = new EntityImportForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.Selection != null && form.Point != null && form.Selection.Count > 0)
                    {
                        vdSelection selection = form.Selection;
                        this.ActiveDocument.Prompt("Pick Insert point.");
                        this.ActiveDocument.ActionUtility.SetAcceptedStringValues(new string[] { "(0,0)" }, new gPoint(0, 0));
                        
                        object ret = this.ActiveDocument.ActionUtility.getUserPoint();
                        if (ret is gPoint)
                        {
                            vdSelection copy = new vdSelection();
                            foreach(vdFigure fig in selection)
                            {
                                copy.AddItem((vdFigure)fig.Clone(this.ActiveDocument), false, vdSelection.AddItemCheck.Nochecking);
                            }
                            gPoint insertPoint = ret as gPoint;
                            Vector vec = new Vector(insertPoint - form.Point);

                            try
                            {
                                Matrix matrix = new Matrix();
                                matrix.TranslateMatrix(vec);
                                selection.Transformby(matrix);

                                selection.SetUnRegisterDocument(this.ActiveDocument);
                                result = MergeSelection(selection);
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                copy.SetUnRegisterDocument(this.ActiveDocument);
                                result = MergeSelection(copy);
                            }
                        }

                    }
                }
            }
            return result;
        }
        
        public void CommandOsnapW()
        {
            OSnapForm osnapForm = new OSnapForm((int)this.ActiveDocument.osnapMode);
            if (osnapForm.ShowDialog() == DialogResult.OK)
                this.ActiveDocument.osnapMode = (OsnapMode)osnapForm.ReturnOsnapMode;
            this.AddpendCommandText(string.Format("Current OsnapMode : {0}", this.ActiveDocument.osnapMode));
        }
        
        public void CommandOrtho()
        {
            this.ActiveDocument.OrthoMode = !this.ActiveDocument.OrthoMode;
            this.AddpendCommandText(string.Format("Ortho : {0}", this.ActiveDocument.OrthoMode ? "On" : "Off"));
        }

        public void CommandLayer()
        {
            // Layer창은 그대로 사용
            this.ActiveDocument.CommandAction.LayerControl();
        }

        public vdSelection GetSelection()
        {
            StringBuilder sbSelectionName = new StringBuilder("VDGRIPSET_");
            sbSelectionName.Append(ActiveDocument.ActiveLayOut.Handle.ToStringValue());
            if (ActiveDocument.ActiveLayOut.ActiveViewPort != null)
                sbSelectionName.Append(ActiveDocument.ActiveLayOut.ActiveViewPort.Handle.ToString());

            vdSelection selection = ActiveDocument.Selections.FindName(sbSelectionName.ToString());
            return selection;
        }

        public void SetActiveDocument(vdDocument doc)
        {
            vdScrollableControl.BaseControl.SetActiveDocument(doc);
            vdScrollableControl.BaseControl.ReFresh();
        }
    }
}
