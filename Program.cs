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
        static async Task Main(string[] args)
        {
            Form1 form = new Form1();
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
                    form.outputDirectory = args.ToList()[0];
                    if (args[1].Trim('"').Contains(".txt"))
                    {
                        form.GetDataFromFileAsync(args[1].Trim('"'));
                    }
                    else
                    {
                        foreach (string word in args[1].Trim('"').Split())
                        {
                            form.secretWords.Add(word + " ");
                        }
                    }
                    foreach (var i in form.secretWords)
                    {
                        form.countOfWords.Add(i, 0);
                    }
                    form.StartProgramAsync();
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