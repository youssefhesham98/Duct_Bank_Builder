namespace Duck_Bank_Builder.UI
{
    partial class Mainform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.getcenters = new System.Windows.Forms.Button();
            this.pipe_types = new System.Windows.Forms.ComboBox();
            this.system_types = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkd = new System.Windows.Forms.PictureBox();
            this.edecs = new System.Windows.Forms.PictureBox();
            this.cls = new System.Windows.Forms.PictureBox();
            this.userselection = new System.Windows.Forms.TextBox();
            this.crt_db = new System.Windows.Forms.Button();
            this.over_db = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.excellocation = new System.Windows.Forms.FolderBrowserDialog();
            this.savetitle = new System.Windows.Forms.TextBox();
            this.bwsexcel = new System.Windows.Forms.Button();
            this.savedirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lnkd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edecs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cls)).BeginInit();
            this.SuspendLayout();
            // 
            // getcenters
            // 
            this.getcenters.Location = new System.Drawing.Point(187, 217);
            this.getcenters.Name = "getcenters";
            this.getcenters.Size = new System.Drawing.Size(75, 25);
            this.getcenters.TabIndex = 1;
            this.getcenters.Text = "Create";
            this.getcenters.UseVisualStyleBackColor = true;
            this.getcenters.Click += new System.EventHandler(this.getcenters_Click);
            // 
            // pipe_types
            // 
            this.pipe_types.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.pipe_types.FormattingEnabled = true;
            this.pipe_types.Location = new System.Drawing.Point(12, 39);
            this.pipe_types.Name = "pipe_types";
            this.pipe_types.Size = new System.Drawing.Size(250, 21);
            this.pipe_types.TabIndex = 2;
            this.pipe_types.Text = "Pipe Types";
            // 
            // system_types
            // 
            this.system_types.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.system_types.FormattingEnabled = true;
            this.system_types.Location = new System.Drawing.Point(12, 12);
            this.system_types.Name = "system_types";
            this.system_types.Size = new System.Drawing.Size(250, 21);
            this.system_types.TabIndex = 3;
            this.system_types.Text = "System Types";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(83, 270);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "MEP TOOLKIT | DUCT BANK BUILDER ®\r\nBY YOUSSEF HESHAM | V 1.1.0";
            // 
            // lnkd
            // 
            this.lnkd.Image = global::Duck_Bank_Builder.Properties.Resources.LinkedIn;
            this.lnkd.Location = new System.Drawing.Point(12, 268);
            this.lnkd.Name = "lnkd";
            this.lnkd.Size = new System.Drawing.Size(28, 28);
            this.lnkd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.lnkd.TabIndex = 8;
            this.lnkd.TabStop = false;
            this.lnkd.Click += new System.EventHandler(this.lnkd_Click);
            // 
            // edecs
            // 
            this.edecs.Image = global::Duck_Bank_Builder.Properties.Resources.EDECS;
            this.edecs.Location = new System.Drawing.Point(268, 257);
            this.edecs.Name = "edecs";
            this.edecs.Size = new System.Drawing.Size(124, 51);
            this.edecs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.edecs.TabIndex = 4;
            this.edecs.TabStop = false;
            this.edecs.Click += new System.EventHandler(this.edecs_Click);
            // 
            // cls
            // 
            this.cls.Image = global::Duck_Bank_Builder.Properties.Resources.Close;
            this.cls.Location = new System.Drawing.Point(46, 270);
            this.cls.Name = "cls";
            this.cls.Size = new System.Drawing.Size(28, 28);
            this.cls.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cls.TabIndex = 9;
            this.cls.TabStop = false;
            this.cls.Click += new System.EventHandler(this.cls_Click);
            // 
            // userselection
            // 
            this.userselection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userselection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.userselection.Location = new System.Drawing.Point(12, 79);
            this.userselection.Name = "userselection";
            this.userselection.Size = new System.Drawing.Size(250, 21);
            this.userselection.TabIndex = 10;
            this.userselection.TextChanged += new System.EventHandler(this.userselection_TextChanged);
            // 
            // crt_db
            // 
            this.crt_db.Location = new System.Drawing.Point(93, 217);
            this.crt_db.Name = "crt_db";
            this.crt_db.Size = new System.Drawing.Size(75, 25);
            this.crt_db.TabIndex = 11;
            this.crt_db.Text = "Create DB";
            this.crt_db.UseVisualStyleBackColor = true;
            this.crt_db.Click += new System.EventHandler(this.crt_db_Click);
            // 
            // over_db
            // 
            this.over_db.Location = new System.Drawing.Point(12, 217);
            this.over_db.Name = "over_db";
            this.over_db.Size = new System.Drawing.Size(75, 25);
            this.over_db.TabIndex = 12;
            this.over_db.Text = "Override DB";
            this.over_db.UseVisualStyleBackColor = true;
            this.over_db.Click += new System.EventHandler(this.over_db_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "User Input";
            // 
            // savetitle
            // 
            this.savetitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.savetitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.savetitle.Location = new System.Drawing.Point(12, 119);
            this.savetitle.Name = "savetitle";
            this.savetitle.Size = new System.Drawing.Size(250, 21);
            this.savetitle.TabIndex = 14;
            // 
            // bwsexcel
            // 
            this.bwsexcel.Location = new System.Drawing.Point(12, 186);
            this.bwsexcel.Name = "bwsexcel";
            this.bwsexcel.Size = new System.Drawing.Size(250, 25);
            this.bwsexcel.TabIndex = 15;
            this.bwsexcel.Text = "Browse Excel and XML";
            this.bwsexcel.UseVisualStyleBackColor = true;
            this.bwsexcel.Click += new System.EventHandler(this.bwsexcel_Click);
            // 
            // savedirectory
            // 
            this.savedirectory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.savedirectory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.savedirectory.Location = new System.Drawing.Point(12, 159);
            this.savedirectory.Name = "savedirectory";
            this.savedirectory.Size = new System.Drawing.Size(250, 21);
            this.savedirectory.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Saving Title";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Saving Directory";
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(411, 305);
            this.Controls.Add(this.savedirectory);
            this.Controls.Add(this.bwsexcel);
            this.Controls.Add(this.savetitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.over_db);
            this.Controls.Add(this.crt_db);
            this.Controls.Add(this.userselection);
            this.Controls.Add(this.cls);
            this.Controls.Add(this.lnkd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edecs);
            this.Controls.Add(this.system_types);
            this.Controls.Add(this.pipe_types);
            this.Controls.Add(this.getcenters);
            this.Controls.Add(this.label2);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Mainform";
            this.Text = "Ductt Bank Builder";
            ((System.ComponentModel.ISupportInitialize)(this.lnkd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edecs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button getcenters;
        private System.Windows.Forms.ComboBox pipe_types;
        private System.Windows.Forms.ComboBox system_types;
        private System.Windows.Forms.PictureBox edecs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox lnkd;
        private System.Windows.Forms.PictureBox cls;
        private System.Windows.Forms.TextBox userselection;
        private System.Windows.Forms.Button crt_db;
        private System.Windows.Forms.Button over_db;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog excellocation;
        private System.Windows.Forms.TextBox savetitle;
        private System.Windows.Forms.Button bwsexcel;
        private System.Windows.Forms.TextBox savedirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}