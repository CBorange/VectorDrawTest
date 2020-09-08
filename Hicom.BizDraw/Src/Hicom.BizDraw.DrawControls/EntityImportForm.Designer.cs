namespace Hicom.BizDraw.DrawControls
{
    partial class EntityImportForm
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
            this.BizDrawCtrl = new BizDrawCtrl();
            this.SuspendLayout();
            // 
            // BizDrawCtrl
            // 
            this.BizDrawCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BizDrawCtrl.Location = new System.Drawing.Point(0, 0);
            this.BizDrawCtrl.Name = "BizDrawCtrl";
            this.BizDrawCtrl.ShowCommand = true;
            this.BizDrawCtrl.ShowProgress = false;
            this.BizDrawCtrl.ShowProperty = true;
            this.BizDrawCtrl.Size = new System.Drawing.Size(924, 559);
            this.BizDrawCtrl.TabIndex = 0;
            this.BizDrawCtrl.EventHandlerCommandStart += new BizDrawCtrl.CommandStartEventHandler(this.BizDrawCtrl_EventHandlerCommandStart);
            // 
            // EntityImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(924, 559);
            this.Controls.Add(this.BizDrawCtrl);
            this.Name = "EntityImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "불러오기";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EntityImportForm_FormClosed);
            this.Load += new System.EventHandler(this.EntityImportForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DrawControls.BizDrawCtrl BizDrawCtrl;
    }
}