using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SystemCodingExam
{
    internal static class Program
    {
        static Mutex mutex;
        [STAThread]
        static void Main(string[] args)
        {
            bool isNewApp = true;
            mutex = new Mutex(true, "individualMutex", out isNewApp);
            if (!isNewApp)
            {
                MessageBox.Show("Приложение уже запущено!!!");
                return;
            }
            ApplicationConfiguration.Initialize();
            try
            {
                if (args.ToList().Count != 0)
                {
                    Application.Run(new Form1(args));
                }
                else
                {
                    Application.Run(new Form1());
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

    }
}