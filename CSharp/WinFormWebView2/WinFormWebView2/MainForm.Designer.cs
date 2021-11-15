
namespace WinFormWebView2
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
            this.pnlForm = new System.Windows.Forms.Panel();
            this.flpButtonZone = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTest1 = new System.Windows.Forms.Button();
            this.btnTest2 = new System.Windows.Forms.Button();
            this.btnTest3 = new System.Windows.Forms.Button();
            this.flpButtonZone.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlForm
            // 
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 29);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(1021, 631);
            this.pnlForm.TabIndex = 1;
            // 
            // flpButtonZone
            // 
            this.flpButtonZone.AutoSize = true;
            this.flpButtonZone.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButtonZone.Controls.Add(this.btnTest1);
            this.flpButtonZone.Controls.Add(this.btnTest2);
            this.flpButtonZone.Controls.Add(this.btnTest3);
            this.flpButtonZone.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpButtonZone.Location = new System.Drawing.Point(0, 0);
            this.flpButtonZone.Name = "flpButtonZone";
            this.flpButtonZone.Size = new System.Drawing.Size(1021, 29);
            this.flpButtonZone.TabIndex = 0;
            // 
            // btnTest1
            // 
            this.btnTest1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest1.Location = new System.Drawing.Point(3, 3);
            this.btnTest1.Name = "btnTest1";
            this.btnTest1.Size = new System.Drawing.Size(75, 23);
            this.btnTest1.TabIndex = 0;
            this.btnTest1.Text = "TEST1";
            this.btnTest1.UseVisualStyleBackColor = true;
            this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
            // 
            // btnTest2
            // 
            this.btnTest2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest2.Location = new System.Drawing.Point(84, 3);
            this.btnTest2.Name = "btnTest2";
            this.btnTest2.Size = new System.Drawing.Size(75, 23);
            this.btnTest2.TabIndex = 1;
            this.btnTest2.Text = "TEST2";
            this.btnTest2.UseVisualStyleBackColor = true;
            this.btnTest2.Click += new System.EventHandler(this.btnTest2_Click);
            // 
            // btnTest3
            // 
            this.btnTest3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest3.Location = new System.Drawing.Point(165, 3);
            this.btnTest3.Name = "btnTest3";
            this.btnTest3.Size = new System.Drawing.Size(75, 23);
            this.btnTest3.TabIndex = 2;
            this.btnTest3.Text = "TEST3";
            this.btnTest3.UseVisualStyleBackColor = true;
            this.btnTest3.Click += new System.EventHandler(this.btnTest3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 660);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.flpButtonZone);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.flpButtonZone.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.FlowLayoutPanel flpButtonZone;
        private System.Windows.Forms.Button btnTest1;
        private System.Windows.Forms.Button btnTest2;
        private System.Windows.Forms.Button btnTest3;
    }
}