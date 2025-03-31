using System.Media;

namespace Pomodorex
{
    public partial class MainPage : ContentPage
    {
        private DateTime timeEnds;
        private bool running;
        private int pomodoroDuration;

        public MainPage()
        {
            InitializeComponent();

            pomodoroDuration = 25;
        }

        private void btnTimer_Clicked(object sender, EventArgs e)
        {
            if (!running)
            {
                timeEnds = DateTime.Now.AddMinutes(pomodoroDuration);
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
                    Dispatcher.Dispatch(() =>
                    {
                        lblTimer.Text = "00:00";
                        btnTimer.Text = "Uruchom";
                    });

                    PlayNotificationSound();

                    return;
                }

                Dispatcher.Dispatch(() =>
                {
                    lblTimer.Text = $"{(int)timeRemaining.TotalMinutes:D2}:{timeRemaining.Seconds:D2}";
                });

                await Task.Delay(1000);
            }
        }

        public void PlayNotificationSound()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "sound.wav");
            var player = new SoundPlayer(path);
            player.Play();
        }

        Slider slider = new Slider
        {
            Maximum = 60,
            Minimum = 1,
            Value = 25
        };
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            pomodoroDuration = (int)args.NewValue;
            lblTimer.Text = $"{pomodoroDuration:D2}:00";

            // Ustawiamy rotację w zależności od wartości suwaka
            //rotatingLabel.Rotation = value;

            // Aktualizujemy tekst w label
            //displayLabel.Text = $"Czas: {value:F0} min"; // Przykład: 15 min
        }
    }
}