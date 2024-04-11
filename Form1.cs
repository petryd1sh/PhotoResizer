using PhotoResizerLibrary;
using System.Runtime.InteropServices;

namespace PhotoResizer;

public partial class Form1 : Form
{
    public ResizerService ResizerService { get; set; }
    public Form1()
    {
        InitializeComponent();
        ResizerService = new ResizerService();
        this.maxImageSizeInKb.Text = ResizerService.ResizerConfig.MaxImageSizeInKiloBytes.ToString();
    }

    private void resizeButton_Click(object sender, EventArgs e)
    {
        output.Items.Clear();
        var workingDirectory = this.folderBrowserDialog1.SelectedPath;
        if (workingDirectory == null || workingDirectory.Length == 0)
        {
            this.workingDirectory.Items.Clear();
            this.workingDirectory.Items.Add("Choose a Directory to process images.");
            return;
        }

        if(this.maxImageSizeInKb.Text == null || this.maxImageSizeInKb.Text == "")
        {
            if(this.maxImageSizeInKb.Text == "")
            {
                this.workingDirectory.Items.Clear();
                this.workingDirectory.Items.Add("Input a max image size.");
                return;
            }
        }

        ResizerService.ResizerConfig = new ResizerConfig()
            .WithWorkingDirectory(workingDirectory)
            .WithMaxImageSizeInKiloBytes(Convert.ToInt64(this.maxImageSizeInKb.Text));

        var images = ResizerService.GetImages(workingDirectory);

        foreach (var image in images)
        {
            output.Items.Add($"Processing {image}");
            var result = ResizerService.ProcessImage(image);
            output.Items.Add($"Completed {result.FullName} {result.Length / 1000}KB");
        }
    }

    private void chooseDirectoryButton_Click_1(object sender, EventArgs e)
    {
        this.folderBrowserDialog1.ShowDialog();
        var folderName = folderBrowserDialog1.SelectedPath;
        workingDirectory.Items.Clear();
        workingDirectory.Items.Add(folderName);
    }

}