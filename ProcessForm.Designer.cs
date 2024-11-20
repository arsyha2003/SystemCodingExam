namespace SystemCodingExam
{
    partial class ProcessForm
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
            progressBar1 = new ProgressBar();
            button1 = new Button();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(40, 33);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(433, 29);
            progressBar1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(182, 82);
            button1.Name = "button1";
            button1.Size = new Size(175, 33);
            button1.TabIndex = 1;
            button1.Text = "Отмена";
            button1.UseVisualStyleBackColor = true;
            button1.Click += StopButtonEvent;
            // 
            // ProcessForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(514, 240);
            Controls.Add(button1);
            Controls.Add(progressBar1);
            Name = "ProcessForm";
            Text = "ProcessForm";
            ResumeLayout(false);
        }

        #endregion

        private ProgressBar progressBar1;
        private Button button1;
    }
}