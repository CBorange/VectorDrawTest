namespace VectordrawTest
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.vectorDrawBaseControl1 = new VectorDraw.Professional.Control.VectorDrawBaseControl();
            this.HorUp_Btn = new System.Windows.Forms.Button();
            this.vdCommandLine1 = new VectorDraw.Professional.vdCommandLine.vdCommandLine();
            this.AddHorBeam_Btn = new System.Windows.Forms.Button();
            this.AddVerBeam_Btn = new System.Windows.Forms.Button();
            this.MidlineCut_Btn = new System.Windows.Forms.Button();
            this.VerUp_Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // vectorDrawBaseControl1
            // 
            this.vectorDrawBaseControl1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.vectorDrawBaseControl1.AllowDrop = true;
            this.vectorDrawBaseControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.vectorDrawBaseControl1.Location = new System.Drawing.Point(12, 12);
            this.vectorDrawBaseControl1.Name = "vectorDrawBaseControl1";
            this.vectorDrawBaseControl1.Size = new System.Drawing.Size(1294, 740);
            this.vectorDrawBaseControl1.TabIndex = 0;
            // 
            // HorUp_Btn
            // 
            this.HorUp_Btn.Location = new System.Drawing.Point(1312, 12);
            this.HorUp_Btn.Name = "HorUp_Btn";
            this.HorUp_Btn.Size = new System.Drawing.Size(115, 28);
            this.HorUp_Btn.TabIndex = 3;
            this.HorUp_Btn.Text = "가로 바 위";
            this.HorUp_Btn.UseVisualStyleBackColor = true;
            this.HorUp_Btn.Click += new System.EventHandler(this.UpBeam_ForHorizontal);
            // 
            // vdCommandLine1
            // 
            this.vdCommandLine1.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.vdCommandLine1.EnablePopupForm = true;
            this.vdCommandLine1.Location = new System.Drawing.Point(12, 758);
            this.vdCommandLine1.MaxNumberOfCommandsShown = 10;
            this.vdCommandLine1.Name = "vdCommandLine1";
            this.vdCommandLine1.PopupBackColor = System.Drawing.Color.White;
            this.vdCommandLine1.PopupFormFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.vdCommandLine1.PopupFormShowBelow = false;
            this.vdCommandLine1.PopupFormShowIcons = true;
            this.vdCommandLine1.PopupFormWidth = 250;
            this.vdCommandLine1.PopuphighlightColor = System.Drawing.SystemColors.Highlight;
            this.vdCommandLine1.ProcessKeyMessages = true;
            this.vdCommandLine1.ShowPopupFormPerigram = true;
            this.vdCommandLine1.Size = new System.Drawing.Size(650, 54);
            this.vdCommandLine1.TabIndex = 7;
            this.vdCommandLine1.UserTextString = "";
            // 
            // AddHorBeam_Btn
            // 
            this.AddHorBeam_Btn.Font = new System.Drawing.Font("굴림", 9F);
            this.AddHorBeam_Btn.Location = new System.Drawing.Point(1312, 130);
            this.AddHorBeam_Btn.Name = "AddHorBeam_Btn";
            this.AddHorBeam_Btn.Size = new System.Drawing.Size(115, 27);
            this.AddHorBeam_Btn.TabIndex = 8;
            this.AddHorBeam_Btn.Text = "수평 Beam 추가";
            this.AddHorBeam_Btn.UseVisualStyleBackColor = true;
            this.AddHorBeam_Btn.Click += new System.EventHandler(this.CreateHorBeam);
            // 
            // AddVerBeam_Btn
            // 
            this.AddVerBeam_Btn.Font = new System.Drawing.Font("굴림", 9F);
            this.AddVerBeam_Btn.Location = new System.Drawing.Point(1312, 163);
            this.AddVerBeam_Btn.Name = "AddVerBeam_Btn";
            this.AddVerBeam_Btn.Size = new System.Drawing.Size(115, 27);
            this.AddVerBeam_Btn.TabIndex = 9;
            this.AddVerBeam_Btn.Text = "수직 Beam 추가";
            this.AddVerBeam_Btn.UseVisualStyleBackColor = true;
            this.AddVerBeam_Btn.Click += new System.EventHandler(this.CreateVerBeam);
            // 
            // MidlineCut_Btn
            // 
            this.MidlineCut_Btn.Font = new System.Drawing.Font("굴림", 9F);
            this.MidlineCut_Btn.Location = new System.Drawing.Point(1312, 80);
            this.MidlineCut_Btn.Name = "MidlineCut_Btn";
            this.MidlineCut_Btn.Size = new System.Drawing.Size(115, 27);
            this.MidlineCut_Btn.TabIndex = 10;
            this.MidlineCut_Btn.Text = "균등 커팅";
            this.MidlineCut_Btn.UseVisualStyleBackColor = true;
            this.MidlineCut_Btn.Click += new System.EventHandler(this.CuttingBeam_Midline);
            // 
            // VerUp_Btn
            // 
            this.VerUp_Btn.Location = new System.Drawing.Point(1312, 46);
            this.VerUp_Btn.Name = "VerUp_Btn";
            this.VerUp_Btn.Size = new System.Drawing.Size(115, 28);
            this.VerUp_Btn.TabIndex = 11;
            this.VerUp_Btn.Text = "세로 바 위";
            this.VerUp_Btn.UseVisualStyleBackColor = true;
            this.VerUp_Btn.Click += new System.EventHandler(this.UpBeam_ForVertical);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1439, 824);
            this.Controls.Add(this.VerUp_Btn);
            this.Controls.Add(this.MidlineCut_Btn);
            this.Controls.Add(this.AddVerBeam_Btn);
            this.Controls.Add(this.AddHorBeam_Btn);
            this.Controls.Add(this.vdCommandLine1);
            this.Controls.Add(this.HorUp_Btn);
            this.Controls.Add(this.vectorDrawBaseControl1);
            this.Location = new System.Drawing.Point(300, 100);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VectorDraw.Professional.Control.VectorDrawBaseControl vectorDrawBaseControl1;
        private System.Windows.Forms.Button HorUp_Btn;
        private VectorDraw.Professional.vdCommandLine.vdCommandLine vdCommandLine1;
        private System.Windows.Forms.Button AddHorBeam_Btn;
        private System.Windows.Forms.Button AddVerBeam_Btn;
        private System.Windows.Forms.Button MidlineCut_Btn;
        private System.Windows.Forms.Button VerUp_Btn;
    }
}

