namespace OmniArt.Views;
public partial class FullViewImagePage : ContentPage
{
	public FullViewImagePage(string imagePath)
	{
		InitializeComponent();

		FullImageView.Source = imagePath;
	}
}