namespace QuickNotes3
{
    partial class PictureButton
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
            this.MainButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MainButton)).BeginInit();
            this.SuspendLayout();
            // 
            // MainButton
            // 
            this.MainButton.BackColor = System.Drawing.Color.Black;
            this.MainButton.Location = new System.Drawing.Point(1, 1);
            this.MainButton.Name = "MainButton";
            this.MainButton.Size = new System.Drawing.Size(21, 21);
            this.MainButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MainButton.TabIndex = 0;
            this.MainButton.TabStop = false;
            // 
            // PictureButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.MainButton);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "PictureButton";
            this.Size = new System.Drawing.Size(23, 23);
            this.Load += new System.EventHandler(this.PictureButton_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MainButton;
    }
}
