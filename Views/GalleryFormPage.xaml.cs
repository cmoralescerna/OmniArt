namespace OmniArt.Views;
using OmniArt.ViewModels;
using OmniArt.Services;
using OmniArt.Models;
using Microsoft.EntityFrameworkCore;

public partial class GalleryFormPage : ContentPage
{
	private readonly GalleryFormViewModel galleryFormViewModel;

	public GalleryFormPage()
	{
		InitializeComponent();

		// Get the Gallery Service instance
		var galleryService = App.Services.GetService<GalleryService>();

		// Pass it to the view model
		BindingContext = new GalleryFormViewModel(galleryService);
	}

	// Overloaded constructor for when we need to reuse
	// the form to edit a gallery.
	public GalleryFormPage(Gallery galleryToEdit)
	{
		InitializeComponent();
		var galleryService = App.Services.GetService<GalleryService>();
		galleryFormViewModel = new GalleryFormViewModel(galleryService);
		galleryFormViewModel.LoadGalleryForEdit(galleryToEdit); // Prefill the form with the current gallery's info
		BindingContext = galleryFormViewModel;
	}
}