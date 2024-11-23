using System.IO;
using System.Text.RegularExpressions;

namespace SystemCodingExam
{
    public partial class Form1 : Form
    {
        private bool isConsole = false;
        private bool isFileOpened = false;
        private bool isPaused = false;

        private string[] args = null;
        private string consoleFilePath = string.Empty;
        private string outputDirectory = string.Empty;
        private string logFilePath = "LogFile.txt";

        private List<string> secretWords = new List<string>();
        private List<string> logFileData = new List<string>();

        private ProcessForm processForm;

        private CancellationTokenSource cts;
        private ManualResetEvent manualResetEvent = new ManualResetEvent(true);
        public Form1()
        {
            InitializeComponent();
            isConsole = false;
        }
        public Form1(string[] args)
        {
            InitializeComponent();
            isConsole = true;
            this.args = args;
            outputDirectory = args[0].Trim('"');
            Text = isConsole.ToString();
            textBox1.Text = outputDirectory;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(logFilePath)) File.Create(logFilePath);
            if (isConsole)
            {
                if (args[1].Trim('"').Contains(".txt"))
                {
                    GetDataFromFileAsync(args[1].Trim('"'));
                }
                else
                {
                    foreach (string word in args[1].Trim('"').Split())
                    {
                        textBox2.Text += word + " ";
                    }
                }
                StartProgramAsync();
            }
        }
        private async Task GetDataFromFileAsync(string filePath)
        {
            await Task.Run(() =>
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line = string.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            secretWords.Add(line);
                            textBox2.Text += line + " ";
                        }
                    }
                }
            });
        }
        private async void GetWordsFromFileButtonEvent(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string filePath = ofd.FileName;
            GetDataFromFileAsync(filePath);
        }
        private async Task AwaitAllTasksAsync(Task[] tasks)
        {
            await Task.Run(() =>
            {
                Task.WaitAll(tasks);
                MessageBox.Show("Обработка завершена!", string.Empty, MessageBoxButtons.OK);
                using (FileStream fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (string line in logFileData)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
            });
        }
        private async void StartProgramAsync()
        {
            cts = new CancellationTokenSource();
            button1.Enabled = false;

            secretWords = textBox2.Text.Split().ToList();
            outputDirectory = @textBox1.Text;
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch { MessageBox.Show("Путь указан некорректно."); return; }
                    MessageBox.Show("Директории которую вы ввели не существует, программа создала ее автоматически.", string.Empty, MessageBoxButtons.OK);
                }
                var drives = DriveInfo.GetDrives().ToList();

                processForm = new ProcessForm(cts);
                processForm.max = drives.Count();
                processForm.SetMaximum();
                processForm.Show();

                List<Task> tasks = new List<Task>();
                foreach (var drive in drives)
                {
                    tasks.Add(Task.Run(() => RecursiveDirectorySearch(drive.Name)));
                }
                await AwaitAllTasksAsync(tasks.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Обработка была прервана!", string.Empty, MessageBoxButtons.OK);
                Log($"Ошибка: {ex.Message} {DateTime.Now}");
            }
            finally
            {
                button1.Enabled = true;
            }
        }
        private async void StartButtonEvent(object sender, EventArgs e) => StartProgramAsync();
        private void RecursiveDirectorySearch(string path)
        {
            try
            {
                foreach (var file in Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly))
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        processForm.Close();
                        return;
                    }
                    if(isPaused == true)
                    {
                        manualResetEvent.WaitOne();
                    }
                    ProcessingTxtFile(file);
                    processForm.max += 1;
                    processForm.SetMaximum();
                    processForm.UpdateProgress(isPaused);

                }
                foreach (var directory in Directory.GetDirectories(path))
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        processForm.Close();
                        return;
                    }
                    if (isPaused == true)
                    {
                        manualResetEvent.WaitOne();
                    }
                    RecursiveDirectorySearch(directory);
                    processForm.max += 1;
                    processForm.SetMaximum();
                    processForm.UpdateProgress(isPaused);
                }
            }
            catch (Exception ex)
            {
                Log($"Ошибка: {ex.Message} {DateTime.Now}");
            }
        }
        private void ProcessingTxtFile(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                bool isWordsFound = secretWords.Any(w => content.Contains(w));

                if (isWordsFound)
                {
                    string filteredContent = string.Empty;
                    foreach (string word in secretWords)
                    {
                        filteredContent = secretWords.Aggregate(content, (current, word) =>
                        current.Replace(word, "*******", StringComparison.OrdinalIgnoreCase));
                    }
                    string newFilePath = Path.Combine(outputDirectory, Path.GetFileName(filePath));
                    File.WriteAllText(newFilePath, filteredContent);

                    Log($"Обработан файл: {filePath}  {DateTime.Now}");
                    Text = $"Обработан файл: {filePath}  {DateTime.Now}";
                }
            }
            catch (Exception ex)
            {
                Log($"Ошибка при обработке файла {filePath}: {ex.Message} {DateTime.Now}");
            }
        }
        private void Log(string message)
        {
            logFileData.Add(message);
        }
        private void StopButtonEvent(object sender, EventArgs e)
        {
            cts.Cancel();
        }
        private void Pause(object sender, EventArgs e)
        {
            manualResetEvent.Reset();
            isPaused = true;
        }
        private void Unpause(object sender, EventArgs e)
        {
            manualResetEvent.Set();
            isPaused = false;
        }

    }
   
}
