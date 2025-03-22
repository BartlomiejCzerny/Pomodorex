namespace Pomodorex
{
    public partial class MainPage : ContentPage
    {
        private DateTime timeEnds;
        private bool running;

        public MainPage()
        {
            InitializeComponent();
        }

        private void btnTimer_Clicked(object sender, EventArgs e)
        {
            if (!running)
            {
                timeEnds = DateTime.Now.AddMinutes(25);
                running = true;
                btnTimer.Text = "Zatrzymaj";

                Task.Run(StartTimer);
            }
            else
            {
                running = false;
                btnTimer.Text = "Uruchom";
            }
        }

        private async Task StartTimer()
        {
            while (running)
            {
                DateTime time = DateTime.Now;
                TimeSpan timeRemaining = timeEnds - time;

                if (timeRemaining.TotalSeconds <= 0)
                {
                    running = false;
                    Dispatcher.Dispatch(() => lblTimer.Text = "00:00");
                    return;
                }

                Dispatcher.Dispatch(() =>
                {
                    lblTimer.Text = $"{(int)timeRemaining.TotalMinutes:D2}:{timeRemaining.Seconds:D2}";
                });

                await Task.Delay(1000);
            }
        }
    }
}
