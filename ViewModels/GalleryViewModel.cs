using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using OmniArt.Models;
using OmniArt.Views;
namespace OmniArt.ViewModels
{
    /// <summary>
    /// This ViewModel is used to retrieve all the information for a particular gallery
    /// </summary>
    public class GalleryViewModel : BindableObject
    {
        public Gallery gallery { get; }
        public ObservableCollection<Art> ArtPieces { get; }
        public ICommand UploadArtCommand { get; }
        public ICommand ImageTappedCommand { get; }
    
        public GalleryViewModel(Gallery gallery, INavigation navigation)
        {
            this.gallery = gallery;
            ArtPieces = new ObservableCollection<Art>(gallery.ArtPieces ?? new List<Art>());

            UploadArtCommand = new Command(async () =>
            {
                await navigation.PushAsync(new UploadArtPage(gallery));
            });

            ImageTappedCommand = new Command<Art>(async (art) =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new FullViewImagePage(art.ImagePath));
            });

        }

        // The GalleryDetailPage is binded with this ViewModel. So the values of these
        // properties are what will appear on the GalleryDetailPage.
        public string Title
        {
            get { return gallery.Title; }
        }

        public string Description
        {
            get { return gallery.Description; }
        }

        public DateTime Date
        {
            get { return gallery.Date; }
        }

        public TimeSpan StartTime
        {
            get { return gallery.StartTime.TimeOfDay; }
        }

        public TimeSpan EndTime
        {
            get { return gallery.EndTime.TimeOfDay; }
        }

        public string HostFullName
        {
            get { return gallery.Host.FullName; }
        }

        public bool IsUpcoming
        {
            get { return gallery.Status == GalleryStatus.Upcoming; }
        }

        public bool IsOngoing
        {
            get { return gallery.Status == GalleryStatus.Ongoing; }
        }

        public bool IsPrevious
        {
            get { return gallery.Status == GalleryStatus.Previous; }
        }

        public bool ShowArt
        {
            get { return IsOngoing || IsPrevious; }
        }
    }
}

