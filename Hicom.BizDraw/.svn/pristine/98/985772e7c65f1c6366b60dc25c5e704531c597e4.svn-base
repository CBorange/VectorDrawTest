using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Hicom.BizDraw.PlanDraw;

namespace Hicom.BizDraw.DrawControls
{
    public partial class DimensionOptionForm : XtraForm
    {
        public DrawDimension.DimensionOptions Options { get; set; }
        public DimensionOptionForm(DrawDimension.DimensionOptions options)
        {
            InitializeComponent();
            Options = new DrawDimension.DimensionOptions(options);
        }

        private void DimensionOptionForm_Load(object sender, EventArgs e)
        {
            SetData();
        }

        private void SetData()
        {
            this.TextBoxScaleFactor.Text = string.Concat(Options.ScaleFactor);
            this.TextBoxTextHeight.Text = string.Concat(Options.TextHeight);
            this.TextBoxDecimalPrecision.Text = string.Concat(Options.DecimalPrecision);
            this.TextBoxArrowSize.Text = string.Concat(Options.ArrowSize);
            this.TextBoxTextDistance.Text = string.Concat(Options.TextDist);
            this.TextBoxExtLineDist1.Text = string.Concat(Options.ExtLineDist1);
            this.TextBoxExtLineDist2.Text = string.Concat(Options.ExtLineDist2);
        }

        private void SaveData()
        {
            Options.ScaleFactor = ToDouble(this.TextBoxScaleFactor);
            Options.TextHeight = ToDouble(this.TextBoxTextHeight);
            Options.DecimalPrecision = (short)ToDouble(this.TextBoxDecimalPrecision);
            Options.ArrowSize = ToDouble(this.TextBoxArrowSize);
            Options.TextDist = ToDouble(this.TextBoxTextDistance);
            Options.ExtLineDist1 = ToDouble(this.TextBoxExtLineDist1);
            Options.ExtLineDist2 = ToDouble(this.TextBoxExtLineDist2);
        }

        private double ToDouble(TextBox edit)
        {
            double result = double.NaN;
            if (double.TryParse(edit.Text, out result))
                return result;
            return result;
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            SaveData();
            DialogResult = DialogResult.OK;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}