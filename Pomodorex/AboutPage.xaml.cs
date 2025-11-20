namespace Pomodorex;

// AboutPage odpowiada za ekran „O aplikacji”, gdzie u¿ytkownik mo¿e dowiedzieæ siê wiêcej o technice Pomodoro
public partial class AboutPage : ContentPage
{
    // Konstruktor strony AboutPage
    // Odpowiada za inicjalizacjê komponentów XAML powi¹zanych z t¹ stron¹
    public AboutPage()
	{
		InitializeComponent(); // £aduje zawartoœæ z pliku AboutPage.xaml i powi¹zuje elementy z kodem
    }

    // Obs³uga zdarzenia klikniêcia przycisku "Learn More"
    // Po klikniêciu otwiera zewnêtrzn¹ stronê Wikipedii w domyœlnej przegl¹darce urz¹dzenia
    private async void LearnMore_Clicked(object sender, EventArgs e)
	{
        // Launcher.Default.OpenAsync otwiera podany URI w systemowej przegl¹darce
        // async/await zapewnia, ¿e metoda nie blokuje UI podczas otwierania strony
        await Launcher.Default.OpenAsync("https://pl.wikipedia.org/wiki/Technika_Pomodoro");
	}
}