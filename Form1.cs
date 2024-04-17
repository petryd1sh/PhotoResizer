using System.ComponentModel;
using PhotoResizerLibrary;
using System.Runtime.InteropServices;

namespace PhotoResizer;

public partial class Form1 : Form
{
    public ResizerService ResizerService { get; set; }
    private List<string> ImageList { get; set; }
    public Form1()
    {
        InitializeComponent();
        ResizerService = new ResizerService();
        maxImageSizeInKb.Text = ResizerService.ResizerConfig.MaxImageSizeInKiloBytes.ToString();
        backgroundWorker1.DoWork += backgroundWorker1_DoWork;
        backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
        backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        backgroundWorker1.WorkerReportsProgress = true;
        backgroundWorker1.WorkerSupportsCancellation = true;
    }

    private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        var count = 0.0;
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 20
        };
        Task.Factory.StartNew(() =>
        {
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker1.ReportProgress(0);
            }
        });

        
        Parallel.ForEach(ImageList, parallelOptions, image =>
        {
            var result = ResizerService.ProcessImage(image);
            var percentProgress = count / ImageList.Count * 100;
            Console.WriteLine($"Progress {percentProgress}");
            count++;
            backgroundWorker1.ReportProgress(Convert.ToInt32(percentProgress));
            output.Items.Add($"Completed {result.FullName} {result.Length / 1000}KB");
        });
        output.Items.Add($"Finished {count} files.");
        backgroundWorker1.ReportProgress(100);
    }

    private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        progressBar1.Value = e.ProgressPercentage;
    }
    
    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            Console.WriteLine("Operation was canceled.");
        }
        else if (e.Error != null)
        {
            Console.WriteLine("Error occurred: " + e.Error.Message, "Error");
        }
        else
        {
            Console.WriteLine("Completed.");
        }
    }
    
    private void resizeButton_Click(object sender, EventArgs e)
    {
        resizeButton.Enabled = false;
        cancelButton.Enabled = true;
        output.Items.Clear();
        var workingDirectory = this.folderBrowserDialog1.SelectedPath;
        if (workingDirectory == null || workingDirectory.Length == 0)
        {
            this.workingDirectory.Items.Clear();
            this.workingDirectory.Items.Add("Choose a Directory to process images.");
            return;
        }

        if (this.maxImageSizeInKb.Text == null || this.maxImageSizeInKb.Text == "")
        {
            if (this.maxImageSizeInKb.Text == "")
            {
                this.workingDirectory.Items.Clear();
                this.workingDirectory.Items.Add("Input a max image size.");
                return;
            }
        }


        ResizerService.ResizerConfig = new ResizerConfig()
            .WithWorkingDirectory(workingDirectory)
            .WithMaxImageSizeInKiloBytes(Convert.ToInt64(this.maxImageSizeInKb.Text));

        ImageList = ResizerService.GetImages(workingDirectory).ToList();
        output.Items.Add($"Found {ImageList.Count} images in {workingDirectory}");
        Console.WriteLine(backgroundWorker1.IsBusy);
        backgroundWorker1.RunWorkerAsync();
    }

    private void chooseDirectoryButton_Click_1(object sender, EventArgs e)
    {
        this.folderBrowserDialog1.ShowDialog();
        var folderName = folderBrowserDialog1.SelectedPath;
        workingDirectory.Items.Clear();
        workingDirectory.Items.Add(folderName);
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        backgroundWorker1.CancelAsync();
        resizeButton.Enabled = true;
        cancelButton.Enabled = false;
    }
}