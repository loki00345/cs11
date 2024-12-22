using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

public class DancingProgressBars : Form
{
    private FlowLayoutPanel flowPanel;
    private Button startButton;
    private int barCount;

    public DancingProgressBars()
    {
        this.Text = "Dancing Progress Bars";
        this.ClientSize = new Size(800, 600);

        flowPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };

        startButton = new Button
        {
            Text = "Start",
            Dock = DockStyle.Top,
            Height = 40
        };
        startButton.Click += StartButton_Click;

        this.Controls.Add(flowPanel);
        this.Controls.Add(startButton);
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        flowPanel.Controls.Clear();
        barCount = 10;

        for (int i = 0; i < barCount; i++)
        {
            ProgressBar progressBar = new ProgressBar
            {
                Width = flowPanel.ClientSize.Width - 30,
                Height = 25
            };
            flowPanel.Controls.Add(progressBar);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Random random = new Random();
                while (progressBar.Value < progressBar.Maximum)
                {
                    Thread.Sleep(random.Next(100, 500));
                    progressBar.Invoke((Action)(() =>
                    {
                        progressBar.Value = Math.Min(progressBar.Value + random.Next(1, 10), progressBar.Maximum);
                    }));
                }
            });
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new DancingProgressBars());
    }
}
