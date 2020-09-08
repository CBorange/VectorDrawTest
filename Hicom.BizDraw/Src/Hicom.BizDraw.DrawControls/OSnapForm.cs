using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Hicom.BizDraw.DrawControls
{
    public partial class OSnapForm : DevExpress.XtraEditors.XtraForm
    {
        private enum OsnapMode
        {
            NONE = 0,
            END = 1,
            MID = 2,
            CEN = 4,
            PER = 16,
            NEA = 32,
            INTERS = 64,
            EXTENSION = 2048,
            ALL = 2167,
        };

        private OsnapMode OriOsnapMode;
        private OsnapMode CurOsnapMode;
        public int ReturnOsnapMode
        {
            get
            {
                if (this.DialogResult == DialogResult.OK)
                    return (int)this.CurOsnapMode;
                else
                    return (int)this.OriOsnapMode;
            }
        }

        public OSnapForm(int osnapMode)
        {
            InitializeComponent();
            
            this.Initialize(osnapMode);
        }

        private void Initialize(int osnapMode)
        {
            this.OriOsnapMode = (OsnapMode)osnapMode;
            this.CurOsnapMode = this.OriOsnapMode;
            
            if ((CurOsnapMode & OsnapMode.END) > 0)
                this.ckbEndPoint.CheckState = CheckState.Checked;
            if ((CurOsnapMode & OsnapMode.MID) > 0)
                this.ckbMidPoint.CheckState = CheckState.Checked;
            if ((CurOsnapMode & OsnapMode.CEN) > 0)
                this.ckbCenter.CheckState = CheckState.Checked;
            if ((CurOsnapMode & OsnapMode.NEA) > 0)
                this.ckbNearest.CheckState = CheckState.Checked;
            if ((CurOsnapMode & OsnapMode.PER) > 0)
                this.ckbPerpendicular.CheckState = CheckState.Checked;
            if ((CurOsnapMode & OsnapMode.INTERS) > 0)
                this.ckbIntersection.CheckState = CheckState.Checked;
            if ((CurOsnapMode & OsnapMode.EXTENSION) > 0)
                this.ckbExtension.CheckState = CheckState.Checked;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            this.ckbEndPoint.CheckState = CheckState.Checked;
            this.ckbMidPoint.CheckState = CheckState.Checked;
            this.ckbCenter.CheckState = CheckState.Checked;
            this.ckbNearest.CheckState = CheckState.Checked;
            this.ckbIntersection.CheckState = CheckState.Checked;
            this.ckbPerpendicular.CheckState = CheckState.Checked;
            this.ckbExtension.CheckState = CheckState.Checked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ckbEndPoint.CheckState = CheckState.Unchecked;
            this.ckbMidPoint.CheckState = CheckState.Unchecked;
            this.ckbCenter.CheckState = CheckState.Unchecked;
            this.ckbNearest.CheckState = CheckState.Unchecked;
            this.ckbIntersection.CheckState = CheckState.Unchecked;
            this.ckbPerpendicular.CheckState = CheckState.Unchecked;
            this.ckbExtension.CheckState = CheckState.Unchecked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void ckb_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit ckb = sender as CheckEdit;
            if (ckb != null)
            {
                OsnapMode compareOsnap = OsnapMode.NONE;
                if (ckb.Equals(this.ckbEndPoint))
                    compareOsnap = OsnapMode.END;
                if (ckb.Equals(this.ckbMidPoint))
                    compareOsnap = OsnapMode.MID;
                if (ckb.Equals(this.ckbCenter))
                    compareOsnap = OsnapMode.CEN;
                if (ckb.Equals(this.ckbNearest))
                    compareOsnap = OsnapMode.NEA;
                if (ckb.Equals(this.ckbPerpendicular))
                    compareOsnap = OsnapMode.PER;
                if (ckb.Equals(this.ckbIntersection))
                    compareOsnap = OsnapMode.INTERS;
                if (ckb.Equals(this.ckbExtension))
                    compareOsnap = OsnapMode.EXTENSION;

                if (ckb.CheckState == CheckState.Checked)
                {
                    if ((this.CurOsnapMode & compareOsnap) == 0)
                        this.CurOsnapMode |= compareOsnap;
                }
                else
                {
                    if ((this.CurOsnapMode & compareOsnap) != 0)
                        this.CurOsnapMode &= ~compareOsnap;
                }
            }
        }
    }
}