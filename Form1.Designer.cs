namespace CybosAutoLogin
{
    partial class Form1
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
            this.buttonItemClick = new System.Windows.Forms.Button();
            this.textBoxItemClick = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonGetColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonItemClick
            // 
            this.buttonItemClick.Location = new System.Drawing.Point(210, 100);
            this.buttonItemClick.Name = "buttonItemClick";
            this.buttonItemClick.Size = new System.Drawing.Size(100, 23);
            this.buttonItemClick.TabIndex = 0;
            this.buttonItemClick.Text = "버튼클릭";
            this.buttonItemClick.UseVisualStyleBackColor = true;
            this.buttonItemClick.Click += new System.EventHandler(this.buttonBtnClick_Click);
            // 
            // textBoxItemClick
            // 
            this.textBoxItemClick.Location = new System.Drawing.Point(210, 39);
            this.textBoxItemClick.Name = "textBoxItemClick";
            this.textBoxItemClick.Size = new System.Drawing.Size(100, 21);
            this.textBoxItemClick.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(210, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "텍스트입력";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(210, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(191, 21);
            this.button2.TabIndex = 5;
            this.button2.Text = "매크로동작";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(235, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "테스트";
            // 
            // buttonGetColor
            // 
            this.buttonGetColor.Location = new System.Drawing.Point(210, 158);
            this.buttonGetColor.Name = "buttonGetColor";
            this.buttonGetColor.Size = new System.Drawing.Size(100, 23);
            this.buttonGetColor.TabIndex = 7;
            this.buttonGetColor.Text = "픽셀가져오기";
            this.buttonGetColor.UseVisualStyleBackColor = true;
            this.buttonGetColor.Click += new System.EventHandler(this.buttonGetColor_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "By REinitiate";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 273);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonGetColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxItemClick);
            this.Controls.Add(this.buttonItemClick);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonItemClick;
        private System.Windows.Forms.TextBox textBoxItemClick;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGetColor;
        private System.Windows.Forms.Label label2;
    }
}

