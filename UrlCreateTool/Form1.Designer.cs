namespace UrlCreateTool
{
    partial class Form1
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTables = new System.Windows.Forms.TextBox();
            this.txtLogs = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrlTemplate = new System.Windows.Forms.TextBox();
            this.BtnAddNewRecord = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtotalUrlTemplate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSimgleCount = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtExtendName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(582, 354);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "开始生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(299, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "字段和表(每个查询用\'--\'隔开，表和字段用\',\'隔开)：";
            // 
            // txtTables
            // 
            this.txtTables.Location = new System.Drawing.Point(11, 52);
            this.txtTables.Multiline = true;
            this.txtTables.Name = "txtTables";
            this.txtTables.Size = new System.Drawing.Size(669, 61);
            this.txtTables.TabIndex = 8;
            // 
            // txtLogs
            // 
            this.txtLogs.Location = new System.Drawing.Point(73, 381);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.Size = new System.Drawing.Size(669, 84);
            this.txtLogs.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "生成日志：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "Url模板：";
            // 
            // txtUrlTemplate
            // 
            this.txtUrlTemplate.Location = new System.Drawing.Point(62, 8);
            this.txtUrlTemplate.Name = "txtUrlTemplate";
            this.txtUrlTemplate.Size = new System.Drawing.Size(618, 21);
            this.txtUrlTemplate.TabIndex = 14;
            // 
            // BtnAddNewRecord
            // 
            this.BtnAddNewRecord.Location = new System.Drawing.Point(686, 21);
            this.BtnAddNewRecord.Name = "BtnAddNewRecord";
            this.BtnAddNewRecord.Size = new System.Drawing.Size(75, 39);
            this.BtnAddNewRecord.TabIndex = 15;
            this.BtnAddNewRecord.Text = "新增";
            this.BtnAddNewRecord.UseVisualStyleBackColor = true;
            this.BtnAddNewRecord.Click += new System.EventHandler(this.BtnAddNewRecord_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(686, 90);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtotalUrlTemplate
            // 
            this.txtotalUrlTemplate.Location = new System.Drawing.Point(14, 140);
            this.txtotalUrlTemplate.Multiline = true;
            this.txtotalUrlTemplate.Name = "txtotalUrlTemplate";
            this.txtotalUrlTemplate.Size = new System.Drawing.Size(735, 191);
            this.txtotalUrlTemplate.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "待生成RUL：";
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(86, 354);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(130, 21);
            this.txtDir.TabIndex = 19;
            this.txtDir.Text = "Article";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 357);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "目录名称：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(392, 357);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "单文件记录数：";
            // 
            // txtSimgleCount
            // 
            this.txtSimgleCount.Location = new System.Drawing.Point(476, 354);
            this.txtSimgleCount.Name = "txtSimgleCount";
            this.txtSimgleCount.Size = new System.Drawing.Size(100, 21);
            this.txtSimgleCount.TabIndex = 22;
            this.txtSimgleCount.Text = "1000";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(663, 354);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "停止线程";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 357);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 24;
            this.label7.Text = "文件后缀名：";
            // 
            // txtExtendName
            // 
            this.txtExtendName.Location = new System.Drawing.Point(305, 354);
            this.txtExtendName.Name = "txtExtendName";
            this.txtExtendName.Size = new System.Drawing.Size(72, 21);
            this.txtExtendName.TabIndex = 25;
            this.txtExtendName.Text = ".csv";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 468);
            this.Controls.Add(this.txtExtendName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSimgleCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtotalUrlTemplate);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.BtnAddNewRecord);
            this.Controls.Add(this.txtUrlTemplate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLogs);
            this.Controls.Add(this.txtTables);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCreate);
            this.Name = "Form1";
            this.Text = "url生成工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTables;
        private System.Windows.Forms.TextBox txtLogs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrlTemplate;
        private System.Windows.Forms.Button BtnAddNewRecord;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtotalUrlTemplate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSimgleCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtExtendName;
    }
}

