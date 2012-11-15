using System;
using System.Windows.Forms;

namespace Pomodorino
{
    public partial class Form1 : Form
    {
        AppState appState;
        DateTime endTime;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            appState = AppState.Initial;
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            UpdateTexts();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            endTime = DateTime.Now.AddMinutes(20);
            timer.Start();
            appState = AppState.Running;
            UpdateTexts();
        }

        void UpdateTexts()
        {
            switch (appState)
            {
                case AppState.Initial:
                case AppState.Stopped:
                    timeDisplay.Text = "20:00";
                    stopButton.Text = "Exit";
                    break;
                case AppState.Running:
                    stopButton.Text = "Stop";
                    break;
                case AppState.Alarming:
                    timeDisplay.Text = "Done!";
                    stopButton.Text = "Stop";
                    break;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            switch (appState)
            {
                case AppState.Running:
                    var remainingTime = (DateTime.Now - endTime);
                    timeDisplay.Text = remainingTime.ToString("mm\\:ss");    
                    if (remainingTime.TotalSeconds >= 0)
                    {
                        appState = AppState.Alarming;
                        UpdateTexts();
                    }
                    break;
                case AppState.Alarming:
                    System.Media.SystemSounds.Beep.Play();
                    break;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            switch (appState)
            {
                case AppState.Initial:
                case AppState.Stopped:
                    Application.Exit();
                    break;
                case AppState.Running:
                case AppState.Alarming:
                    timer.Stop();
                    appState = AppState.Stopped;
                    break;
            }
            UpdateTexts();
        }
    }

    internal enum AppState
    {
        Initial,
        Running,
        Alarming,
        Stopped
    }
}
