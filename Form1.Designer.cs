namespace PhotoResizer;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        resize = new Button();
        workingDirectory = new ListBox();
        chooseDirectory = new Button();
        folderBrowserDialog1 = new FolderBrowserDialog();
        output = new ListBox();
        maxImageSizeInKb = new TextBox();
        label1 = new Label();
        SuspendLayout();
        // 
        // resize
        // 
        resize.BackColor = SystemColors.Control;
        resize.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        resize.ForeColor = SystemColors.ControlText;
        resize.Location = new Point(393, 23);
        resize.Name = "resize";
        resize.Size = new Size(85, 23);
        resize.TabIndex = 0;
        resize.Text = "Resize";
        resize.UseVisualStyleBackColor = false;
        resize.Click += resizeButton_Click;
        // 
        // workingDirectory
        // 
        workingDirectory.FormattingEnabled = true;
        workingDirectory.ItemHeight = 15;
        workingDirectory.Location = new Point(24, 62);
        workingDirectory.Name = "workingDirectory";
        workingDirectory.Size = new Size(952, 34);
        workingDirectory.TabIndex = 1;
        // 
        // chooseDirectory
        // 
        chooseDirectory.Location = new Point(24, 23);
        chooseDirectory.Name = "chooseDirectory";
        chooseDirectory.Size = new Size(138, 23);
        chooseDirectory.TabIndex = 2;
        chooseDirectory.Text = "Choose Directory";
        chooseDirectory.UseVisualStyleBackColor = true;
        chooseDirectory.Click += chooseDirectoryButton_Click_1;
        // 
        // output
        // 
        output.FormattingEnabled = true;
        output.ItemHeight = 15;
        output.Location = new Point(24, 120);
        output.Name = "output";
        output.Size = new Size(952, 589);
        output.TabIndex = 3;
        // 
        // maxImageSizeInKb
        // 
        maxImageSizeInKb.Location = new Point(305, 23);
        maxImageSizeInKb.Name = "maxImageSizeInKb";
        maxImageSizeInKb.Size = new Size(68, 23);
        maxImageSizeInKb.TabIndex = 4;
        maxImageSizeInKb.TextAlign = HorizontalAlignment.Right;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.BackColor = SystemColors.Control;
        label1.Location = new Point(180, 27);
        label1.Name = "label1";
        label1.Size = new Size(122, 15);
        label1.TabIndex = 5;
        label1.Text = "Max Image Size in KB:";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1008, 729);
        Controls.Add(label1);
        Controls.Add(maxImageSizeInKb);
        Controls.Add(output);
        Controls.Add(chooseDirectory);
        Controls.Add(workingDirectory);
        Controls.Add(resize);
        Name = "Form1";
        Text = "PhotoResizer";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button resize;
    private ListBox workingDirectory;
    private Button chooseDirectory;
    private FolderBrowserDialog folderBrowserDialog1;
    private ListBox output;
    private TextBox maxImageSizeInKb;
    private Label label1;
}