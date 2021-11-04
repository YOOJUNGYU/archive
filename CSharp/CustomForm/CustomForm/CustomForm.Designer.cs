
namespace CustomForm
{
    partial class CustomForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnWindowMinimize = new System.Windows.Forms.Button();
            this.btnWindowMaximize = new System.Windows.Forms.Button();
            this.btnWindowRestore = new System.Windows.Forms.Button();
            this.btnWindowClose = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.pnlTitleBar.Controls.Add(this.lblTitle);
            this.pnlTitleBar.Controls.Add(this.btnWindowMinimize);
            this.pnlTitleBar.Controls.Add(this.btnWindowMaximize);
            this.pnlTitleBar.Controls.Add(this.btnWindowRestore);
            this.pnlTitleBar.Controls.Add(this.btnWindowClose);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(800, 35);
            this.pnlTitleBar.TabIndex = 3;
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitle.Location = new System.Drawing.Point(12, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(41, 12);
            this.lblTitle.TabIndex = 12;
            this.lblTitle.Text = "타이틀";
            // 
            // btnWindowMinimize
            // 
            this.btnWindowMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnWindowMinimize.FlatAppearance.BorderSize = 0;
            this.btnWindowMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnWindowMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowMinimize.Image = global::CustomForm.Properties.Resources.window_minimize;
            this.btnWindowMinimize.Location = new System.Drawing.Point(687, 0);
            this.btnWindowMinimize.Margin = new System.Windows.Forms.Padding(5);
            this.btnWindowMinimize.Name = "btnWindowMinimize";
            this.btnWindowMinimize.Size = new System.Drawing.Size(35, 35);
            this.btnWindowMinimize.TabIndex = 9;
            this.btnWindowMinimize.UseVisualStyleBackColor = true;
            this.btnWindowMinimize.Click += new System.EventHandler(this.btnWindowMinimize_Click);
            // 
            // btnWindowMaximize
            // 
            this.btnWindowMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowMaximize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnWindowMaximize.FlatAppearance.BorderSize = 0;
            this.btnWindowMaximize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnWindowMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowMaximize.Image = global::CustomForm.Properties.Resources.window_maximize;
            this.btnWindowMaximize.Location = new System.Drawing.Point(725, 0);
            this.btnWindowMaximize.Margin = new System.Windows.Forms.Padding(5);
            this.btnWindowMaximize.Name = "btnWindowMaximize";
            this.btnWindowMaximize.Size = new System.Drawing.Size(35, 35);
            this.btnWindowMaximize.TabIndex = 8;
            this.btnWindowMaximize.UseVisualStyleBackColor = true;
            this.btnWindowMaximize.Click += new System.EventHandler(this.btnWindowMaximize_Click);
            // 
            // btnWindowRestore
            // 
            this.btnWindowRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowRestore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnWindowRestore.FlatAppearance.BorderSize = 0;
            this.btnWindowRestore.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnWindowRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowRestore.Image = global::CustomForm.Properties.Resources.window_restore;
            this.btnWindowRestore.Location = new System.Drawing.Point(725, 0);
            this.btnWindowRestore.Margin = new System.Windows.Forms.Padding(5);
            this.btnWindowRestore.Name = "btnWindowRestore";
            this.btnWindowRestore.Size = new System.Drawing.Size(35, 35);
            this.btnWindowRestore.TabIndex = 10;
            this.btnWindowRestore.UseVisualStyleBackColor = true;
            this.btnWindowRestore.Click += new System.EventHandler(this.btnWindowRestore_Click);
            // 
            // btnWindowClose
            // 
            this.btnWindowClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindowClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnWindowClose.FlatAppearance.BorderSize = 0;
            this.btnWindowClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnWindowClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowClose.Image = global::CustomForm.Properties.Resources.window_close;
            this.btnWindowClose.Location = new System.Drawing.Point(763, 0);
            this.btnWindowClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnWindowClose.Name = "btnWindowClose";
            this.btnWindowClose.Size = new System.Drawing.Size(35, 35);
            this.btnWindowClose.TabIndex = 7;
            this.btnWindowClose.UseVisualStyleBackColor = true;
            this.btnWindowClose.Click += new System.EventHandler(this.btnWindowClose_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(800, 450);
            this.pnlMain.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.pnlTitleBar);
            this.Controls.Add(this.pnlMain);
            this.Name = "Form1";
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnWindowMinimize;
        private System.Windows.Forms.Button btnWindowMaximize;
        private System.Windows.Forms.Button btnWindowRestore;
        private System.Windows.Forms.Button btnWindowClose;
        private System.Windows.Forms.Panel pnlMain;
    }
}

