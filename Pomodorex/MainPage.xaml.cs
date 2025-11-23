using Plugin.Maui.Audio;

namespace Pomodorex
{
    public partial class MainPage : ContentPage
    {
        // Czas trwania pojedynczej sesji Pomodoro (w minutach).
        // Domyślnie 25, ale użytkownik może zmienić sliderem.
        private byte _pomodoroDuration = 25;

        // Flaga informująca, czy timer aktualnie odlicza.
        private bool _isStarted;

        // Flaga informująca, czy timer został zatrzymany w połowie (pauza).
        private bool _isPaused;

        // Moment w przyszłości, kiedy odliczanie powinno się zakończyć.
        private DateTime _timeEnds;

        // Pozostały czas odliczania — aktualizowany co sekundę i podczas pauzy.
        private TimeSpan _remainingTime;

        // Właściwość powiązana z UI (binding);
        // używana do odświeżania elementów interfejsu, np. przycisków.
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

            // Kontekst danych dla powiązań XAML -> C#.
            // Dzięki temu XAML może reagować na zmiany właściwości.
            BindingContext = this;
        }

        // Obsługa logiki Start / Pause / Resume jednego przycisku.
        private void btnStartPauseTimer_Clicked(object sender, EventArgs e)
        {
            // ▶ START (gdy timer nie działa i nie jest wstrzymany)
            if (!isStarted && !_isPaused)
            {
                // Ustawienie początkowego czasu na podstawie slidera.
                _remainingTime = TimeSpan.FromMinutes(_pomodoroDuration);

                // Obliczenie czasu zakończenia sesji (punkt w przyszłości).
                _timeEnds = DateTime.Now.Add(_remainingTime);

                // Uruchamiamy timer.
                isStarted = true;

                // Blokujemy zmianę czasu trwania, aby uniknąć błędów.
                sliderDuration.IsEnabled = false;

                // Zmieniamy etykietę na przycisku.
                btnStartPauseTimer.Text = "Wstrzymaj";

                // Uruchamiamy odliczanie na wątku w tle.
                Task.Run(StartTimer);
            }
            // ⏸ PAUSE (gdy timer działa)
            else if (isStarted)
            {
                // Wyliczamy ile czasu zostało do końca.
                _remainingTime = _timeEnds - DateTime.Now;

                // Zatrzymujemy odliczanie.
                isStarted = false;
                _isPaused = true;

                // Nadal blokujemy slider — użytkownik nie powinien edytować czasu.
                sliderDuration.IsEnabled = false;

                btnStartPauseTimer.Text = "Wznów";
            }
            // ▶ RESUME (gdy timer jest wstrzymany)
            else if (_isPaused)
            {
                // Nowy czas zakończenia — "przesuwamy" deadline o pozostały czas.
                _timeEnds = DateTime.Now.Add(_remainingTime);

                isStarted = true;
                _isPaused = false;

                sliderDuration.IsEnabled = false;
                btnStartPauseTimer.Text = "Wstrzymaj";

                Task.Run(StartTimer);
            }
        }

        // Przycisk STOP — resetuje całą logikę timera.
        private void btnStopTimer_Clicked(object sender, EventArgs e)
        {
            isStarted = false;
            _isPaused = false;

            // Odblokowanie slidera — użytkownik może ustawić nowy czas.
            sliderDuration.IsEnabled = true;


            btnStartPauseTimer.Text = "Uruchom";

            // Wyświetlenie domyślnego formatu czasu.
            lblTimer.Text = $"{_pomodoroDuration:D2}:00";
        }

        // Główna pętla odliczania — działa w tle i aktualizuje UI przez Dispatcher.
        private async Task StartTimer()
        {
            while (isStarted)
            {
                // Obliczenie pozostałego czasu w danym momencie.
                _remainingTime = _timeEnds - DateTime.Now;

                // Jeśli czas się skończył — wyjście z pętli.
                if (_remainingTime.TotalSeconds <= 0)
                {
                    isStarted = false;
                    _isPaused = false;

                    // Aktualizacja UI — wątki tła nie mogą bezpośrednio manipulować UI.
                    Dispatcher.Dispatch(() =>
                    {
                        lblTimer.Text = "Sesja zakończona";
                        btnStartPauseTimer.Text = "Uruchom";
                        sliderDuration.IsEnabled = true;
                    });

                    // Powiadomienie dźwiękowe po zakończeniu.
                    PlayNotificationSound();
                    return;
                }

                // Odświeżenie zegara na ekranie.
                Dispatcher.Dispatch(() =>
                {
                    lblTimer.Text = $"{(int)_remainingTime.TotalMinutes:D2}:{_remainingTime.Seconds:D2}";
                });

                // Odświeżanie co sekundę — typowa logika timera.
                await Task.Delay(1000);
            }
        }

        // Odtwarzanie dźwięku po zakończeniu sesji.
        private async void PlayNotificationSound()
        {
            try
            {
                var audioManager = AudioManager.Current;
                var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("sound.wav"));
                player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd odtwarzania dźwięku: {ex.Message}");
            }
        }

        // Reakcja na zmianę wartości slidera (czas trwania Pomodoro).
        // Zmiana czasu automatycznie resetuje timer.
        private void OnPomodoroDurationChanged(object sender, ValueChangedEventArgs args)
        {
            isStarted = false;

            // Zapamiętujemy nową długość sesji.
            _pomodoroDuration = (byte)args.NewValue;

            // Aktualizacja wyświetlanego czasu.
            lblTimer.Text = $"{_pomodoroDuration:D2}:00";
        }
    }
}