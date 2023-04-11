namespace SKYNET.Controls
{
    partial class SKYNET_Box
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
            this.PB_Image = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_Image
            // 
            this.PB_Image.BackColor = System.Drawing.Color.Transparent;
            this.PB_Image.Location = new System.Drawing.Point(11, 9);
            this.PB_Image.Name = "PB_Image";
            this.PB_Image.Size = new System.Drawing.Size(10, 10);
            this.PB_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PB_Image.TabIndex = 5;
            this.PB_Image.TabStop = false;
            this.PB_Image.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnClicked);
            this.PB_Image.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.PB_Image.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            // 
            // SKYNET_Box
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(54)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.PB_Image);
            this.Name = "SKYNET_Box";
            this.Size = new System.Drawing.Size(34, 26);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnClicked);
            this.MouseLeave += new System.EventHandler(this.OnMouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_Image;
    }
}
