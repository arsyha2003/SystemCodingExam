using System.IO;
using System.Text.RegularExpressions;

namespace SystemCodingExam
{
    public partial class Form1 : Form
    {
        public DateTime timeOfStart;
        public int countOfFiles = 0;

        public bool isConsole = false;
        public bool isFileOpened = false;
        public bool isPaused = false;

        public string[] args = null;
        public string consoleFilePath = string.Empty;
        public string outputDirectory = string.Empty;
        public string logFilePath = "LogFile.txt";

        public List<string> secretWords = new List<string>();
        public List<string> logFileData = new List<string>();
        public Dictionary<string, int> countOfWords = new Dictionary<string, int>();

        public ProcessForm processForm;

        public CancellationTokenSource cts;
        public ManualResetEvent manualResetEvent = new ManualResetEvent(true);
        public Form1()
        {
            InitializeComponent();
            isConsole = false;
        }
        public Form1(string[] args)
        {
            isConsole = true;
            this.args = args;
            outputDirectory = args[0].Trim('"');
            Text = isConsole.ToString();
            textBox1.Text = outputDirectory;
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(logFilePath)) File.Create(logFilePath);
        }
        public async Task GetDataFromFileAsync(string filePath)
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
                            if(isConsole==false) textBox2.Text += line + " ";
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
        public async Task AwaitAllTasksAsync(Task[] tasks)
        {
            await Task.Run(() =>
            {
                Task.WaitAll(tasks);
                if (isConsole == false)
                {
                    MessageBox.Show($"Обработка завершена!\n" +
                    $"Время обработки: {(DateTime.Now - timeOfStart).Hours}:{(DateTime.Now - timeOfStart).Minutes}:{(DateTime.Now - timeOfStart).Seconds}\n" +
                    $"Обработано файлов: {countOfFiles}", string.Empty, MessageBoxButtons.OK);
                }
                string topOfWords = string.Empty;
                foreach(var item in countOfWords.OrderBy(x=>x.Value))
                {
                    topOfWords += $"1. Слово: {item.Key} количество повторений: {item.Value}\n";
                }
                Log(topOfWords);
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
        public async void StartProgramAsync()
        {
            timeOfStart = DateTime.Now; 
            countOfFiles = 0;
            cts = new CancellationTokenSource();
            button1.Enabled = false;
            if (isConsole == false)
            {
                secretWords = textBox2.Text.Split().ToList();
                outputDirectory = @textBox1.Text;
            }
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
                if (isConsole == false)
                {
                    processForm.Show();
                }

                List<Task> tasks = new List<Task>();
                foreach (var drive in drives)
                {
                    tasks.Add(Task.Run(() => RecursiveDirectorySearch(drive.Name)));
                }
                await AwaitAllTasksAsync(tasks.ToArray());
            }
            catch (Exception ex)
            {
                if (isConsole == false) MessageBox.Show("Обработка была прервана!", string.Empty, MessageBoxButtons.OK);
                Log($"Ошибка: {ex.Message} {DateTime.Now}");
            }
            finally
            {
                if (isConsole == false) button1.Enabled = true;
            }
        }
        private async void StartButtonEvent(object sender, EventArgs e) => StartProgramAsync();
        public void RecursiveDirectorySearch(string path)
        {
            try
            {
                foreach (var file in Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly))
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        if (isConsole == false) processForm.Close();
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
                        if (isConsole == false) processForm.Close();
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
        public void ProcessingTxtFile(string filePath)
        {
            try
            {
                countOfFiles++;
                string content = File.ReadAllText(filePath);
                bool isWordsFound = secretWords.Any(w => content.Contains(w));
                if (isWordsFound)
                {
                    string filteredContent = string.Empty;
                    foreach (string word in secretWords)
                    {
                        filteredContent = secretWords.Aggregate(content, (current, word) =>
                        current.Replace(word, "*******", StringComparison.OrdinalIgnoreCase));
                        try
                        {
                            countOfWords[word] += 1;
                        }
                        catch { }
                    }
                    string newFilePath = Path.Combine(outputDirectory, Path.GetFileName(filePath));
                    File.WriteAllText(newFilePath, filteredContent);
                    Log($"Обработан файл: {filePath}  размер: {new FileInfo(filePath).Length} {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Log($"Ошибка при обработке файла {filePath}: {ex.Message} {DateTime.Now}");
            }
        }
        public void Log(string message)
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
