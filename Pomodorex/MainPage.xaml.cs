namespace Pomodorex
{
    public partial class MainPage : ContentPage
    {
        private DateTime timeStarted;
        private bool running;

        public MainPage()
        {
            InitializeComponent();
        }

        private void btnTimer_Clicked(object sender, EventArgs e)
        {
            if (btnTimer.Text.Contains("Uruchom"))
            {
                timeStarted = DateTime.Now;
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
            TimeSpan timePassed = time - timeStarted;
            Dispatcher.Dispatch(new Action(() =>
            {
                string formattedTime = string.Format("{0:D2}:{1:D2}", (int)timePassed.TotalMinutes, timePassed.Seconds);
                lblTimer.Text = formattedTime;
            }));
            Thread.Sleep(100);
            StartTimer();
        }
    }

}
