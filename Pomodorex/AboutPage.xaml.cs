namespace Pomodorex;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

	private async void LearnMore_Clicked(object sender, EventArgs e)
	{
		await Launcher.Default.OpenAsync("https://pl.wikipedia.org/wiki/Technika_Pomodoro");
	}
}