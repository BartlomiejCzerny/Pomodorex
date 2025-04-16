using System.Media;

namespace Pomodorex
{
    public partial class MainPage : ContentPage
    {
        private byte _pomodoroDuration = 25;
        private bool _isStarted;
        private bool _isPaused;
        private DateTime _timeEnds;
        private TimeSpan _remainingTime;

        public bool isStarted
        {
            get => _isStarted;
            set
            {
                if (_isStarted != value)
                {
                    _isStarted = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void btnStartPauseTimer_Clicked(object sender, EventArgs e)
        {
            if (!isStarted && !_isPaused)
            {
                _remainingTime = TimeSpan.FromMinutes(_pomodoroDuration);
                _timeEnds = DateTime.Now.Add(_remainingTime);
                isStarted = true;
                sliderDuration.IsEnabled = false;
                btnStartPauseTimer.Text = "Wstrzymaj";
                Task.Run(StartTimer);
            }
            else if (isStarted)
            {
                _remainingTime = _timeEnds - DateTime.Now;
                isStarted = false;
                _isPaused = true;
                sliderDuration.IsEnabled = false;
                btnStartPauseTimer.Text = "Wznów";
            }
            else if (_isPaused)
            {
                _timeEnds = DateTime.Now.Add(_remainingTime);
                isStarted = true;
                _isPaused = false;
                sliderDuration.IsEnabled = false;
                btnStartPauseTimer.Text = "Wstrzymaj";
                Task.Run(StartTimer);
            }
        }

        private void btnStopTimer_Clicked(object sender, EventArgs e)
        {
            isStarted = false;
            _isPaused = false;
            sliderDuration.IsEnabled = true;
            btnStartPauseTimer.Text = "Uruchom";
            lblTimer.Text = $"{_pomodoroDuration:D2}:00";
        }

        private async Task StartTimer()
        {
            while (isStarted)
            {
                _remainingTime = _timeEnds - DateTime.Now;

                if (_remainingTime.TotalSeconds <= 0)
                {
                    isStarted = false;
                    _isPaused = false;

                    Dispatcher.Dispatch(() =>
                    {
                        lblTimer.Text = "00:00";
                        btnStartPauseTimer.Text = "Uruchom";
                        sliderDuration.IsEnabled = true;
                    });

                    PlayNotificationSound();
                    return;
                }

                Dispatcher.Dispatch(() =>
                {
                    lblTimer.Text = $"{(int)_remainingTime.TotalMinutes:D2}:{_remainingTime.Seconds:D2}";
                });

                await Task.Delay(1000);
            }
        }

        private void PlayNotificationSound()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "sound.wav");
            var player = new SoundPlayer(path);
            player.Play();
        }

        private void OnPomodoroDurationChanged(object sender, ValueChangedEventArgs args)
        {
            isStarted = false;
            _pomodoroDuration = (byte)args.NewValue;
            lblTimer.Text = $"{_pomodoroDuration:D2}:00";
        }
    }
}