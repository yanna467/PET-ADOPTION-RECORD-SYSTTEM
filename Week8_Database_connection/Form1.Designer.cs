namespace Database_Connection
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            dataGridView1 = new DataGridView();
            load_data = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            textBox3 = new TextBox();
            label3 = new Label();
            Insert = new Button();
            Delete = new Button();
            Update = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label4 = new Label();
            YearlevelcomboBox = new ComboBox();
            coursetbx = new TextBox();
            label5 = new Label();
            Refreshbtn = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 26);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(104, 39);
            button1.TabIndex = 0;
            button1.Text = "Connection Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(386, 13);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(962, 682);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // load_data
            // 
            load_data.Location = new Point(139, 26);
            load_data.Margin = new Padding(3, 4, 3, 4);
            load_data.Name = "load_data";
            load_data.Size = new Size(110, 39);
            load_data.TabIndex = 2;
            load_data.Text = "Load File";
            load_data.UseVisualStyleBackColor = true;
            load_data.Click += loadBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 38);
            label1.Name = "label1";
            label1.Size = new Size(95, 20);
            label1.TabIndex = 3;
            label1.Text = "STUDENT ID:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(116, 27);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(88, 27);
            textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(116, 87);
            textBox2.Margin = new Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(170, 27);
            textBox2.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 95);
            label2.Name = "label2";
            label2.Size = new Size(91, 20);
            label2.TabIndex = 5;
            label2.Text = "LAST NAME:";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(116, 151);
            textBox3.Margin = new Padding(3, 4, 3, 4);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(213, 27);
            textBox3.TabIndex = 8;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 154);
            label3.Name = "label3";
            label3.Size = new Size(94, 20);
            label3.TabIndex = 7;
            label3.Text = "FIRST NAME:";
            // 
            // Insert
            // 
            Insert.Location = new Point(15, 556);
            Insert.Margin = new Padding(3, 4, 3, 4);
            Insert.Name = "Insert";
            Insert.Size = new Size(90, 56);
            Insert.TabIndex = 9;
            Insert.Text = "Insert";
            Insert.UseVisualStyleBackColor = true;
            Insert.Click += Insert_Click;
            // 
            // Delete
            // 
            Delete.Location = new Point(150, 556);
            Delete.Margin = new Padding(3, 4, 3, 4);
            Delete.Name = "Delete";
            Delete.Size = new Size(86, 56);
            Delete.TabIndex = 10;
            Delete.Text = "Delete";
            Delete.UseVisualStyleBackColor = true;
            Delete.Click += Delete_Click;
            // 
            // Update
            // 
            Update.Location = new Point(275, 556);
            Update.Margin = new Padding(3, 4, 3, 4);
            Update.Name = "Update";
            Update.Size = new Size(86, 56);
            Update.TabIndex = 11;
            Update.Text = "Update";
            Update.UseVisualStyleBackColor = true;
            Update.Click += Update_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(YearlevelcomboBox);
            groupBox1.Controls.Add(coursetbx);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Location = new Point(6, 106);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(374, 426);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "Student's Information";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioButton4);
            groupBox2.Controls.Add(radioButton3);
            groupBox2.Controls.Add(radioButton2);
            groupBox2.Controls.Add(radioButton1);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(20, 201);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(309, 110);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "YEAR LEVEL";
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(195, 64);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(83, 24);
            radioButton4.TabIndex = 14;
            radioButton4.TabStop = true;
            radioButton4.Text = "4th year\r\n";
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(58, 64);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(84, 24);
            radioButton3.TabIndex = 13;
            radioButton3.TabStop = true;
            radioButton3.Text = "3rd year\r\n";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(195, 31);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(87, 24);
            radioButton2.TabIndex = 12;
            radioButton2.TabStop = true;
            radioButton2.Text = "2nd year\r\n";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(58, 31);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(81, 24);
            radioButton1.TabIndex = 11;
            radioButton1.TabStop = true;
            radioButton1.Text = "1st year\r\n";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 30);
            label4.Name = "label4";
            label4.Size = new Size(0, 20);
            label4.TabIndex = 10;
            label4.Click += label4_Click;
            // 
            // YearlevelcomboBox
            // 
            YearlevelcomboBox.FormattingEnabled = true;
            YearlevelcomboBox.Location = new Point(117, 231);
            YearlevelcomboBox.Name = "YearlevelcomboBox";
            YearlevelcomboBox.Size = new Size(151, 28);
            YearlevelcomboBox.TabIndex = 13;
            // 
            // coursetbx
            // 
            coursetbx.Location = new Point(94, 339);
            coursetbx.Name = "coursetbx";
            coursetbx.Size = new Size(87, 27);
            coursetbx.TabIndex = 12;
            coursetbx.TextChanged += textBox5_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(9, 342);
            label5.Name = "label5";
            label5.Size = new Size(67, 20);
            label5.TabIndex = 11;
            label5.Text = "COURSE:";
            // 
            // Refreshbtn
            // 
            Refreshbtn.Location = new Point(275, 13);
            Refreshbtn.Margin = new Padding(3, 4, 3, 4);
            Refreshbtn.Name = "Refreshbtn";
            Refreshbtn.Size = new Size(97, 65);
            Refreshbtn.TabIndex = 13;
            Refreshbtn.Text = "REFRESH";
            Refreshbtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1353, 709);
            Controls.Add(Refreshbtn);
            Controls.Add(groupBox1);
            Controls.Add(Update);
            Controls.Add(Delete);
            Controls.Add(Insert);
            Controls.Add(load_data);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private DataGridView dataGridView1;
        private Button load_data;
        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private TextBox textBox3;
        private Label label3;
        private Button Insert;
        private Button Delete;
        private Button Update;
        private GroupBox groupBox1;
        private TextBox coursetbx;
        private Label label5;
        private Label label4;
        private Button Refreshbtn;
        private ComboBox YearlevelcomboBox;
        private GroupBox groupBox2;
        private RadioButton radioButton1;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
    }
}
