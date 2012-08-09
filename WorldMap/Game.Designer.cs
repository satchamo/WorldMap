namespace WorldMap
{
    partial class Game
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
            this.countryNameTextBox = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.scoreStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // countryNameTextBox
            // 
            this.countryNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.countryNameTextBox.Enabled = false;
            this.countryNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countryNameTextBox.Location = new System.Drawing.Point(12, 12);
            this.countryNameTextBox.Name = "countryNameTextBox";
            this.countryNameTextBox.ReadOnly = true;
            this.countryNameTextBox.Size = new System.Drawing.Size(781, 37);
            this.countryNameTextBox.TabIndex = 0;
            this.countryNameTextBox.Text = "Hi";
            this.countryNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.scoreStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 522);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(805, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Score:";
            // 
            // scoreStatusLabel
            // 
            this.scoreStatusLabel.Name = "scoreStatusLabel";
            this.scoreStatusLabel.Size = new System.Drawing.Size(24, 17);
            this.scoreStatusLabel.Text = "0/0";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 544);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.countryNameTextBox);
            this.Name = "Game";
            this.Text = "World Map";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox countryNameTextBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel scoreStatusLabel;










    }
}

