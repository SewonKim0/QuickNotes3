namespace QuickNotes3
{
    partial class MainForm
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
            this.Doc = new System.Windows.Forms.RichTextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.DocPath = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.FindInput = new System.Windows.Forms.TextBox();
            this.FindUpButton = new System.Windows.Forms.Button();
            this.FindDownButton = new System.Windows.Forms.Button();
            this.Sections = new System.Windows.Forms.ComboBox();
            this.Backups = new System.Windows.Forms.ComboBox();
            this.BackupButton = new QuickNotes3.PictureButton();
            this.SectionButton = new QuickNotes3.PictureButton();
            this.FindButton = new QuickNotes3.PictureButton();
            this.SuspendLayout();
            // 
            // Doc
            // 
            this.Doc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Doc.BackColor = System.Drawing.Color.Black;
            this.Doc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Doc.ForeColor = System.Drawing.Color.White;
            this.Doc.Location = new System.Drawing.Point(0, 22);
            this.Doc.Name = "Doc";
            this.Doc.Size = new System.Drawing.Size(385, 238);
            this.Doc.TabIndex = 0;
            this.Doc.TabStop = false;
            this.Doc.Text = "";
            this.Doc.TextChanged += new System.EventHandler(this.Doc_TextChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackColor = System.Drawing.Color.Black;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.ForeColor = System.Drawing.Color.White;
            this.SaveButton.Location = new System.Drawing.Point(329, 0);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(55, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.TabStop = false;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadButton.BackColor = System.Drawing.Color.Black;
            this.LoadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadButton.ForeColor = System.Drawing.Color.White;
            this.LoadButton.Location = new System.Drawing.Point(275, 0);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(54, 23);
            this.LoadButton.TabIndex = 2;
            this.LoadButton.TabStop = false;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = false;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // DocPath
            // 
            this.DocPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DocPath.AutoEllipsis = true;
            this.DocPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocPath.ForeColor = System.Drawing.Color.White;
            this.DocPath.Location = new System.Drawing.Point(0, 3);
            this.DocPath.Name = "DocPath";
            this.DocPath.Size = new System.Drawing.Size(136, 16);
            this.DocPath.TabIndex = 3;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteButton.BackColor = System.Drawing.Color.Black;
            this.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.ForeColor = System.Drawing.Color.White;
            this.DeleteButton.Location = new System.Drawing.Point(211, 0);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(64, 23);
            this.DeleteButton.TabIndex = 4;
            this.DeleteButton.TabStop = false;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = false;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FindInput
            // 
            this.FindInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindInput.BackColor = System.Drawing.Color.Black;
            this.FindInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindInput.ForeColor = System.Drawing.Color.White;
            this.FindInput.Location = new System.Drawing.Point(0, 33);
            this.FindInput.Name = "FindInput";
            this.FindInput.Size = new System.Drawing.Size(121, 24);
            this.FindInput.TabIndex = 6;
            this.FindInput.TabStop = false;
            this.FindInput.Visible = false;
            // 
            // FindUpButton
            // 
            this.FindUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindUpButton.BackColor = System.Drawing.Color.Black;
            this.FindUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindUpButton.ForeColor = System.Drawing.Color.White;
            this.FindUpButton.Location = new System.Drawing.Point(121, 34);
            this.FindUpButton.Name = "FindUpButton";
            this.FindUpButton.Size = new System.Drawing.Size(21, 10);
            this.FindUpButton.TabIndex = 8;
            this.FindUpButton.TabStop = false;
            this.FindUpButton.UseVisualStyleBackColor = false;
            this.FindUpButton.Visible = false;
            this.FindUpButton.Click += new System.EventHandler(this.FindUpButton_Click);
            // 
            // FindDownButton
            // 
            this.FindDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindDownButton.BackColor = System.Drawing.Color.Black;
            this.FindDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindDownButton.ForeColor = System.Drawing.Color.White;
            this.FindDownButton.Location = new System.Drawing.Point(121, 45);
            this.FindDownButton.Name = "FindDownButton";
            this.FindDownButton.Size = new System.Drawing.Size(21, 10);
            this.FindDownButton.TabIndex = 9;
            this.FindDownButton.TabStop = false;
            this.FindDownButton.UseVisualStyleBackColor = false;
            this.FindDownButton.Visible = false;
            this.FindDownButton.Click += new System.EventHandler(this.FindDownButton_Click);
            // 
            // Sections
            // 
            this.Sections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Sections.BackColor = System.Drawing.Color.Black;
            this.Sections.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sections.ForeColor = System.Drawing.Color.White;
            this.Sections.FormattingEnabled = true;
            this.Sections.Location = new System.Drawing.Point(0, 64);
            this.Sections.Name = "Sections";
            this.Sections.Size = new System.Drawing.Size(142, 24);
            this.Sections.TabIndex = 11;
            this.Sections.TabStop = false;
            this.Sections.Visible = false;
            this.Sections.SelectedIndexChanged += new System.EventHandler(this.Sections_SelectedIndexChanged);
            // 
            // Backups
            // 
            this.Backups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Backups.BackColor = System.Drawing.Color.Black;
            this.Backups.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Backups.ForeColor = System.Drawing.Color.White;
            this.Backups.FormattingEnabled = true;
            this.Backups.Location = new System.Drawing.Point(0, 94);
            this.Backups.Name = "Backups";
            this.Backups.Size = new System.Drawing.Size(142, 24);
            this.Backups.TabIndex = 16;
            this.Backups.TabStop = false;
            this.Backups.Visible = false;
            this.Backups.SelectedIndexChanged += new System.EventHandler(this.Backups_SelectedIndexChanged);
            // 
            // BackupButton
            // 
            this.BackupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BackupButton.BackColor = System.Drawing.Color.DarkGray;
            this.BackupButton.ForeColor = System.Drawing.Color.White;
            this.BackupButton.Location = new System.Drawing.Point(142, 0);
            this.BackupButton.Name = "BackupButton";
            this.BackupButton.Size = new System.Drawing.Size(23, 23);
            this.BackupButton.TabIndex = 15;
            this.BackupButton.TabStop = false;
            // 
            // SectionButton
            // 
            this.SectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SectionButton.BackColor = System.Drawing.Color.DarkGray;
            this.SectionButton.ForeColor = System.Drawing.Color.White;
            this.SectionButton.Location = new System.Drawing.Point(165, 0);
            this.SectionButton.Name = "SectionButton";
            this.SectionButton.Size = new System.Drawing.Size(23, 23);
            this.SectionButton.TabIndex = 14;
            this.SectionButton.TabStop = false;
            // 
            // FindButton
            // 
            this.FindButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindButton.BackColor = System.Drawing.Color.DarkGray;
            this.FindButton.ForeColor = System.Drawing.Color.White;
            this.FindButton.Location = new System.Drawing.Point(188, 0);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(23, 23);
            this.FindButton.TabIndex = 13;
            this.FindButton.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(385, 260);
            this.Controls.Add(this.Backups);
            this.Controls.Add(this.BackupButton);
            this.Controls.Add(this.SectionButton);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.Sections);
            this.Controls.Add(this.FindDownButton);
            this.Controls.Add(this.FindUpButton);
            this.Controls.Add(this.FindInput);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.DocPath);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.Doc);
            this.Name = "MainForm";
            this.Text = "QuickNotes3";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox Doc;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Label DocPath;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.TextBox FindInput;
        private System.Windows.Forms.Button FindUpButton;
        private System.Windows.Forms.Button FindDownButton;
        private System.Windows.Forms.ComboBox Sections;
        private PictureButton FindButton;
        private PictureButton SectionButton;
        private PictureButton BackupButton;
        private System.Windows.Forms.ComboBox Backups;
    }
}

