using OmniArt.Services;
using OmniArt.ViewModels;
using OmniArt.Models;

namespace OmniArt.Views;

public partial class UpcomingGalleriesPage : ContentPage
{
	private readonly GalleriesViewModel galleriesViewModel;
	private readonly GalleryService galleryService;

	public UpcomingGalleriesPage()
	{
		InitializeComponent();

		galleryService = App.Services.GetService<GalleryService>();
		galleriesViewModel = new GalleriesViewModel(galleryService);
		BindingContext = galleriesViewModel;
	}

	private async void OnGallerySelected(object sender, SelectionChangedEventArgs e)
	{
        var collectionView = sender as CollectionView;
        if (collectionView != null)
        {
            collectionView.SelectedItem = null;
        }

        var selectedGallery = e.CurrentSelection.FirstOrDefault() as Gallery;
		if (selectedGallery == null)
		{
			return;
		}

        // Pass the object (if using a shared data store or stateful model)
        // Shell.Current.GoToAsync(nameof(GalleryDetailPage), true,
        //   new Dictionary<string, object> { ["Gallery"] = selectedGallery });
        await Navigation.PushAsync(new GalleryDetailPage(selectedGallery));
	}

	private async void OnOptionsClicked(object sender, EventArgs e)
	{
		if (sender is ImageButton button && button.BindingContext is Gallery gallery)
		{
			string action = await Application.Current.MainPage.DisplayActionSheet(
				"Options",
				"Cancel",
				null,
				"Edit",
				"Delete");

			if (action == "Edit")
			{
				await Navigation.PushAsync(new GalleryFormPage(gallery));

			} else if (action == "Delete")
			{
				bool confirm = await Application.Current.MainPage.DisplayAlert("" +
					"Confirm Delete",
					$"Delete gallery '{gallery.Title}?",
					"Yes", "No");

				if (confirm)
				{
					await galleriesViewModel.DeleteGalleryAsync(gallery);
				} else
				{
					return;
				}
			}
		}
	}

	
	// Loads the current galleries
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await galleriesViewModel.LoadAsync();
	}
};