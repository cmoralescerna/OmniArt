namespace OmniArt.Views;
using OmniArt.ViewModels;
using OmniArt.Services;
using OmniArt.Models;
using Microsoft.EntityFrameworkCore;

public partial class GalleryDetailPage : ContentPage
{
	private Gallery selectedGallery;

	public GalleryDetailPage(Gallery selectedGallery)
	{
		InitializeComponent();
		this.selectedGallery = selectedGallery;
        BindingContext = new GalleryViewModel(selectedGallery, Navigation);
    }
}