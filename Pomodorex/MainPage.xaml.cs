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
            if (btnTimer.Text.Contains("Uruchom"))
            {
                timeEnds = DateTime.Now.AddMinutes(25);
                running = true;
                Thread thread = new Thread(StartTimer);
                thread.Start();
                btnTimer.Text = "Zatrzymaj";
            }
            else
            {
                running = false;
                btnTimer.Text = "Uruchom";
            }
        }

        private void StartTimer()
        {
            if (!running)
            {
                return;
            }
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
                string formattedTime = string.Format("{0:D2}:{1:D2}", (int)timeRemaining.TotalMinutes, timeRemaining.Seconds);
                lblTimer.Text = formattedTime;
            });

            Thread.Sleep(100);
            StartTimer();
        }
    }
}
