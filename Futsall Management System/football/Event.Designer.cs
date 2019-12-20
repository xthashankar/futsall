namespace football
{
    partial class Event
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Event));
            this.Enoofdays = new System.Windows.Forms.ComboBox();
            this.ErefPrsn = new System.Windows.Forms.TextBox();
            this.ETm = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ERem = new System.Windows.Forms.TextBox();
            this.label138 = new System.Windows.Forms.Label();
            this.label139 = new System.Windows.Forms.Label();
            this.Efed = new System.Windows.Forms.RichTextBox();
            this.EComp = new System.Windows.Forms.TextBox();
            this.label140 = new System.Windows.Forms.Label();
            this.EAdd = new System.Windows.Forms.TextBox();
            this.EMob = new System.Windows.Forms.TextBox();
            this.ENm = new System.Windows.Forms.TextBox();
            this.EEvNm = new System.Windows.Forms.TextBox();
            this.label141 = new System.Windows.Forms.Label();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.button14 = new System.Windows.Forms.Button();
            this.EBook = new System.Windows.Forms.Button();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.label142 = new System.Windows.Forms.Label();
            this.dateTimePicker8 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker6 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker7 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker5 = new System.Windows.Forms.DateTimePicker();
            this.label153 = new System.Windows.Forms.Label();
            this.label143 = new System.Windows.Forms.Label();
            this.label144 = new System.Windows.Forms.Label();
            this.label152 = new System.Windows.Forms.Label();
            this.label150 = new System.Windows.Forms.Label();
            this.label151 = new System.Windows.Forms.Label();
            this.label145 = new System.Windows.Forms.Label();
            this.label146 = new System.Windows.Forms.Label();
            this.label147 = new System.Windows.Forms.Label();
            this.EGrdNm = new System.Windows.Forms.ComboBox();
            this.label154 = new System.Windows.Forms.Label();
            this.label148 = new System.Windows.Forms.Label();
            this.label149 = new System.Windows.Forms.Label();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.SuspendLayout();
            // 
            // Enoofdays
            // 
            this.Enoofdays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Enoofdays.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.Enoofdays.FormattingEnabled = true;
            this.Enoofdays.ItemHeight = 17;
            this.Enoofdays.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.Enoofdays.Location = new System.Drawing.Point(162, 282);
            this.Enoofdays.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Enoofdays.Name = "Enoofdays";
            this.Enoofdays.Size = new System.Drawing.Size(209, 25);
            this.Enoofdays.TabIndex = 147;
            this.Enoofdays.SelectedIndexChanged += new System.EventHandler(this.Enoofdays_SelectedIndexChanged);
            // 
            // ErefPrsn
            // 
            this.ErefPrsn.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.ErefPrsn.Location = new System.Drawing.Point(162, 387);
            this.ErefPrsn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ErefPrsn.Name = "ErefPrsn";
            this.ErefPrsn.Size = new System.Drawing.Size(209, 23);
            this.ErefPrsn.TabIndex = 140;
            this.ErefPrsn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ENm_KeyPress);
            // 
            // ETm
            // 
            this.ETm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ETm.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.ETm.FormattingEnabled = true;
            this.ETm.ItemHeight = 17;
            this.ETm.Items.AddRange(new object[] {
            " 6:00 am - 7:00 am",
            " 7:00 am - 8:00 am",
            " 8:00 am - 9:00 am",
            " 9:00 am - 10:00 am",
            " 10:00 am - 11:00 am",
            " 11:00 am - 12:00 pm",
            " 12:00 pm - 1:00 pm",
            " 1:00 pm - 2:00 pm",
            " 2:00 pm - 3:00 pm",
            " 3:00 pm - 4:00 pm",
            " 4:00 pm - 5:00 pm",
            " 5:00 pm - 6:00 pm",
            " 6:00 pm - 7:00 pm",
            "Whole Day"});
            this.ETm.Location = new System.Drawing.Point(162, 317);
            this.ETm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ETm.Name = "ETm";
            this.ETm.Size = new System.Drawing.Size(209, 25);
            this.ETm.TabIndex = 148;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.ERem);
            this.groupBox6.Controls.Add(this.label138);
            this.groupBox6.Controls.Add(this.label139);
            this.groupBox6.Controls.Add(this.Efed);
            this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox6.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.groupBox6.ForeColor = System.Drawing.Color.Black;
            this.groupBox6.Location = new System.Drawing.Point(483, 184);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.Size = new System.Drawing.Size(428, 193);
            this.groupBox6.TabIndex = 174;
            this.groupBox6.TabStop = false;
            // 
            // ERem
            // 
            this.ERem.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.ERem.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.ERem.Location = new System.Drawing.Point(100, 30);
            this.ERem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ERem.Name = "ERem";
            this.ERem.Size = new System.Drawing.Size(209, 23);
            this.ERem.TabIndex = 0;
            this.ERem.Text = "Comment...";
            this.ERem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ERem_KeyDown);
            this.ERem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ENm_KeyPress);
            // 
            // label138
            // 
            this.label138.AutoSize = true;
            this.label138.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label138.ForeColor = System.Drawing.Color.Black;
            this.label138.Location = new System.Drawing.Point(13, 72);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(81, 16);
            this.label138.TabIndex = 60;
            this.label138.Text = "Feedback :";
            // 
            // label139
            // 
            this.label139.AutoSize = true;
            this.label139.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label139.ForeColor = System.Drawing.Color.Black;
            this.label139.Location = new System.Drawing.Point(28, 33);
            this.label139.Name = "label139";
            this.label139.Size = new System.Drawing.Size(66, 16);
            this.label139.TabIndex = 59;
            this.label139.Text = "Remark :";
            // 
            // Efed
            // 
            this.Efed.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.Efed.Location = new System.Drawing.Point(100, 68);
            this.Efed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Efed.Name = "Efed";
            this.Efed.Size = new System.Drawing.Size(311, 103);
            this.Efed.TabIndex = 2;
            this.Efed.Text = "";
            this.Efed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ENm_KeyPress);
            // 
            // EComp
            // 
            this.EComp.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.EComp.Location = new System.Drawing.Point(162, 183);
            this.EComp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EComp.Name = "EComp";
            this.EComp.Size = new System.Drawing.Size(209, 23);
            this.EComp.TabIndex = 144;
            // 
            // label140
            // 
            this.label140.AutoSize = true;
            this.label140.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label140.ForeColor = System.Drawing.Color.Black;
            this.label140.Location = new System.Drawing.Point(37, 390);
            this.label140.Name = "label140";
            this.label140.Size = new System.Drawing.Size(121, 16);
            this.label140.TabIndex = 141;
            this.label140.Text = "Refrence person :";
            // 
            // EAdd
            // 
            this.EAdd.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.EAdd.Location = new System.Drawing.Point(162, 249);
            this.EAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EAdd.Name = "EAdd";
            this.EAdd.Size = new System.Drawing.Size(209, 23);
            this.EAdd.TabIndex = 146;
            // 
            // EMob
            // 
            this.EMob.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.EMob.Location = new System.Drawing.Point(162, 216);
            this.EMob.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EMob.MaxLength = 10;
            this.EMob.Name = "EMob";
            this.EMob.Size = new System.Drawing.Size(209, 23);
            this.EMob.TabIndex = 145;
            this.EMob.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EMob_KeyPress);
            // 
            // ENm
            // 
            this.ENm.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.ENm.Location = new System.Drawing.Point(162, 117);
            this.ENm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ENm.Name = "ENm";
            this.ENm.Size = new System.Drawing.Size(209, 23);
            this.ENm.TabIndex = 142;
            this.ENm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ENm_KeyPress);
            // 
            // EEvNm
            // 
            this.EEvNm.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.EEvNm.Location = new System.Drawing.Point(162, 150);
            this.EEvNm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EEvNm.Name = "EEvNm";
            this.EEvNm.Size = new System.Drawing.Size(209, 23);
            this.EEvNm.TabIndex = 143;
            // 
            // label141
            // 
            this.label141.AutoSize = true;
            this.label141.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label141.ForeColor = System.Drawing.Color.Black;
            this.label141.Location = new System.Drawing.Point(78, 186);
            this.label141.Name = "label141";
            this.label141.Size = new System.Drawing.Size(80, 16);
            this.label141.TabIndex = 173;
            this.label141.Text = "Company :";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("Century Gothic", 9.25F, System.Drawing.FontStyle.Bold);
            this.radioButton4.ForeColor = System.Drawing.Color.Black;
            this.radioButton4.Location = new System.Drawing.Point(356, 89);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(152, 20);
            this.radioButton4.TabIndex = 172;
            this.radioButton4.Text = "Facebook message";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Font = new System.Drawing.Font("Century Gothic", 9.25F, System.Drawing.FontStyle.Bold);
            this.radioButton5.ForeColor = System.Drawing.Color.Black;
            this.radioButton5.Location = new System.Drawing.Point(242, 89);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(103, 20);
            this.radioButton5.TabIndex = 171;
            this.radioButton5.Text = "On The Spot";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(27)))), ((int)(((byte)(50)))));
            this.button14.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.Font = new System.Drawing.Font("Century Gothic", 7.75F);
            this.button14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.button14.Location = new System.Drawing.Point(483, 383);
            this.button14.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(105, 30);
            this.button14.TabIndex = 155;
            this.button14.Text = "Clear All";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // EBook
            // 
            this.EBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(27)))), ((int)(((byte)(50)))));
            this.EBook.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.EBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EBook.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EBook.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.EBook.Location = new System.Drawing.Point(762, 442);
            this.EBook.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EBook.Name = "EBook";
            this.EBook.Size = new System.Drawing.Size(120, 39);
            this.EBook.TabIndex = 154;
            this.EBook.Text = "Book!";
            this.EBook.UseVisualStyleBackColor = false;
            this.EBook.Click += new System.EventHandler(this.EBook_Click);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.BackColor = System.Drawing.Color.Transparent;
            this.radioButton6.Font = new System.Drawing.Font("Century Gothic", 9.25F, System.Drawing.FontStyle.Bold);
            this.radioButton6.ForeColor = System.Drawing.Color.Black;
            this.radioButton6.Location = new System.Drawing.Point(115, 89);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(116, 20);
            this.radioButton6.TabIndex = 170;
            this.radioButton6.Text = "Phone/Mobile";
            this.radioButton6.UseVisualStyleBackColor = false;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // label142
            // 
            this.label142.AutoSize = true;
            this.label142.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label142.ForeColor = System.Drawing.Color.Black;
            this.label142.Location = new System.Drawing.Point(48, 65);
            this.label142.Name = "label142";
            this.label142.Size = new System.Drawing.Size(110, 16);
            this.label142.TabIndex = 169;
            this.label142.Text = "Booking Mode :";
            // 
            // dateTimePicker8
            // 
            this.dateTimePicker8.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker8.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker8.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker8.Location = new System.Drawing.Point(602, 437);
            this.dateTimePicker8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker8.Name = "dateTimePicker8";
            this.dateTimePicker8.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker8.TabIndex = 153;
            this.dateTimePicker8.Visible = false;
            // 
            // dateTimePicker6
            // 
            this.dateTimePicker6.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker6.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker6.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker6.Location = new System.Drawing.Point(257, 437);
            this.dateTimePicker6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker6.Name = "dateTimePicker6";
            this.dateTimePicker6.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker6.TabIndex = 151;
            this.dateTimePicker6.Visible = false;
            // 
            // dateTimePicker7
            // 
            this.dateTimePicker7.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker7.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker7.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker7.Location = new System.Drawing.Point(430, 437);
            this.dateTimePicker7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker7.Name = "dateTimePicker7";
            this.dateTimePicker7.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker7.TabIndex = 152;
            this.dateTimePicker7.Visible = false;
            // 
            // dateTimePicker5
            // 
            this.dateTimePicker5.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker5.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker5.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker5.Location = new System.Drawing.Point(84, 437);
            this.dateTimePicker5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker5.Name = "dateTimePicker5";
            this.dateTimePicker5.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker5.TabIndex = 150;
            this.dateTimePicker5.Visible = false;
            // 
            // label153
            // 
            this.label153.AutoSize = true;
            this.label153.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label153.ForeColor = System.Drawing.Color.Black;
            this.label153.Location = new System.Drawing.Point(72, 285);
            this.label153.Name = "label153";
            this.label153.Size = new System.Drawing.Size(86, 16);
            this.label153.TabIndex = 166;
            this.label153.Text = "No of Days :";
            // 
            // label143
            // 
            this.label143.AutoSize = true;
            this.label143.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label143.ForeColor = System.Drawing.Color.Black;
            this.label143.Location = new System.Drawing.Point(112, 320);
            this.label143.Name = "label143";
            this.label143.Size = new System.Drawing.Size(46, 16);
            this.label143.TabIndex = 167;
            this.label143.Text = "Time :";
            // 
            // label144
            // 
            this.label144.AutoSize = true;
            this.label144.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label144.ForeColor = System.Drawing.Color.Black;
            this.label144.Location = new System.Drawing.Point(93, 355);
            this.label144.Name = "label144";
            this.label144.Size = new System.Drawing.Size(65, 16);
            this.label144.TabIndex = 165;
            this.label144.Text = "Ground :";
            // 
            // label152
            // 
            this.label152.AutoSize = true;
            this.label152.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label152.ForeColor = System.Drawing.Color.Black;
            this.label152.Location = new System.Drawing.Point(548, 442);
            this.label152.Name = "label152";
            this.label152.Size = new System.Drawing.Size(46, 16);
            this.label152.TabIndex = 164;
            this.label152.Text = "Date :";
            this.label152.Visible = false;
            // 
            // label150
            // 
            this.label150.AutoSize = true;
            this.label150.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label150.ForeColor = System.Drawing.Color.Black;
            this.label150.Location = new System.Drawing.Point(198, 442);
            this.label150.Name = "label150";
            this.label150.Size = new System.Drawing.Size(46, 16);
            this.label150.TabIndex = 163;
            this.label150.Text = "Date :";
            this.label150.Visible = false;
            // 
            // label151
            // 
            this.label151.AutoSize = true;
            this.label151.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label151.ForeColor = System.Drawing.Color.Black;
            this.label151.Location = new System.Drawing.Point(373, 442);
            this.label151.Name = "label151";
            this.label151.Size = new System.Drawing.Size(46, 16);
            this.label151.TabIndex = 162;
            this.label151.Text = "Date :";
            this.label151.Visible = false;
            // 
            // label145
            // 
            this.label145.AutoSize = true;
            this.label145.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label145.ForeColor = System.Drawing.Color.Black;
            this.label145.Location = new System.Drawing.Point(23, 442);
            this.label145.Name = "label145";
            this.label145.Size = new System.Drawing.Size(46, 16);
            this.label145.TabIndex = 161;
            this.label145.Text = "Date :";
            this.label145.Visible = false;
            // 
            // label146
            // 
            this.label146.AutoSize = true;
            this.label146.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label146.ForeColor = System.Drawing.Color.Black;
            this.label146.Location = new System.Drawing.Point(89, 252);
            this.label146.Name = "label146";
            this.label146.Size = new System.Drawing.Size(69, 16);
            this.label146.TabIndex = 160;
            this.label146.Text = "Address :";
            // 
            // label147
            // 
            this.label147.AutoSize = true;
            this.label147.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label147.ForeColor = System.Drawing.Color.Black;
            this.label147.Location = new System.Drawing.Point(97, 219);
            this.label147.Name = "label147";
            this.label147.Size = new System.Drawing.Size(61, 16);
            this.label147.TabIndex = 159;
            this.label147.Text = "Mobile :";
            // 
            // EGrdNm
            // 
            this.EGrdNm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EGrdNm.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.EGrdNm.FormattingEnabled = true;
            this.EGrdNm.ItemHeight = 17;
            this.EGrdNm.Items.AddRange(new object[] {
            "7A side",
            "Ground A(5A side)",
            "Ground B(5A side)"});
            this.EGrdNm.Location = new System.Drawing.Point(162, 352);
            this.EGrdNm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EGrdNm.Name = "EGrdNm";
            this.EGrdNm.Size = new System.Drawing.Size(209, 25);
            this.EGrdNm.TabIndex = 149;
            // 
            // label154
            // 
            this.label154.AutoSize = true;
            this.label154.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label154.ForeColor = System.Drawing.Color.Black;
            this.label154.Location = new System.Drawing.Point(103, 120);
            this.label154.Name = "label154";
            this.label154.Size = new System.Drawing.Size(55, 16);
            this.label154.TabIndex = 158;
            this.label154.Text = "Name :";
            // 
            // label148
            // 
            this.label148.AutoSize = true;
            this.label148.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label148.ForeColor = System.Drawing.Color.Black;
            this.label148.Location = new System.Drawing.Point(65, 153);
            this.label148.Name = "label148";
            this.label148.Size = new System.Drawing.Size(93, 16);
            this.label148.TabIndex = 157;
            this.label148.Text = "Event Name :";
            // 
            // label149
            // 
            this.label149.AutoSize = true;
            this.label149.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label149.ForeColor = System.Drawing.Color.Black;
            this.label149.Location = new System.Drawing.Point(39, 16);
            this.label149.Name = "label149";
            this.label149.Size = new System.Drawing.Size(415, 35);
            this.label149.TabIndex = 156;
            this.label149.Text = "Book your event at our venue";
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox9.BackgroundImage")));
            this.pictureBox9.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox9.Image")));
            this.pictureBox9.Location = new System.Drawing.Point(744, 16);
            this.pictureBox9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(167, 157);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox9.TabIndex = 168;
            this.pictureBox9.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(23, 476);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 161;
            this.label1.Text = "Date :";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(198, 476);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 162;
            this.label2.Text = "Date :";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(373, 476);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 163;
            this.label3.Text = "Date :";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(548, 476);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 16);
            this.label4.TabIndex = 164;
            this.label4.Text = "Date :";
            this.label4.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(84, 471);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker1.TabIndex = 150;
            this.dateTimePicker1.Visible = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(257, 471);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker2.TabIndex = 152;
            this.dateTimePicker2.Visible = false;
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker3.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(430, 471);
            this.dateTimePicker3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker3.TabIndex = 151;
            this.dateTimePicker3.Visible = false;
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.CalendarFont = new System.Drawing.Font("Century Gothic", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker4.Font = new System.Drawing.Font("Century Gothic", 9.25F);
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Location = new System.Drawing.Point(602, 471);
            this.dateTimePicker4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.Size = new System.Drawing.Size(104, 23);
            this.dateTimePicker4.TabIndex = 153;
            this.dateTimePicker4.Visible = false;
            // 
            // Event
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.Controls.Add(this.Enoofdays);
            this.Controls.Add(this.ErefPrsn);
            this.Controls.Add(this.ETm);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.EComp);
            this.Controls.Add(this.label140);
            this.Controls.Add(this.EAdd);
            this.Controls.Add(this.EMob);
            this.Controls.Add(this.ENm);
            this.Controls.Add(this.EEvNm);
            this.Controls.Add(this.label141);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.EBook);
            this.Controls.Add(this.radioButton6);
            this.Controls.Add(this.label142);
            this.Controls.Add(this.dateTimePicker4);
            this.Controls.Add(this.dateTimePicker8);
            this.Controls.Add(this.dateTimePicker3);
            this.Controls.Add(this.dateTimePicker6);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker7);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dateTimePicker5);
            this.Controls.Add(this.label153);
            this.Controls.Add(this.label143);
            this.Controls.Add(this.label144);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label152);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label150);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label151);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label145);
            this.Controls.Add(this.label146);
            this.Controls.Add(this.label147);
            this.Controls.Add(this.EGrdNm);
            this.Controls.Add(this.label154);
            this.Controls.Add(this.label148);
            this.Controls.Add(this.label149);
            this.Controls.Add(this.pictureBox9);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Event";
            this.Size = new System.Drawing.Size(930, 511);
            this.Load += new System.EventHandler(this.Event_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Enoofdays;
        private System.Windows.Forms.TextBox ErefPrsn;
        private System.Windows.Forms.ComboBox ETm;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox ERem;
        private System.Windows.Forms.Label label138;
        private System.Windows.Forms.Label label139;
        private System.Windows.Forms.RichTextBox Efed;
        private System.Windows.Forms.TextBox EComp;
        private System.Windows.Forms.Label label140;
        private System.Windows.Forms.TextBox EAdd;
        private System.Windows.Forms.TextBox EMob;
        private System.Windows.Forms.TextBox ENm;
        private System.Windows.Forms.TextBox EEvNm;
        private System.Windows.Forms.Label label141;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button EBook;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Label label142;
        private System.Windows.Forms.DateTimePicker dateTimePicker8;
        private System.Windows.Forms.DateTimePicker dateTimePicker6;
        private System.Windows.Forms.DateTimePicker dateTimePicker7;
        private System.Windows.Forms.DateTimePicker dateTimePicker5;
        private System.Windows.Forms.Label label153;
        private System.Windows.Forms.Label label143;
        private System.Windows.Forms.Label label144;
        private System.Windows.Forms.Label label152;
        private System.Windows.Forms.Label label150;
        private System.Windows.Forms.Label label151;
        private System.Windows.Forms.Label label145;
        private System.Windows.Forms.Label label146;
        private System.Windows.Forms.Label label147;
        private System.Windows.Forms.ComboBox EGrdNm;
        private System.Windows.Forms.Label label154;
        private System.Windows.Forms.Label label148;
        private System.Windows.Forms.Label label149;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
    }
}
