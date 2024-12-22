using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public class HorseRacingGame : Form
{
    private Button startButton;
    private FlowLayoutPanel trackPanel;
    private Label resultsLabel;
    private List<ProgressBar> progressBars;
    private List<int> raceResults;

    public HorseRacingGame()
    {
        this.Text = "Horse Racing Game";
        this.ClientSize = new Size(800, 600);

        startButton = new Button
        {
            Text = "Start Race",
            Dock = DockStyle.Top,
            Height = 40
        };
        startButton.Click += StartRace;

        trackPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };

        resultsLabel = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 100,
            TextAlign = ContentAlignment.TopCenter,
            Font = new Font("Arial", 12),
            Text = "Results will appear here after the race.",
            BorderStyle = BorderStyle.FixedSingle
        };

        this.Controls.Add(trackPanel);
        this.Controls.Add(startButton);
        this.Controls.Add(resultsLabel);

        progressBars = new List<ProgressBar>();
        raceResults = new List<int>();

        InitializeRaceTrack();
    }

    private void InitializeRaceTrack()
    {
        trackPanel.Controls.Clear();
        progressBars.Clear();
        raceResults.Clear();

        for (int i = 0; i < 5; i++) // 5 коней
        {
            ProgressBar progressBar = new ProgressBar
            {
                Width = trackPanel.ClientSize.Width - 30,
                Height = 25,
                Maximum = 100
            };
            trackPanel.Controls.Add(new Label { Text = $"Horse {i + 1}", AutoSize = true });
            trackPanel.Controls.Add(progressBar);
            progressBars.Add(progressBar);
        }
    }

    private async void StartRace(object sender, EventArgs e)
    {
        startButton.Enabled = false;
        resultsLabel.Text = "The race is on!";
        raceResults.Clear();

        var tasks = new List<Task>();

        for (int i = 0; i < progressBars.Count; i++)
        {
            int horseIndex = i;
            tasks.Add(Task.Run(() => RunHorse(horseIndex)));
        }

        await Task.WhenAll(tasks);

        DisplayResults();
        startButton.Enabled = true;
    }

    private void RunHorse(int horseIndex)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        ProgressBar progressBar = progressBars[horseIndex];

        while (progressBar.Value < progressBar.Maximum)
        {
            Thread.Sleep(random.Next(50, 200)); // Швидкість бігу
            progressBar.Invoke((Action)(() =>
            {
                progressBar.Value = Math.Min(progressBar.Value + random.Next(1, 5), progressBar.Maximum);
            }));
        }

        lock (raceResults)
        {
            raceResults.Add(horseIndex); // Зберігаємо порядок фінішу
        }
    }

    private void DisplayResults()
    {
        var resultsText = new List<string>();
        for (int i = 0; i < raceResults.Count; i++)
        {
            resultsText.Add($"Place {i + 1}: Horse {raceResults[i] + 1}");
        }

        resultsLabel.Invoke((Action)(() =>
        {
            resultsLabel.Text = string.Join("\n", resultsText);
        }));
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new HorseRacingGame());
    }
}
