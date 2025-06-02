using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using OmniArt.Models;
using OmniArt.Services;

namespace OmniArt.ViewModels
{
    /// <summary>
    /// This ViewModel manages the upcoming, ongoing, and previous viewmodels
    /// It adds them to the proper section respective to their status.
    /// </summary>
    public class GalleriesViewModel : BindableObject
    {
        private readonly GalleryService galleryService;
        private bool isUpcomingEmpty;
        private bool isOngoingEmpty;
        private bool isPreviousEmpty;

        public ObservableCollection<Gallery> Upcoming { get; } = new();
        public ObservableCollection<Gallery> Ongoing { get; } = new();
        public ObservableCollection<Gallery> Previous { get; } = new();

        public GalleriesViewModel(GalleryService galleryService)
        {
            this.galleryService = galleryService;
        }

        public bool IsUpcomingEmpty
        {
            get { return isUpcomingEmpty; }
            set
            {
                if (isUpcomingEmpty != value)
                {
                    isUpcomingEmpty = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UpcomingHasGalleries));
                }
            }
        }

        public bool IsOngoingEmpty
        {
            get { return isOngoingEmpty; }
            set
            {
                if(isOngoingEmpty != value)
                {
                    isOngoingEmpty = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(OngoingHasGalleries));
                }
            }
        }

        public bool IsPreviousEmpty
        {
            get { return isPreviousEmpty; }
            set
            {
                if (isPreviousEmpty != value)
                {
                    isPreviousEmpty = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PreviousHasGalleries));
                }
            }
        }

        public bool UpcomingHasGalleries
        {
            get { return !IsUpcomingEmpty; }
        }

        public bool OngoingHasGalleries
        {
            get { return !IsOngoingEmpty; }
        }

        public bool PreviousHasGalleries
        {
            get { return !IsPreviousEmpty; }
        }

        public async Task LoadAsync()
        {
            Upcoming.Clear();
            Ongoing.Clear();
            Previous.Clear();

            var galleries = await galleryService.GetAllGalleriesAsync();

            foreach(var gallery in galleries)
            {
                switch(gallery.Status)
                {
                    case GalleryStatus.Upcoming: Upcoming.Add(gallery);
                        break;
                    case GalleryStatus.Ongoing: Ongoing.Add(gallery);
                        break;
                    case GalleryStatus.Previous: Previous.Add(gallery);
                        break;
                }
            }

            IsUpcomingEmpty = Upcoming.Count == 0;
            IsOngoingEmpty = Ongoing.Count == 0;
            IsPreviousEmpty = Previous.Count == 0;
        }

        public async Task DeleteGalleryAsync(Gallery gallery)
        {
            if (gallery == null)
            {
                return;
            }

            bool success = await galleryService.DeleteGalleryAsync(gallery.GalleryId);
        
            if (success)
            {
                switch (gallery.Status)
                {
                    case GalleryStatus.Upcoming:
                        Upcoming.Remove(gallery);
                        IsUpcomingEmpty = Upcoming.Count == 0;
                        break;
                    case GalleryStatus.Ongoing:
                        Ongoing.Remove(gallery);
                        IsOngoingEmpty = Ongoing.Count == 0;
                        break;
                    case GalleryStatus.Previous:
                        Previous.Remove(gallery);
                        IsPreviousEmpty = Previous.Count == 0;
                        break;
                }
            }
        }
        
        public async Task<bool> EditGalleryAsync(Gallery updatedGallery)
        {
            if (updatedGallery == null)
            {
                return false;
            }

            bool success = await galleryService.EditGalleryAsync(updatedGallery);
            
            if (!success)
            {
                return false;
            }

            // Retrieve the existing gallery object from its corresponding list
            var currentGallery = Upcoming.FirstOrDefault(g => g.GalleryId == updatedGallery.GalleryId)
                               ?? Ongoing.FirstOrDefault(g => g.GalleryId == updatedGallery.GalleryId)
                               ?? Previous.FirstOrDefault(g => g.GalleryId == updatedGallery.GalleryId);

            if (currentGallery != null)
            {
                // Reset any changed values
                currentGallery.Title = updatedGallery.Title;
                currentGallery.Description = updatedGallery.Description;
                currentGallery.Date = updatedGallery.Date;
                currentGallery.StartTime = updatedGallery.StartTime;
                currentGallery.EndTime = updatedGallery.EndTime;
                currentGallery.Host.FirstName = updatedGallery.Host.FirstName;
                currentGallery.Host.LastName = updatedGallery.Host.LastName;
            }

            // We must notify the UI to refresh so it can display the updated gallery!
            OnPropertyChanged(nameof(Upcoming));
            OnPropertyChanged(nameof(Ongoing));
            OnPropertyChanged(nameof(Previous));

            return true;
        }
    }
}
