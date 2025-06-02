using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace OmniArt.Models
{
    public class Gallery : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string title;
        private string description;
        private DateTime date;
        private DateTime startTime;
        private DateTime endTime;
        private GalleryStatus status;
        private List<Art> artPieces;
        private string hostId;
        private Host host; 

        // ------ Constructor ------
        public Gallery()
        {
            artPieces = new List<Art>();
        }

        // ------ Properties ------

        public string Title // User enters gallery title
        {
            get { return title; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));

                } else
                {
                    Console.WriteLine("Error, title cannot be empty!");
                    return;
                }
            }
        }

        public string Description // User enters gallery description
        {
            get { return description; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));

                } else
                {
                    Console.WriteLine("Error, description cannot be empty!");
                    return;
                }
            }
        }

        public DateTime Date // User enters date for gallery showing
        {
            get { return date; }

            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public DateTime StartTime // User enters time for when they want the gallery to start
        {
            get { return startTime; }

            set { startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        public DateTime EndTime // User enters time for when they want the gallery to end
        {
            get { return endTime; }

            set { endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }

        public string GalleryId { get; set; } = Guid.NewGuid().ToString();

        public GalleryStatus Status // There are three gallery statuses (upcoming, ongoing, or previous)
                                    // The gallery status updates based on the galleries' DateTime and current DateTime
        {
            get 
            {
                var now = DateTime.Now;
                var galleryStart = Date.Date + StartTime.TimeOfDay;
                var galleryEnd = Date.Date + EndTime.TimeOfDay;

                if (now < galleryStart) return GalleryStatus.Upcoming;
                if (now >= galleryStart && now <= galleryEnd) return GalleryStatus.Ongoing;
                return GalleryStatus.Previous;
            }
        }

        public List<Art> ArtPieces // Returns all the art piece objects in the current gallery
        {
            get { return artPieces; }
            set { artPieces = value; }
        }

        public Host Host { get; set; }


        // ------ Methods ------
        public bool CanAddArt()
        {
            return Status == GalleryStatus.Upcoming;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
