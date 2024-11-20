using System.IO;
using System.Text.RegularExpressions;

namespace SystemCodingExam
{
    public partial class Form1 : Form
    {
        private string[] args = null;
        private string outputDirectory = string.Empty;
        private string logFilePath = "LogFile.txt";
        private int countOfFiles = 0;
        private List<string> secretWords = new List<string>();
        private List<string> logFileData = new List<string>();
        private ProcessForm processForm;
        private CancellationTokenSource cts;
        public Form1()
        {
            if (!File.Exists(logFilePath)) File.Create(logFilePath);
            InitializeComponent();
        }
        public Form1(string[] args)
        {
            this.args = args;
            if (!File.Exists(logFilePath)) File.Create(logFilePath);
            InitializeComponent();
            Text = args.Length.ToString();
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
                            textBox2.Text += line + "\n";
                        }
                    }
                }
            });
        }
        private async void GetWordsFromFile(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string filePath = ofd.FileName;
            GetDataFromFileAsync(filePath);
        }
        private async Task AwaitAllTasks(Task[] tasks)
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
        private async void StartButtonEvent(object sender, EventArgs e)
        {
            secretWords = textBox2.Text.Split().ToList();
            countOfFiles = 0;
            outputDirectory = @textBox1.Text;
            button1.Enabled = false;

            cts = new CancellationTokenSource();

            try
            {
                if(!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch { MessageBox.Show("Путь указан некорректно."); return; }
                    MessageBox.Show("Директории которую вы ввели не существует, программа создала ее автоматически.",string.Empty, MessageBoxButtons.OK);
                }
                var drives = DriveInfo.GetDrives().ToList();

                processForm = new ProcessForm(cts);
                processForm.max = drives.Count();
                processForm.SetMaximum();
                processForm.Show();

                List<Task> tasks = new List<Task>();
                foreach(var drive in drives)
                {
                    tasks.Add(Task.Run(() => RecurseDirectory(drive.Name)));
                }
                await AwaitAllTasks(tasks.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Обработка была прервана! "+ex.Message, string.Empty, MessageBoxButtons.OK);
                Log($"Ошибка: {ex.Message} {DateTime.Now}");
            }
            finally
            {
                button1.Enabled = true;
            }
        }
        private void RecurseDirectory(string path)
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
                    ProcessFile(file);
                    processForm.max += 1;
                    processForm.SetMaximum();
                    processForm.UpdateProgress();

                }
                foreach (var directory in Directory.GetDirectories(path))
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        processForm.Close();
                        return;
                    }
                    RecurseDirectory(directory);
                    processForm.max += 1;
                    processForm.SetMaximum();
                    processForm.UpdateProgress();
                }
            }
            catch (Exception ex)
            {
                Log($"Ошибка: {ex.Message} {DateTime.Now}");
            }
        }
        private void ProcessFile(string filePath)
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
                Log($"Ошибка при обработке файла {filePath}: {ex.Message}");
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


    }
}
