using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using OmniArt.Services;
using OmniArt.Models;

namespace OmniArt.ViewModels
{
    public class UploadArtViewModel : BindableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Declare the services
        private readonly ArtService artService;
        private readonly GalleryService galleryService;

        private string title;
        private string description;
        private string imagePath;
        //private Country selectedCountry;

        public List<Country> CountriesList { get; }
        public Gallery SelectedGallery { get; set; }
        public ArtViewModel Artist { get; set; }
        public ParticipantViewModel Participant { get; set; }
        public ICommand UploadArtCommand { get; }
        public ICommand PickImageCommand { get; }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return this.description; }
            set
            {
                this.description = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return this.imagePath; }

            set
            {
                imagePath = value;
                OnPropertyChanged();
            }
        }

        public UploadArtViewModel(ArtService artService, GalleryService galleryService, Gallery selectedGallery)
        {
            CountriesList = Enum.GetValues(typeof(Country)).Cast<Country>().ToList();

            this.artService = artService;
            this.galleryService = galleryService;

            Participant = new ParticipantViewModel();

            SelectedGallery = selectedGallery;

            UploadArtCommand = new Command(async () => await UploadArtAsync());
            PickImageCommand = new Command(async () => await PickImageAsync());
        }

        private async Task UploadArtAsync()
        {
            try
            {
                // Field validation
                if ((string.IsNullOrWhiteSpace(this.Title)) ||
                    (string.IsNullOrWhiteSpace(this.Description)) ||
                    (string.IsNullOrWhiteSpace(this.ImagePath)) ||
                    (string.IsNullOrWhiteSpace(Participant.FirstName)) ||
                    (string.IsNullOrWhiteSpace(Participant.LastName)))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Fields cannot be missing!", "OK");
                    return;
                }

                var newArt = new Art
                {
                    Title = this.Title,
                    Description = this.Description,
                    ImagePath = this.ImagePath,
                    Participant = new Participant
                    {
                        FirstName = Participant.FirstName,
                        LastName = Participant.LastName,
                        Country = Participant.Country
                    }
                };

                await artService.UploadArtAsync(SelectedGallery, newArt);

                await Application.Current.MainPage.DisplayAlert("Success", "Your art has been uploaded!", "OK");

                ResetForm();
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task PickImageAsync()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    var fileName = Path.GetFileName(result.FullPath);
                    var appDir = FileSystem.AppDataDirectory;
                    var destinationPath = Path.Combine(appDir, fileName);

                    using var sourceStream = await result.OpenReadAsync();
                    using var destinationStream = File.Create(destinationPath);
                    await sourceStream.CopyToAsync(destinationStream);

                    if (File.Exists(destinationPath))
                    {
                        ImagePath = destinationPath;

                    } else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to copy image", "OK");
                    } 
                }

            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Image selection failed: {ex.Message}", "OK");
            }
        }

        public void ResetForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            ImagePath = string.Empty;
            Participant.FirstName = string.Empty;
            Participant.LastName = string.Empty;
            Participant.Country = (int)Country.Afghanistan; // Afghanistan is the default value
        }
    }
}
