namespace OmniArt;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

	/// <summary>
	/// When we click on "Get Started" on the Welcome Page
	/// The HomePage should open...
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnGetStartedClicked(object sender, EventArgs e)
	{

		Preferences.Default.Set("HasLaunched", true);

		Application.Current.Windows[0].Page = new AppShell();
	}
}