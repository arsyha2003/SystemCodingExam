using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SystemCodingExam
{
    public partial class ProcessForm: Form
    {
        public int max = 0;
        CancellationTokenSource cts = new CancellationTokenSource();
        public ProcessForm(CancellationTokenSource cts)
        {
            InitializeComponent();
            this.cts = cts;
        }
        public void SetMaximum()
        {
            progressBar1.Maximum = max;
        }

        public void UpdateProgress()
        {
            progressBar1.Value++;
        }
        private void StopButtonEvent(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}
