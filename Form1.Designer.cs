namespace SystemCodingExam
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            progressBar1 = new ProgressBar();
            button1 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 21);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(555, 29);
            progressBar1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(659, 21);
            button1.Name = "button1";
            button1.Size = new Size(215, 119);
            button1.TabIndex = 1;
            button1.Text = "Старт";
            button1.UseVisualStyleBackColor = true;
            button1.Click += StartButtonEvent;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 86);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(409, 27);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 63);
            label1.Name = "label1";
            label1.Size = new Size(411, 20);
            label1.TabIndex = 3;
            label1.Text = "Введите путь к директории куда будут отправлены файлы";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(14, 179);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(409, 241);
            textBox2.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 156);
            label2.Name = "label2";
            label2.Size = new Size(265, 20);
            label2.TabIndex = 5;
            label2.Text = "Введите слова-фильтр через пробел";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(886, 450);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(progressBar1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar1;
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
    }
}
