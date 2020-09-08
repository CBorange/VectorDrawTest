using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Geometry;
using VectorDraw.Actions;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.Constants;

namespace Hicom.BizDraw.DrawControls
{
    public partial class EntityImportForm : DevExpress.XtraEditors.XtraForm
    {
        public vdSelection Selection;
        public gPoint Point;
        public List<vdPolyline> closeList;
        public vdDocument ActiveDocument { get { return this.BizDrawCtrl.ActiveDocument; } }

        public EntityImportForm()
        {
            InitializeComponent();
        }

        ~EntityImportForm()
        {

        }

        private void EntityImportForm_Load(object sender, EventArgs e)
        {
            Selection = new vdSelection("selection");
            this.BizDrawCtrl.ExecuteCommand(Hicom.BizDraw.Command.Command.OPEN);

            foreach (vdFigure fig in this.ActiveDocument.Model.Entities)
            {
                if (fig is vdDimension)
                {
                    vdDimension dim = fig as vdDimension;
                    dim.UsingDefaultTextPosition = false;
                    dim.Update();

                }
            }
            this.ActiveDocument.Redraw(true);

            this.ActiveDocument.ZoomExtents();
            //20200826 ZoomE 변경
            //this.ActiveDocument.ZoomAll();
        }

        private void BizDrawCtrl_EventHandlerCommandStart(string commandname, bool isDefaultImplemented, ref bool success, ref bool continuous, ref List<object> figures)
        {
            switch(commandname.ToLower())
            {
                case "export":
                    this.Export();
                    break;
            }
        }

        private void Export()
        {
            try
            {
                vdSelection selection = BizDrawCtrl.GetSelection();
                if(selection.Count > 0)
                {
                    this.Selection.AddRange(selection, vdSelection.AddItemCheck.RemoveInVisibleAndLockLayer | vdSelection.AddItemCheck.RemoveInVisibleEntities | vdSelection.AddItemCheck.RemoveLockLayerEntities);                    
                }
                else
                {
                    this.ActiveDocument.Prompt("Select entities.");
                    this.Selection = this.ActiveDocument.ActionUtility.getUserSelection();
                }

                this.ActiveDocument.Prompt("Pick datum point.");
                gPoint p;
                StatusCode scode = this.ActiveDocument.ActionUtility.getUserPoint(out p);


                if (scode == StatusCode.Success)
                {
                    this.Point = p;
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch(OutOfMemoryException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void EntityImportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ActiveDocument.ActiveLayOut.Entities.RemoveAll();
            ActiveDocument.ActiveLayOut.Dispose();
            ActiveDocument.ActiveLayOut = null;
            ActiveDocument.LayOuts.RemoveAll();
            ActiveDocument.ClearAll();
            ActiveDocument.Dispose();
        }

      
    }
}