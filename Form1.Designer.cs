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
            button1 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            button2 = new Button();
            label3 = new Label();
            button3 = new Button();
            button4 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(853, 124);
            button1.Name = "button1";
            button1.Size = new Size(302, 183);
            button1.TabIndex = 1;
            button1.Text = "Старт";
            button1.UseVisualStyleBackColor = true;
            button1.Click += StartButtonEvent;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 44);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(707, 27);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(411, 20);
            label1.TabIndex = 3;
            label1.Text = "Введите путь к директории куда будут отправлены файлы";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(14, 124);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(704, 296);
            textBox2.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 101);
            label2.Name = "label2";
            label2.Size = new Size(265, 20);
            label2.TabIndex = 5;
            label2.Text = "Введите слова-фильтр через пробел";
            // 
            // button2
            // 
            button2.Location = new Point(853, 11);
            button2.Name = "button2";
            button2.Size = new Size(302, 64);
            button2.TabIndex = 6;
            button2.Text = "Взять слова из файла";
            button2.UseVisualStyleBackColor = true;
            button2.Click += GetWordsFromFileButtonEvent;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(853, 78);
            label3.Name = "label3";
            label3.Size = new Size(302, 20);
            label3.TabIndex = 7;
            label3.Text = "*слова должны быть записаны в столбик*";
            // 
            // button3
            // 
            button3.Location = new Point(992, 326);
            button3.Name = "button3";
            button3.Size = new Size(163, 47);
            button3.TabIndex = 8;
            button3.Text = "Пауза";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Pause;
            // 
            // button4
            // 
            button4.Location = new Point(992, 391);
            button4.Name = "button4";
            button4.Size = new Size(163, 47);
            button4.TabIndex = 9;
            button4.Text = "Возобновить";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Unpause;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 450);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label3);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
        private Button button2;
        private Label label3;
        private Button button3;
        private Button button4;
    }
}
