namespace Hicom.BizDraw.DrawControls
{
    partial class OSnapForm
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
            this.btnAll = new DevExpress.XtraEditors.SimpleButton();
            this.ckbEndPoint = new DevExpress.XtraEditors.CheckEdit();
            this.ckbMidPoint = new DevExpress.XtraEditors.CheckEdit();
            this.ckbCenter = new DevExpress.XtraEditors.CheckEdit();
            this.ckbNearest = new DevExpress.XtraEditors.CheckEdit();
            this.ckbPerpendicular = new DevExpress.XtraEditors.CheckEdit();
            this.ckbIntersection = new DevExpress.XtraEditors.CheckEdit();
            this.ckbExtension = new DevExpress.XtraEditors.CheckEdit();
            this.pbxEndPoint = new System.Windows.Forms.PictureBox();
            this.pbxMidPoint = new System.Windows.Forms.PictureBox();
            this.pbxCenter = new System.Windows.Forms.PictureBox();
            this.pbxNearest = new System.Windows.Forms.PictureBox();
            this.pbxPerpendicular = new System.Windows.Forms.PictureBox();
            this.pbxIntersection = new System.Windows.Forms.PictureBox();
            this.pbxExtension = new System.Windows.Forms.PictureBox();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ckbEndPoint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbMidPoint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbCenter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbNearest.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbPerpendicular.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbIntersection.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbExtension.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxEndPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMidPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNearest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPerpendicular)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIntersection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxExtension)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(12, 173);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 0;
            this.btnAll.Text = "ALL";
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // ckbEndPoint
            // 
            this.ckbEndPoint.Location = new System.Drawing.Point(55, 25);
            this.ckbEndPoint.Name = "ckbEndPoint";
            this.ckbEndPoint.Properties.Caption = "END POINT";
            this.ckbEndPoint.Size = new System.Drawing.Size(112, 19);
            this.ckbEndPoint.TabIndex = 1;
            this.ckbEndPoint.CheckedChanged += new System.EventHandler(this.ckb_CheckedChanged);
            // 
            // ckbMidPoint
            // 
            this.ckbMidPoint.Location = new System.Drawing.Point(55, 63);
            this.ckbMidPoint.Name = "ckbMidPoint";
            this.ckbMidPoint.Properties.Caption = "MID POINT";
            this.ckbMidPoint.Size = new System.Drawing.Size(112, 19);
            this.ckbMidPoint.TabIndex = 2;
            this.ckbMidPoint.CheckedChanged += new System.EventHandler(this.ckb_CheckedChanged);
            // 
            // ckbCenter
            // 
            this.ckbCenter.Location = new System.Drawing.Point(55, 101);
            this.ckbCenter.Name = "ckbCenter";
            this.ckbCenter.Properties.Caption = "CENTER";
            this.ckbCenter.Size = new System.Drawing.Size(112, 19);
            this.ckbCenter.TabIndex = 3;
            this.ckbCenter.CheckedChanged += new System.EventHandler(this.ckb_CheckedChanged);
            // 
            // ckbNearest
            // 
            this.ckbNearest.Location = new System.Drawing.Point(55, 139);
            this.ckbNearest.Name = "ckbNearest";
            this.ckbNearest.Properties.Caption = "NEAREST";
            this.ckbNearest.Size = new System.Drawing.Size(112, 19);
            this.ckbNearest.TabIndex = 4;
            this.ckbNearest.CheckedChanged += new System.EventHandler(this.ckb_CheckedChanged);
            // 
            // ckbPerpendicular
            // 
            this.ckbPerpendicular.Location = new System.Drawing.Point(228, 25);
            this.ckbPerpendicular.Name = "ckbPerpendicular";
            this.ckbPerpendicular.Properties.Caption = "PERPENDICULAR";
            this.ckbPerpendicular.Size = new System.Drawing.Size(112, 19);
            this.ckbPerpendicular.TabIndex = 5;
            this.ckbPerpendicular.CheckedChanged += new System.EventHandler(this.ckb_CheckedChanged);
            // 
            // ckbIntersection
            // 
            this.ckbIntersection.Location = new System.Drawing.Point(227, 63);
            this.ckbIntersection.Name = "ckbIntersection";
            this.ckbIntersection.Properties.Caption = "INTERSECTION";
            this.ckbIntersection.Size = new System.Drawing.Size(112, 19);
            this.ckbIntersection.TabIndex = 6;
            this.ckbIntersection.CheckedChanged += new System.EventHandler(this.ckb_CheckedChanged);
            // 
            // ckbExtension
            // 
            this.ckbExtension.Location = new System.Drawing.Point(228, 101);
            this.ckbExtension.Name = "ckbExtension";
            this.ckbExtension.Properties.Caption = "EXTENSION";
            this.ckbExtension.Size = new System.Drawing.Size(112, 19);
            this.ckbExtension.TabIndex = 7;
            this.ckbExtension.CheckedChanged += new System.EventHandler(this.ckb_CheckedChanged);
            // 
            // pbxEndPoint
            // 
            this.pbxEndPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxEndPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxEndPoint.Location = new System.Drawing.Point(17, 12);
            this.pbxEndPoint.Name = "pbxEndPoint";
            this.pbxEndPoint.Size = new System.Drawing.Size(32, 32);
            this.pbxEndPoint.TabIndex = 8;
            this.pbxEndPoint.TabStop = false;
            // 
            // pbxMidPoint
            // 
            this.pbxMidPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxMidPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxMidPoint.Location = new System.Drawing.Point(17, 50);
            this.pbxMidPoint.Name = "pbxMidPoint";
            this.pbxMidPoint.Size = new System.Drawing.Size(32, 32);
            this.pbxMidPoint.TabIndex = 9;
            this.pbxMidPoint.TabStop = false;
            // 
            // pbxCenter
            // 
            this.pbxCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxCenter.Location = new System.Drawing.Point(17, 88);
            this.pbxCenter.Name = "pbxCenter";
            this.pbxCenter.Size = new System.Drawing.Size(32, 32);
            this.pbxCenter.TabIndex = 10;
            this.pbxCenter.TabStop = false;
            // 
            // pbxNearest
            // 
            this.pbxNearest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxNearest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxNearest.Location = new System.Drawing.Point(17, 126);
            this.pbxNearest.Name = "pbxNearest";
            this.pbxNearest.Size = new System.Drawing.Size(32, 32);
            this.pbxNearest.TabIndex = 11;
            this.pbxNearest.TabStop = false;
            // 
            // pbxPerpendicular
            // 
            this.pbxPerpendicular.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxPerpendicular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxPerpendicular.Location = new System.Drawing.Point(190, 12);
            this.pbxPerpendicular.Name = "pbxPerpendicular";
            this.pbxPerpendicular.Size = new System.Drawing.Size(32, 32);
            this.pbxPerpendicular.TabIndex = 12;
            this.pbxPerpendicular.TabStop = false;
            // 
            // pbxIntersection
            // 
            this.pbxIntersection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxIntersection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxIntersection.Location = new System.Drawing.Point(190, 50);
            this.pbxIntersection.Name = "pbxIntersection";
            this.pbxIntersection.Size = new System.Drawing.Size(32, 32);
            this.pbxIntersection.TabIndex = 13;
            this.pbxIntersection.TabStop = false;
            // 
            // pbxExtension
            // 
            this.pbxExtension.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxExtension.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxExtension.Location = new System.Drawing.Point(190, 88);
            this.pbxExtension.Name = "pbxExtension";
            this.pbxExtension.Size = new System.Drawing.Size(32, 32);
            this.pbxExtension.TabIndex = 14;
            this.pbxExtension.TabStop = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(93, 173);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 15;
            this.btnClear.Text = "CLEAR";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(189, 173);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 173);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // OSnapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(357, 208);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pbxExtension);
            this.Controls.Add(this.pbxIntersection);
            this.Controls.Add(this.pbxPerpendicular);
            this.Controls.Add(this.pbxNearest);
            this.Controls.Add(this.pbxCenter);
            this.Controls.Add(this.pbxMidPoint);
            this.Controls.Add(this.pbxEndPoint);
            this.Controls.Add(this.ckbExtension);
            this.Controls.Add(this.ckbIntersection);
            this.Controls.Add(this.ckbPerpendicular);
            this.Controls.Add(this.ckbNearest);
            this.Controls.Add(this.ckbCenter);
            this.Controls.Add(this.ckbMidPoint);
            this.Controls.Add(this.ckbEndPoint);
            this.Controls.Add(this.btnAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OSnapForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Osnap Option";
            ((System.ComponentModel.ISupportInitialize)(this.ckbEndPoint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbMidPoint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbCenter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbNearest.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbPerpendicular.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbIntersection.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckbExtension.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxEndPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMidPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNearest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPerpendicular)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIntersection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxExtension)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnAll;
        private DevExpress.XtraEditors.CheckEdit ckbEndPoint;
        private DevExpress.XtraEditors.CheckEdit ckbMidPoint;
        private DevExpress.XtraEditors.CheckEdit ckbCenter;
        private DevExpress.XtraEditors.CheckEdit ckbNearest;
        private DevExpress.XtraEditors.CheckEdit ckbPerpendicular;
        private DevExpress.XtraEditors.CheckEdit ckbIntersection;
        private DevExpress.XtraEditors.CheckEdit ckbExtension;
        private System.Windows.Forms.PictureBox pbxEndPoint;
        private System.Windows.Forms.PictureBox pbxMidPoint;
        private System.Windows.Forms.PictureBox pbxCenter;
        private System.Windows.Forms.PictureBox pbxNearest;
        private System.Windows.Forms.PictureBox pbxPerpendicular;
        private System.Windows.Forms.PictureBox pbxIntersection;
        private System.Windows.Forms.PictureBox pbxExtension;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}