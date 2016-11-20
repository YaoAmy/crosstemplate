namespace Crawler
{
    partial class Crawler
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.wb1 = new System.Windows.Forms.WebBrowser();
            this.crawbutton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // wb1
            // 
            this.wb1.Dock = System.Windows.Forms.DockStyle.Left;
            this.wb1.Location = new System.Drawing.Point(0, 0);
            this.wb1.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb1.Name = "wb1";
            this.wb1.Size = new System.Drawing.Size(467, 412);
            this.wb1.TabIndex = 0;
            // 
            // crawbutton
            // 
            this.crawbutton.Location = new System.Drawing.Point(496, 30);
            this.crawbutton.Name = "crawbutton";
            this.crawbutton.Size = new System.Drawing.Size(75, 23);
            this.crawbutton.TabIndex = 1;
            this.crawbutton.Text = "crawler";
            this.crawbutton.UseVisualStyleBackColor = true;
            this.crawbutton.Click += new System.EventHandler(this.crawbutton_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Crawler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 412);
            this.Controls.Add(this.crawbutton);
            this.Controls.Add(this.wb1);
            this.Name = "Crawler";
            this.Text = "Crawler";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wb1;
        private System.Windows.Forms.Button crawbutton;
        private System.Windows.Forms.Timer timer1;
    }
}

