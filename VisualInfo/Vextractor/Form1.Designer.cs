namespace Vextractor
{
    partial class Form1
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.extractv = new System.Windows.Forms.Button();
            this.Docfinished = new System.Windows.Forms.Timer(this.components);
            this.divide = new System.Windows.Forms.Button();
            this.Domtext = new System.Windows.Forms.TextBox();
            this.extract_info = new System.Windows.Forms.Button();
            this.extract_timer = new System.Windows.Forms.Timer(this.components);
            this.partition_timer = new System.Windows.Forms.Timer(this.components);
            this.Scoretor = new System.Windows.Forms.Button();
            this.weight_scores = new System.Windows.Forms.Button();
            this.weight_timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Left;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(250, 276);
            this.webBrowser1.TabIndex = 0;
            // 
            // extractv
            // 
            this.extractv.Location = new System.Drawing.Point(279, 32);
            this.extractv.Name = "extractv";
            this.extractv.Size = new System.Drawing.Size(75, 23);
            this.extractv.TabIndex = 1;
            this.extractv.Text = "extractv";
            this.extractv.UseVisualStyleBackColor = true;
            this.extractv.Click += new System.EventHandler(this.extractv_Click);
            // 
            // Docfinished
            // 
            this.Docfinished.Interval = 8000;
            this.Docfinished.Tick += new System.EventHandler(this.Docfinished_Tick);
            // 
            // divide
            // 
            this.divide.Location = new System.Drawing.Point(279, 80);
            this.divide.Name = "divide";
            this.divide.Size = new System.Drawing.Size(75, 23);
            this.divide.TabIndex = 2;
            this.divide.Text = "divide";
            this.divide.UseVisualStyleBackColor = true;
            this.divide.Click += new System.EventHandler(this.divide_Click);
            // 
            // Domtext
            // 
            this.Domtext.Location = new System.Drawing.Point(389, 32);
            this.Domtext.Multiline = true;
            this.Domtext.Name = "Domtext";
            this.Domtext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Domtext.Size = new System.Drawing.Size(194, 69);
            this.Domtext.TabIndex = 3;
            // 
            // extract_info
            // 
            this.extract_info.Location = new System.Drawing.Point(441, 108);
            this.extract_info.Name = "extract_info";
            this.extract_info.Size = new System.Drawing.Size(107, 23);
            this.extract_info.TabIndex = 4;
            this.extract_info.Text = "extract_info";
            this.extract_info.UseVisualStyleBackColor = true;
            this.extract_info.Click += new System.EventHandler(this.extract_info_Click);
            // 
            // extract_timer
            // 
            this.extract_timer.Interval = 3000;
            this.extract_timer.Tick += new System.EventHandler(this.extract_timer_Tick);
            // 
            // partition_timer
            // 
            this.partition_timer.Interval = 3000;
            this.partition_timer.Tick += new System.EventHandler(this.partition_timer_Tick);
            // 
            // Scoretor
            // 
            this.Scoretor.Location = new System.Drawing.Point(279, 129);
            this.Scoretor.Name = "Scoretor";
            this.Scoretor.Size = new System.Drawing.Size(75, 23);
            this.Scoretor.TabIndex = 5;
            this.Scoretor.Text = "Scoretor";
            this.Scoretor.UseVisualStyleBackColor = true;
            this.Scoretor.Click += new System.EventHandler(this.Scoretor_Click);
            // 
            // weight_scores
            // 
            this.weight_scores.Location = new System.Drawing.Point(279, 171);
            this.weight_scores.Name = "weight_scores";
            this.weight_scores.Size = new System.Drawing.Size(75, 23);
            this.weight_scores.TabIndex = 6;
            this.weight_scores.Text = "weight_scores";
            this.weight_scores.UseVisualStyleBackColor = true;
            this.weight_scores.Click += new System.EventHandler(this.weight_scores_Click);
            // 
            // weight_timer
            // 
            this.weight_timer.Interval = 3000;
            this.weight_timer.Tick += new System.EventHandler(this.weight_timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 276);
            this.Controls.Add(this.weight_scores);
            this.Controls.Add(this.Scoretor);
            this.Controls.Add(this.extract_info);
            this.Controls.Add(this.Domtext);
            this.Controls.Add(this.divide);
            this.Controls.Add(this.extractv);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "extracotr";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button extractv;
        private System.Windows.Forms.Timer Docfinished;
        private System.Windows.Forms.Button divide;
        private System.Windows.Forms.TextBox Domtext;
        private System.Windows.Forms.Button extract_info;
        private System.Windows.Forms.Timer extract_timer;
        private System.Windows.Forms.Timer partition_timer;
        private System.Windows.Forms.Button Scoretor;
        private System.Windows.Forms.Button weight_scores;
        private System.Windows.Forms.Timer weight_timer;
    }
}

