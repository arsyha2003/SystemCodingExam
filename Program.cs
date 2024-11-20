namespace SystemCodingExam
{
    internal static class Program
    {
        static List<string> secretWords = new List<string>();
        static List<string> logFileData = new List<string>();
        static Mutex mutex;
        [STAThread]
        static void Main(string[]args)
        {
            bool isNewApp = true;
            mutex = new Mutex(true, "individualMutex",out isNewApp);
            if(!isNewApp)
            {
                MessageBox.Show("Приложение уже запущено!!!");
                return;
            }
            ApplicationConfiguration.Initialize();            
            try
            {
                if(args.ToList().Count!=0)
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