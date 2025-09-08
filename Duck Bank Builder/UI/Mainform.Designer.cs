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
            ((System.ComponentModel.ISupportInitialize)(this.lnkd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edecs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cls)).BeginInit();
            this.SuspendLayout();
            // 
            // getcenters
            // 
            this.getcenters.Location = new System.Drawing.Point(530, 12);
            this.getcenters.Name = "getcenters";
            this.getcenters.Size = new System.Drawing.Size(75, 21);
            this.getcenters.TabIndex = 1;
            this.getcenters.Text = "Create";
            this.getcenters.UseVisualStyleBackColor = true;
            this.getcenters.Click += new System.EventHandler(this.getcenters_Click);
            // 
            // pipe_types
            // 
            this.pipe_types.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.pipe_types.FormattingEnabled = true;
            this.pipe_types.Location = new System.Drawing.Point(12, 12);
            this.pipe_types.Name = "pipe_types";
            this.pipe_types.Size = new System.Drawing.Size(250, 21);
            this.pipe_types.TabIndex = 2;
            this.pipe_types.Text = "Pipe Types";
            // 
            // system_types
            // 
            this.system_types.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.system_types.FormattingEnabled = true;
            this.system_types.Location = new System.Drawing.Point(274, 12);
            this.system_types.Name = "system_types";
            this.system_types.Size = new System.Drawing.Size(250, 21);
            this.system_types.TabIndex = 3;
            this.system_types.Text = "System Types";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "MEP TOOLKIT | DUCT BANK BUILDER ® | BY YOUSSEF HESHAM | V 1.1.0";
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
            this.edecs.Location = new System.Drawing.Point(497, 258);
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
            this.userselection.Location = new System.Drawing.Point(12, 51);
            this.userselection.Name = "userselection";
            this.userselection.Size = new System.Drawing.Size(250, 21);
            this.userselection.TabIndex = 10;
            this.userselection.TextChanged += new System.EventHandler(this.userselection_TextChanged);
            // 
            // crt_db
            // 
            this.crt_db.Location = new System.Drawing.Point(530, 49);
            this.crt_db.Name = "crt_db";
            this.crt_db.Size = new System.Drawing.Size(75, 23);
            this.crt_db.TabIndex = 11;
            this.crt_db.Text = "Create DB";
            this.crt_db.UseVisualStyleBackColor = true;
            this.crt_db.Click += new System.EventHandler(this.crt_db_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(619, 305);
            this.Controls.Add(this.crt_db);
            this.Controls.Add(this.userselection);
            this.Controls.Add(this.cls);
            this.Controls.Add(this.lnkd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edecs);
            this.Controls.Add(this.system_types);
            this.Controls.Add(this.pipe_types);
            this.Controls.Add(this.getcenters);
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
    }
}