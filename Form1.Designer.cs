﻿namespace MathPractice
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.vdCommandLine1 = new VectorDraw.Professional.vdCommandLine.vdCommandLine();
            this.SuspendLayout();
            // 
            // vectorDrawBaseControl1
            // 
            this.vectorDrawBaseControl1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.vectorDrawBaseControl1.AllowDrop = true;
            this.vectorDrawBaseControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.vectorDrawBaseControl1.Location = new System.Drawing.Point(194, 12);
            this.vectorDrawBaseControl1.Name = "vectorDrawBaseControl1";
            this.vectorDrawBaseControl1.Size = new System.Drawing.Size(650, 650);
            this.vectorDrawBaseControl1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(850, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 26);
            this.button1.TabIndex = 1;
            this.button1.Text = "회전 +";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.PlusRotate_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(850, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "회전 -";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.MinusRotate_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(850, 87);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 28);
            this.button3.TabIndex = 3;
            this.button3.Text = "가로 바 위";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.CuttingByHorizontal_Click);
            // 
            // vdCommandLine1
            // 
            this.vdCommandLine1.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.vdCommandLine1.EnablePopupForm = true;
            this.vdCommandLine1.Location = new System.Drawing.Point(12, 668);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 734);
            this.Controls.Add(this.vdCommandLine1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vectorDrawBaseControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VectorDraw.Professional.Control.VectorDrawBaseControl vectorDrawBaseControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private VectorDraw.Professional.vdCommandLine.vdCommandLine vdCommandLine1;
    }
}

