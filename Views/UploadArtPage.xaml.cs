namespace OmniArt.Views;
using OmniArt.ViewModels;
using OmniArt.Services;
using OmniArt.Models;
using Microsoft.EntityFrameworkCore;

public partial class UploadArtPage : ContentPage
{
	public UploadArtPage(Gallery gallery)
	{
		InitializeComponent();

		// Get the gallery and art service instance
		var galleryService = App.Services.GetService<GalleryService>();
		var artService = App.Services.GetService<ArtService>();

		// Pass it to the ViewModel
		BindingContext = new UploadArtViewModel(artService, galleryService, gallery);
	}
}