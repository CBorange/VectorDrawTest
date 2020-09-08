namespace Hicom.BizDraw.DrawControls
{
    partial class ProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgressBarControl = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBarControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ProgressBarControl
            // 
            this.ProgressBarControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressBarControl.Location = new System.Drawing.Point(0, 0);
            this.ProgressBarControl.Name = "ProgressBarControl";
            this.ProgressBarControl.Properties.Appearance.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgressBarControl.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            this.ProgressBarControl.Properties.ShowTitle = true;
            this.ProgressBarControl.Properties.Step = 1;
            this.ProgressBarControl.ShowProgressInTaskBar = true;
            this.ProgressBarControl.Size = new System.Drawing.Size(600, 20);
            this.ProgressBarControl.TabIndex = 1;
            // 
            // ProgressForm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(600, 20);
            this.Controls.Add(this.ProgressBarControl);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "진행률";
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBarControl.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.ProgressBarControl ProgressBarControl;
    }
}