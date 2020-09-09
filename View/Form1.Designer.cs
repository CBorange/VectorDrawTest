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
            this.Calc_CollisionLines_Btn = new System.Windows.Forms.Button();
            this.BarInfo_Group = new System.Windows.Forms.GroupBox();
            this.Bar_Length_Text = new System.Windows.Forms.Label();
            this.Bar_Width_Text = new System.Windows.Forms.Label();
            this.Bar_Length_Label = new System.Windows.Forms.Label();
            this.Bar_Width_Label = new System.Windows.Forms.Label();
            this.Bar_Rotation_Text = new System.Windows.Forms.Label();
            this.Bar_Rotation_Label = new System.Windows.Forms.Label();
            this.Bar_EndPoint_Text = new System.Windows.Forms.Label();
            this.Bar_EndPoint_Label = new System.Windows.Forms.Label();
            this.Bar_StartPoint_Text = new System.Windows.Forms.Label();
            this.Bar_StartPoint_Label = new System.Windows.Forms.Label();
            this.BarName_Text = new System.Windows.Forms.Label();
            this.BarName_Label = new System.Windows.Forms.Label();
            this.BarVertext_Group = new System.Windows.Forms.GroupBox();
            this.Bar_Center_Text = new System.Windows.Forms.Label();
            this.Bar_Right_Text = new System.Windows.Forms.Label();
            this.Bar_Left_Text = new System.Windows.Forms.Label();
            this.Bar_RB_Text = new System.Windows.Forms.Label();
            this.Bar_LB_Text = new System.Windows.Forms.Label();
            this.Bar_RT_Text = new System.Windows.Forms.Label();
            this.Bar_Center_Label = new System.Windows.Forms.Label();
            this.Bar_Right_Label = new System.Windows.Forms.Label();
            this.Bar_Left_Label = new System.Windows.Forms.Label();
            this.Bar_RB_Label = new System.Windows.Forms.Label();
            this.Bar_LB_Label = new System.Windows.Forms.Label();
            this.Bar_RT_Label = new System.Windows.Forms.Label();
            this.Bar_LT_Text = new System.Windows.Forms.Label();
            this.Bar_LT_Label = new System.Windows.Forms.Label();
            this.Bar_StartPoint = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Check_BeamInfo_Btn = new System.Windows.Forms.Button();
            this.Bar_Collision_TreeView = new System.Windows.Forms.TreeView();
            this.Bar_Collision_TreeView_Label = new System.Windows.Forms.Label();
            this.BarInfo_Group.SuspendLayout();
            this.BarVertext_Group.SuspendLayout();
            this.SuspendLayout();
            // 
            // vectorDrawBaseControl1
            // 
            this.vectorDrawBaseControl1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.vectorDrawBaseControl1.AllowDrop = true;
            this.vectorDrawBaseControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.vectorDrawBaseControl1.Location = new System.Drawing.Point(12, 12);
            this.vectorDrawBaseControl1.Name = "vectorDrawBaseControl1";
            this.vectorDrawBaseControl1.Size = new System.Drawing.Size(1255, 777);
            this.vectorDrawBaseControl1.TabIndex = 0;
            // 
            // HorUp_Btn
            // 
            this.HorUp_Btn.Location = new System.Drawing.Point(1273, 12);
            this.HorUp_Btn.Name = "HorUp_Btn";
            this.HorUp_Btn.Size = new System.Drawing.Size(94, 28);
            this.HorUp_Btn.TabIndex = 3;
            this.HorUp_Btn.Text = "가로 바 위";
            this.HorUp_Btn.UseVisualStyleBackColor = true;
            this.HorUp_Btn.Click += new System.EventHandler(this.UpBeam_ForHorizontal);
            // 
            // vdCommandLine1
            // 
            this.vdCommandLine1.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.vdCommandLine1.EnablePopupForm = true;
            this.vdCommandLine1.Location = new System.Drawing.Point(12, 795);
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
            this.AddHorBeam_Btn.Location = new System.Drawing.Point(1273, 46);
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
            this.AddVerBeam_Btn.Location = new System.Drawing.Point(1394, 46);
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
            this.MidlineCut_Btn.Location = new System.Drawing.Point(1468, 13);
            this.MidlineCut_Btn.Name = "MidlineCut_Btn";
            this.MidlineCut_Btn.Size = new System.Drawing.Size(104, 27);
            this.MidlineCut_Btn.TabIndex = 10;
            this.MidlineCut_Btn.Text = "균등 커팅";
            this.MidlineCut_Btn.UseVisualStyleBackColor = true;
            this.MidlineCut_Btn.Click += new System.EventHandler(this.CuttingBeam_Midline);
            // 
            // VerUp_Btn
            // 
            this.VerUp_Btn.Location = new System.Drawing.Point(1373, 12);
            this.VerUp_Btn.Name = "VerUp_Btn";
            this.VerUp_Btn.Size = new System.Drawing.Size(89, 28);
            this.VerUp_Btn.TabIndex = 11;
            this.VerUp_Btn.Text = "세로 바 위";
            this.VerUp_Btn.UseVisualStyleBackColor = true;
            this.VerUp_Btn.Click += new System.EventHandler(this.UpBeam_ForVertical);
            // 
            // Calc_CollisionLines_Btn
            // 
            this.Calc_CollisionLines_Btn.Location = new System.Drawing.Point(1273, 79);
            this.Calc_CollisionLines_Btn.Name = "Calc_CollisionLines_Btn";
            this.Calc_CollisionLines_Btn.Size = new System.Drawing.Size(94, 28);
            this.Calc_CollisionLines_Btn.TabIndex = 12;
            this.Calc_CollisionLines_Btn.Text = "충돌 라인 검사";
            this.Calc_CollisionLines_Btn.UseVisualStyleBackColor = true;
            this.Calc_CollisionLines_Btn.Click += new System.EventHandler(this.CalcCollisionLines);
            // 
            // BarInfo_Group
            // 
            this.BarInfo_Group.Controls.Add(this.Bar_Length_Text);
            this.BarInfo_Group.Controls.Add(this.Bar_Width_Text);
            this.BarInfo_Group.Controls.Add(this.Bar_Length_Label);
            this.BarInfo_Group.Controls.Add(this.Bar_Width_Label);
            this.BarInfo_Group.Controls.Add(this.Bar_Rotation_Text);
            this.BarInfo_Group.Controls.Add(this.Bar_Rotation_Label);
            this.BarInfo_Group.Controls.Add(this.Bar_EndPoint_Text);
            this.BarInfo_Group.Controls.Add(this.Bar_EndPoint_Label);
            this.BarInfo_Group.Controls.Add(this.Bar_StartPoint_Text);
            this.BarInfo_Group.Controls.Add(this.Bar_StartPoint_Label);
            this.BarInfo_Group.Controls.Add(this.BarName_Text);
            this.BarInfo_Group.Controls.Add(this.BarName_Label);
            this.BarInfo_Group.Location = new System.Drawing.Point(1273, 113);
            this.BarInfo_Group.Name = "BarInfo_Group";
            this.BarInfo_Group.Size = new System.Drawing.Size(299, 122);
            this.BarInfo_Group.TabIndex = 13;
            this.BarInfo_Group.TabStop = false;
            this.BarInfo_Group.Text = "바 기본정보";
            // 
            // Bar_Length_Text
            // 
            this.Bar_Length_Text.AutoSize = true;
            this.Bar_Length_Text.Location = new System.Drawing.Point(75, 102);
            this.Bar_Length_Text.Name = "Bar_Length_Text";
            this.Bar_Length_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_Length_Text.TabIndex = 12;
            this.Bar_Length_Text.Text = "19625.6216126126126";
            // 
            // Bar_Width_Text
            // 
            this.Bar_Width_Text.AutoSize = true;
            this.Bar_Width_Text.Location = new System.Drawing.Point(62, 85);
            this.Bar_Width_Text.Name = "Bar_Width_Text";
            this.Bar_Width_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_Width_Text.TabIndex = 11;
            this.Bar_Width_Text.Text = "19625.6216126126126";
            // 
            // Bar_Length_Label
            // 
            this.Bar_Length_Label.AutoSize = true;
            this.Bar_Length_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_Length_Label.Location = new System.Drawing.Point(3, 97);
            this.Bar_Length_Label.Name = "Bar_Length_Label";
            this.Bar_Length_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_Length_Label.Size = new System.Drawing.Size(67, 17);
            this.Bar_Length_Label.TabIndex = 10;
            this.Bar_Length_Label.Text = "바 Length :";
            // 
            // Bar_Width_Label
            // 
            this.Bar_Width_Label.AutoSize = true;
            this.Bar_Width_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_Width_Label.Location = new System.Drawing.Point(3, 80);
            this.Bar_Width_Label.Name = "Bar_Width_Label";
            this.Bar_Width_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_Width_Label.Size = new System.Drawing.Size(59, 17);
            this.Bar_Width_Label.TabIndex = 9;
            this.Bar_Width_Label.Text = "바 Width :";
            // 
            // Bar_Rotation_Text
            // 
            this.Bar_Rotation_Text.AutoSize = true;
            this.Bar_Rotation_Text.Location = new System.Drawing.Point(83, 68);
            this.Bar_Rotation_Text.Name = "Bar_Rotation_Text";
            this.Bar_Rotation_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_Rotation_Text.TabIndex = 7;
            this.Bar_Rotation_Text.Text = "19625.6216126126126";
            // 
            // Bar_Rotation_Label
            // 
            this.Bar_Rotation_Label.AutoSize = true;
            this.Bar_Rotation_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_Rotation_Label.Location = new System.Drawing.Point(3, 63);
            this.Bar_Rotation_Label.Name = "Bar_Rotation_Label";
            this.Bar_Rotation_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_Rotation_Label.Size = new System.Drawing.Size(74, 17);
            this.Bar_Rotation_Label.TabIndex = 6;
            this.Bar_Rotation_Label.Text = "바 Rotation :";
            // 
            // Bar_EndPoint_Text
            // 
            this.Bar_EndPoint_Text.AutoSize = true;
            this.Bar_EndPoint_Text.Location = new System.Drawing.Point(83, 51);
            this.Bar_EndPoint_Text.Name = "Bar_EndPoint_Text";
            this.Bar_EndPoint_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_EndPoint_Text.TabIndex = 5;
            this.Bar_EndPoint_Text.Text = "19625.6216126126126";
            // 
            // Bar_EndPoint_Label
            // 
            this.Bar_EndPoint_Label.AutoSize = true;
            this.Bar_EndPoint_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_EndPoint_Label.Location = new System.Drawing.Point(3, 46);
            this.Bar_EndPoint_Label.Name = "Bar_EndPoint_Label";
            this.Bar_EndPoint_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_EndPoint_Label.Size = new System.Drawing.Size(79, 17);
            this.Bar_EndPoint_Label.TabIndex = 4;
            this.Bar_EndPoint_Label.Text = "바 EndPoint :";
            // 
            // Bar_StartPoint_Text
            // 
            this.Bar_StartPoint_Text.AutoSize = true;
            this.Bar_StartPoint_Text.Location = new System.Drawing.Point(83, 34);
            this.Bar_StartPoint_Text.Name = "Bar_StartPoint_Text";
            this.Bar_StartPoint_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_StartPoint_Text.TabIndex = 3;
            this.Bar_StartPoint_Text.Text = "19625.6216126126126";
            // 
            // Bar_StartPoint_Label
            // 
            this.Bar_StartPoint_Label.AutoSize = true;
            this.Bar_StartPoint_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_StartPoint_Label.Location = new System.Drawing.Point(3, 29);
            this.Bar_StartPoint_Label.Name = "Bar_StartPoint_Label";
            this.Bar_StartPoint_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_StartPoint_Label.Size = new System.Drawing.Size(82, 17);
            this.Bar_StartPoint_Label.TabIndex = 2;
            this.Bar_StartPoint_Label.Text = "바 StartPoint :";
            // 
            // BarName_Text
            // 
            this.BarName_Text.AutoSize = true;
            this.BarName_Text.Location = new System.Drawing.Point(56, 17);
            this.BarName_Text.Name = "BarName_Text";
            this.BarName_Text.Size = new System.Drawing.Size(38, 12);
            this.BarName_Text.TabIndex = 1;
            this.BarName_Text.Text = "Beam";
            // 
            // BarName_Label
            // 
            this.BarName_Label.AutoSize = true;
            this.BarName_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarName_Label.Location = new System.Drawing.Point(3, 17);
            this.BarName_Label.Name = "BarName_Label";
            this.BarName_Label.Size = new System.Drawing.Size(53, 12);
            this.BarName_Label.TabIndex = 0;
            this.BarName_Label.Text = "바 이름 :";
            // 
            // BarVertext_Group
            // 
            this.BarVertext_Group.Controls.Add(this.Bar_Center_Text);
            this.BarVertext_Group.Controls.Add(this.Bar_Right_Text);
            this.BarVertext_Group.Controls.Add(this.Bar_Left_Text);
            this.BarVertext_Group.Controls.Add(this.Bar_RB_Text);
            this.BarVertext_Group.Controls.Add(this.Bar_LB_Text);
            this.BarVertext_Group.Controls.Add(this.Bar_RT_Text);
            this.BarVertext_Group.Controls.Add(this.Bar_Center_Label);
            this.BarVertext_Group.Controls.Add(this.Bar_Right_Label);
            this.BarVertext_Group.Controls.Add(this.Bar_Left_Label);
            this.BarVertext_Group.Controls.Add(this.Bar_RB_Label);
            this.BarVertext_Group.Controls.Add(this.Bar_LB_Label);
            this.BarVertext_Group.Controls.Add(this.Bar_RT_Label);
            this.BarVertext_Group.Controls.Add(this.Bar_LT_Text);
            this.BarVertext_Group.Controls.Add(this.Bar_LT_Label);
            this.BarVertext_Group.Location = new System.Drawing.Point(1273, 241);
            this.BarVertext_Group.Name = "BarVertext_Group";
            this.BarVertext_Group.Size = new System.Drawing.Size(287, 136);
            this.BarVertext_Group.TabIndex = 8;
            this.BarVertext_Group.TabStop = false;
            this.BarVertext_Group.Text = "바 Vertex 정보";
            // 
            // Bar_Center_Text
            // 
            this.Bar_Center_Text.AutoSize = true;
            this.Bar_Center_Text.Location = new System.Drawing.Point(67, 119);
            this.Bar_Center_Text.Name = "Bar_Center_Text";
            this.Bar_Center_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_Center_Text.TabIndex = 13;
            this.Bar_Center_Text.Text = "512589125.215125125";
            // 
            // Bar_Right_Text
            // 
            this.Bar_Right_Text.AutoSize = true;
            this.Bar_Right_Text.Location = new System.Drawing.Point(58, 102);
            this.Bar_Right_Text.Name = "Bar_Right_Text";
            this.Bar_Right_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_Right_Text.TabIndex = 12;
            this.Bar_Right_Text.Text = "512589125.215125125";
            // 
            // Bar_Left_Text
            // 
            this.Bar_Left_Text.AutoSize = true;
            this.Bar_Left_Text.Location = new System.Drawing.Point(50, 85);
            this.Bar_Left_Text.Name = "Bar_Left_Text";
            this.Bar_Left_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_Left_Text.TabIndex = 11;
            this.Bar_Left_Text.Text = "512589125.215125125";
            // 
            // Bar_RB_Text
            // 
            this.Bar_RB_Text.AutoSize = true;
            this.Bar_RB_Text.Location = new System.Drawing.Point(80, 68);
            this.Bar_RB_Text.Name = "Bar_RB_Text";
            this.Bar_RB_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_RB_Text.TabIndex = 10;
            this.Bar_RB_Text.Text = "512589125.215125125";
            // 
            // Bar_LB_Text
            // 
            this.Bar_LB_Text.AutoSize = true;
            this.Bar_LB_Text.Location = new System.Drawing.Point(72, 51);
            this.Bar_LB_Text.Name = "Bar_LB_Text";
            this.Bar_LB_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_LB_Text.TabIndex = 9;
            this.Bar_LB_Text.Text = "512589125.215125125";
            // 
            // Bar_RT_Text
            // 
            this.Bar_RT_Text.AutoSize = true;
            this.Bar_RT_Text.Location = new System.Drawing.Point(72, 34);
            this.Bar_RT_Text.Name = "Bar_RT_Text";
            this.Bar_RT_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_RT_Text.TabIndex = 8;
            this.Bar_RT_Text.Text = "512589125.215125125";
            // 
            // Bar_Center_Label
            // 
            this.Bar_Center_Label.AutoSize = true;
            this.Bar_Center_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_Center_Label.Location = new System.Drawing.Point(3, 114);
            this.Bar_Center_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.Bar_Center_Label.Name = "Bar_Center_Label";
            this.Bar_Center_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_Center_Label.Size = new System.Drawing.Size(66, 17);
            this.Bar_Center_Label.TabIndex = 7;
            this.Bar_Center_Label.Text = "바 Center :";
            // 
            // Bar_Right_Label
            // 
            this.Bar_Right_Label.AutoSize = true;
            this.Bar_Right_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_Right_Label.Location = new System.Drawing.Point(3, 97);
            this.Bar_Right_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.Bar_Right_Label.Name = "Bar_Right_Label";
            this.Bar_Right_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_Right_Label.Size = new System.Drawing.Size(57, 17);
            this.Bar_Right_Label.TabIndex = 6;
            this.Bar_Right_Label.Text = "바 Right :";
            // 
            // Bar_Left_Label
            // 
            this.Bar_Left_Label.AutoSize = true;
            this.Bar_Left_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_Left_Label.Location = new System.Drawing.Point(3, 80);
            this.Bar_Left_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.Bar_Left_Label.Name = "Bar_Left_Label";
            this.Bar_Left_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_Left_Label.Size = new System.Drawing.Size(49, 17);
            this.Bar_Left_Label.TabIndex = 5;
            this.Bar_Left_Label.Text = "바 Left :";
            // 
            // Bar_RB_Label
            // 
            this.Bar_RB_Label.AutoSize = true;
            this.Bar_RB_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_RB_Label.Location = new System.Drawing.Point(3, 63);
            this.Bar_RB_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.Bar_RB_Label.Name = "Bar_RB_Label";
            this.Bar_RB_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_RB_Label.Size = new System.Drawing.Size(75, 17);
            this.Bar_RB_Label.TabIndex = 4;
            this.Bar_RB_Label.Text = "바 RightBot :";
            // 
            // Bar_LB_Label
            // 
            this.Bar_LB_Label.AutoSize = true;
            this.Bar_LB_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_LB_Label.Location = new System.Drawing.Point(3, 46);
            this.Bar_LB_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.Bar_LB_Label.Name = "Bar_LB_Label";
            this.Bar_LB_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_LB_Label.Size = new System.Drawing.Size(67, 17);
            this.Bar_LB_Label.TabIndex = 3;
            this.Bar_LB_Label.Text = "바 LeftBot :";
            // 
            // Bar_RT_Label
            // 
            this.Bar_RT_Label.AutoSize = true;
            this.Bar_RT_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_RT_Label.Location = new System.Drawing.Point(3, 29);
            this.Bar_RT_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.Bar_RT_Label.Name = "Bar_RT_Label";
            this.Bar_RT_Label.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Bar_RT_Label.Size = new System.Drawing.Size(79, 17);
            this.Bar_RT_Label.TabIndex = 2;
            this.Bar_RT_Label.Text = "바 RightTop :";
            // 
            // Bar_LT_Text
            // 
            this.Bar_LT_Text.AutoSize = true;
            this.Bar_LT_Text.Location = new System.Drawing.Point(72, 17);
            this.Bar_LT_Text.Name = "Bar_LT_Text";
            this.Bar_LT_Text.Size = new System.Drawing.Size(117, 12);
            this.Bar_LT_Text.TabIndex = 1;
            this.Bar_LT_Text.Text = "512589125.215125125";
            // 
            // Bar_LT_Label
            // 
            this.Bar_LT_Label.AutoSize = true;
            this.Bar_LT_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_LT_Label.Location = new System.Drawing.Point(3, 17);
            this.Bar_LT_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.Bar_LT_Label.Name = "Bar_LT_Label";
            this.Bar_LT_Label.Size = new System.Drawing.Size(71, 12);
            this.Bar_LT_Label.TabIndex = 0;
            this.Bar_LT_Label.Text = "바 LeftTop :";
            // 
            // Bar_StartPoint
            // 
            this.Bar_StartPoint.AutoSize = true;
            this.Bar_StartPoint.Location = new System.Drawing.Point(83, 34);
            this.Bar_StartPoint.Name = "Bar_StartPoint";
            this.Bar_StartPoint.Size = new System.Drawing.Size(117, 12);
            this.Bar_StartPoint.TabIndex = 3;
            this.Bar_StartPoint.Text = "19625.6216126126126";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "19625.6216126126126";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(83, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "19625.6216126126126";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "바 LeftTop :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(3, 63);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label5.Size = new System.Drawing.Size(71, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "바 LeftTop :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(3, 97);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label7.Size = new System.Drawing.Size(71, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "바 LeftTop :";
            // 
            // Check_BeamInfo_Btn
            // 
            this.Check_BeamInfo_Btn.Location = new System.Drawing.Point(1371, 79);
            this.Check_BeamInfo_Btn.Name = "Check_BeamInfo_Btn";
            this.Check_BeamInfo_Btn.Size = new System.Drawing.Size(92, 28);
            this.Check_BeamInfo_Btn.TabIndex = 14;
            this.Check_BeamInfo_Btn.Text = "바 정보확인";
            this.Check_BeamInfo_Btn.UseVisualStyleBackColor = true;
            this.Check_BeamInfo_Btn.Click += new System.EventHandler(this.CheckBeamInfo);
            // 
            // Bar_Collision_TreeView
            // 
            this.Bar_Collision_TreeView.Location = new System.Drawing.Point(1273, 404);
            this.Bar_Collision_TreeView.Name = "Bar_Collision_TreeView";
            this.Bar_Collision_TreeView.Size = new System.Drawing.Size(287, 385);
            this.Bar_Collision_TreeView.TabIndex = 15;
            // 
            // Bar_Collision_TreeView_Label
            // 
            this.Bar_Collision_TreeView_Label.AutoSize = true;
            this.Bar_Collision_TreeView_Label.Location = new System.Drawing.Point(1273, 389);
            this.Bar_Collision_TreeView_Label.Name = "Bar_Collision_TreeView_Label";
            this.Bar_Collision_TreeView_Label.Size = new System.Drawing.Size(69, 12);
            this.Bar_Collision_TreeView_Label.TabIndex = 14;
            this.Bar_Collision_TreeView_Label.Text = "바 충돌정보";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.Bar_Collision_TreeView_Label);
            this.Controls.Add(this.Bar_Collision_TreeView);
            this.Controls.Add(this.Check_BeamInfo_Btn);
            this.Controls.Add(this.BarInfo_Group);
            this.Controls.Add(this.BarVertext_Group);
            this.Controls.Add(this.Calc_CollisionLines_Btn);
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
            this.BarInfo_Group.ResumeLayout(false);
            this.BarInfo_Group.PerformLayout();
            this.BarVertext_Group.ResumeLayout(false);
            this.BarVertext_Group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VectorDraw.Professional.Control.VectorDrawBaseControl vectorDrawBaseControl1;
        private System.Windows.Forms.Button HorUp_Btn;
        private VectorDraw.Professional.vdCommandLine.vdCommandLine vdCommandLine1;
        private System.Windows.Forms.Button AddHorBeam_Btn;
        private System.Windows.Forms.Button AddVerBeam_Btn;
        private System.Windows.Forms.Button MidlineCut_Btn;
        private System.Windows.Forms.Button VerUp_Btn;
        private System.Windows.Forms.Button Calc_CollisionLines_Btn;
        private System.Windows.Forms.GroupBox BarInfo_Group;
        private System.Windows.Forms.Label BarName_Text;
        private System.Windows.Forms.Label BarName_Label;
        private System.Windows.Forms.Label Bar_StartPoint_Label;
        private System.Windows.Forms.Label Bar_StartPoint_Text;
        private System.Windows.Forms.Label Bar_Rotation_Text;
        private System.Windows.Forms.Label Bar_Rotation_Label;
        private System.Windows.Forms.Label Bar_EndPoint_Text;
        private System.Windows.Forms.Label Bar_EndPoint_Label;
        private System.Windows.Forms.Label Bar_StartPoint;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox BarVertext_Group;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Bar_LT_Label;
        private System.Windows.Forms.Label Bar_LT_Text;
        private System.Windows.Forms.Label Bar_RT_Label;
        private System.Windows.Forms.Label Bar_Center_Label;
        private System.Windows.Forms.Label Bar_Right_Label;
        private System.Windows.Forms.Label Bar_Left_Label;
        private System.Windows.Forms.Label Bar_RB_Label;
        private System.Windows.Forms.Label Bar_LB_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Bar_Center_Text;
        private System.Windows.Forms.Label Bar_Right_Text;
        private System.Windows.Forms.Label Bar_Left_Text;
        private System.Windows.Forms.Label Bar_RB_Text;
        private System.Windows.Forms.Label Bar_LB_Text;
        private System.Windows.Forms.Label Bar_RT_Text;
        private System.Windows.Forms.Button Check_BeamInfo_Btn;
        private System.Windows.Forms.Label Bar_Length_Text;
        private System.Windows.Forms.Label Bar_Width_Text;
        private System.Windows.Forms.Label Bar_Length_Label;
        private System.Windows.Forms.Label Bar_Width_Label;
        private System.Windows.Forms.TreeView Bar_Collision_TreeView;
        private System.Windows.Forms.Label Bar_Collision_TreeView_Label;
    }
}

