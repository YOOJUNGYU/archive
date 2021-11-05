
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
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlResizeBorderRight = new System.Windows.Forms.Panel();
            this.pnlResizeBorderBottom = new System.Windows.Forms.Panel();
            this.pnlResizeBorderLeft = new System.Windows.Forms.Panel();
            this.pnlResizeBorderTop = new System.Windows.Forms.Panel();
            this.pnlTitleBar.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
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
            this.pnlMain.Location = new System.Drawing.Point(0, 35);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(800, 415);
            this.pnlMain.TabIndex = 4;
            // 
            // pnlCenter
            // 
            this.pnlCenter.Controls.Add(this.pnlMain);
            this.pnlCenter.Controls.Add(this.pnlTitleBar);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(0, 0);
            this.pnlCenter.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(800, 450);
            this.pnlCenter.TabIndex = 0;
            // 
            // pnlResizeBorderRight
            // 
            this.pnlResizeBorderRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlResizeBorderRight.Location = new System.Drawing.Point(795, 0);
            this.pnlResizeBorderRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlResizeBorderRight.Name = "pnlResizeBorderRight";
            this.pnlResizeBorderRight.Size = new System.Drawing.Size(5, 450);
            this.pnlResizeBorderRight.TabIndex = 1;
            this.pnlResizeBorderRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderRight_MouseDown);
            this.pnlResizeBorderRight.MouseLeave += new System.EventHandler(this.pnlResizeBorder_MouseLeave);
            this.pnlResizeBorderRight.MouseHover += new System.EventHandler(this.pnlResizeBorderRight_MouseHover);
            this.pnlResizeBorderRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderRight_MouseMove);
            this.pnlResizeBorderRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderRight_MouseUp);
            // 
            // pnlResizeBorderBottom
            // 
            this.pnlResizeBorderBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlResizeBorderBottom.Location = new System.Drawing.Point(0, 445);
            this.pnlResizeBorderBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlResizeBorderBottom.Name = "pnlResizeBorderBottom";
            this.pnlResizeBorderBottom.Size = new System.Drawing.Size(795, 5);
            this.pnlResizeBorderBottom.TabIndex = 2;
            this.pnlResizeBorderBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderBottom_MouseDown);
            this.pnlResizeBorderBottom.MouseLeave += new System.EventHandler(this.pnlResizeBorder_MouseLeave);
            this.pnlResizeBorderBottom.MouseHover += new System.EventHandler(this.pnlResizeBorderBottom_MouseHover);
            this.pnlResizeBorderBottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderBottom_MouseMove);
            this.pnlResizeBorderBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderBottom_MouseUp);
            // 
            // pnlResizeBorderLeft
            // 
            this.pnlResizeBorderLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlResizeBorderLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlResizeBorderLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlResizeBorderLeft.Name = "pnlResizeBorderLeft";
            this.pnlResizeBorderLeft.Size = new System.Drawing.Size(5, 445);
            this.pnlResizeBorderLeft.TabIndex = 3;
            this.pnlResizeBorderLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderLeft_MouseDown);
            this.pnlResizeBorderLeft.MouseLeave += new System.EventHandler(this.pnlResizeBorder_MouseLeave);
            this.pnlResizeBorderLeft.MouseHover += new System.EventHandler(this.pnlResizeBorderLeft_MouseHover);
            this.pnlResizeBorderLeft.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderLeft_MouseMove);
            this.pnlResizeBorderLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderLeft_MouseUp);
            // 
            // pnlResizeBorderTop
            // 
            this.pnlResizeBorderTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlResizeBorderTop.Location = new System.Drawing.Point(5, 0);
            this.pnlResizeBorderTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlResizeBorderTop.Name = "pnlResizeBorderTop";
            this.pnlResizeBorderTop.Size = new System.Drawing.Size(790, 5);
            this.pnlResizeBorderTop.TabIndex = 4;
            this.pnlResizeBorderTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderTop_MouseDown);
            this.pnlResizeBorderTop.MouseLeave += new System.EventHandler(this.pnlResizeBorder_MouseLeave);
            this.pnlResizeBorderTop.MouseHover += new System.EventHandler(this.pnlResizeBorderTop_MouseHover);
            this.pnlResizeBorderTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderTop_MouseMove);
            this.pnlResizeBorderTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlResizeBorderTop_MouseUp);
            // 
            // CustomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlResizeBorderTop);
            this.Controls.Add(this.pnlResizeBorderLeft);
            this.Controls.Add(this.pnlResizeBorderBottom);
            this.Controls.Add(this.pnlResizeBorderRight);
            this.Controls.Add(this.pnlCenter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(450, 300);
            this.Name = "CustomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.pnlCenter.ResumeLayout(false);
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
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlResizeBorderRight;
        private System.Windows.Forms.Panel pnlResizeBorderBottom;
        private System.Windows.Forms.Panel pnlResizeBorderLeft;
        private System.Windows.Forms.Panel pnlResizeBorderTop;
    }
}

