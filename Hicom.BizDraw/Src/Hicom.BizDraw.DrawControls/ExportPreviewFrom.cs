using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hicom.BizDraw.DrawControls
{
    public partial class ExportPreviewFrom : Form
    {
        public string FileName;
        private MemoryStream _stream;
        public ExportPreviewFrom()
        {
            InitializeComponent();
        }
        public ExportPreviewFrom(MemoryStream stream)
        {
            InitializeComponent();
            _stream = stream;
        }
        private void ExportPreview_Load(object sender, EventArgs e)
        {
            BizDrawCtrl.ActiveDocument.LoadFromMemory(_stream);
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "DWG Export";
            dlg.Filter = "AUTO CAD FILE|*.dwg";
            dlg.FileName = FileName;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                bool save = BizDrawCtrl.ActiveDocument.Document.SaveAs(dlg.FileName, null, "ver 12");
                DialogResult = DialogResult.OK;
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
