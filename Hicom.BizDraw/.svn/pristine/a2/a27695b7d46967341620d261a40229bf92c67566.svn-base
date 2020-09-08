using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Hicom.BizDraw.DrawControls
{
    public partial class ProgressForm : XtraForm
    {
        
        public ProgressForm()
        {
            InitializeComponent();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            this.Height = 18;
        }

        public void ShowProgress(long limit)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.ProgressBarControl.Properties.Maximum = (int)limit;
            this.Show();
        }

        public void SetProgress(long percent)
        {
            this.ProgressBarControl.Position = (int)percent;
            this.ProgressBarControl.Update();
        }

        public void HideProgress()
        {
            Cursor.Current = Cursors.Default;
            this.Hide();
        }
    }
}
