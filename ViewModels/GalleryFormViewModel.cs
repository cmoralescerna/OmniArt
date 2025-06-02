using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using OmniArt.Services;
using OmniArt.Models;

namespace OmniArt.ViewModels
{
    // The BindableObject class provides a data storage mechanism that enables synchronization
    // of data between objects in response to changes (e.g. the View and ViewModel)
    public class GalleryFormViewModel: BindableObject
    {
        // PropertyChangedEventHandler is essential for notifying the UI 
        // to update when a property on a data-bound object changes
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly GalleryService galleryService;

        private string title;
        private string description;
        private DateTime date = DateTime.Today;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private Gallery editingGallery;

        public ICommand CreateGalleryCommand { get; }

        public HostViewModel Host { get; set; }

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

        public DateTime Date
        {
            get { return this.date; }
            set 
            { 
                this.date = value; 
                OnPropertyChanged(); 
            }
        }

        public TimeSpan StartTime
        {
            get { return this.startTime; }
            set 
            {
                this.startTime = value; 
                OnPropertyChanged(); 
            }
        }

        public TimeSpan EndTime
        {
            get { return this.endTime; }
            set 
            { 
                this.endTime = value; 
                OnPropertyChanged(); 
            }
        }

        public bool IsEditable
        {
            get { return editingGallery == null || editingGallery.Status == GalleryStatus.Upcoming; }
        }

        public bool IsTimeEditable
        {
            get { return editingGallery != null && editingGallery.Status == GalleryStatus.Upcoming; }
        }

        public GalleryFormViewModel(GalleryService galleryService)
        {
            this.galleryService = galleryService;

            // Default start and end time to the current time
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = DateTime.Now.TimeOfDay;

            Host = new HostViewModel();
            CreateGalleryCommand = new Command(async () => await SaveGalleryAsync());
        }

        private async Task SaveGalleryAsync()
        {
            try
            {
                if (await ValidateFields() || await ValidateDate() || await ValidateTime())
                {
                    return;
                }

                if (editingGallery == null)
                {
                    var newGallery = new Gallery
                    {
                        Title = this.Title,
                        Description = this.Description,
                        Date = this.Date,
                        StartTime = this.Date.Date + this.StartTime,
                        EndTime = this.Date.Date + this.EndTime,
                        Host = new Host
                        {
                            FirstName = Host.FirstName,
                            LastName = Host.LastName
                        }
                    };

                    await galleryService.CreateGalleryAsync(newGallery);
                    await ShowSuccess("Gallery created");

                } else
                {
                    editingGallery.Title = Title;
                    editingGallery.Description = Description;
                    editingGallery.Date = Date;
                    editingGallery.StartTime = Date.Date + StartTime;
                    editingGallery.EndTime = Date.Date + EndTime;
                    editingGallery.Host.FirstName = Host.FirstName;
                    editingGallery.Host.LastName = Host.LastName;

                    await galleryService.EditGalleryAsync(editingGallery);
                    await ShowSuccess("Gallery updated"); 

                    editingGallery = null; // reset
                }

                // Clear the form
                ResetForm();

            } catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public void ResetForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = DateTime.Now.TimeOfDay;
            Host.FirstName = string.Empty;
            Host.LastName = string.Empty;
        }

        public async Task<bool> ValidateFields()
        {
            bool error = false;

            if ((string.IsNullOrWhiteSpace(this.Title)) ||
                    (string.IsNullOrWhiteSpace(this.Description)) ||
                    (string.IsNullOrWhiteSpace(Host.FirstName)) ||
                    (string.IsNullOrWhiteSpace(Host.LastName)))
            {
                await ShowError("Fields cannot be missing");
                error = true;
            }

            return error;
        }

        public async Task<bool> ValidateTime()
        {
            DateTime fullStart = Date.Date + StartTime;
            DateTime fullEnd = Date.Date + EndTime;

            // Basic time logic validation
            if (fullEnd <= fullStart)
            {
                await ShowError("The gallery cannot end before it starts!");
                return true;

            }

            // Validation for when creating a new gallery
            if (editingGallery == null)
            {
                if (fullStart < DateTime.Now)
                {
                    await ShowError("The gallery cannot start in the past!");
                    return true;
                }
                return false;
            }

            // Editing an existing gallery
            bool timeChanged = editingGallery.StartTime != fullStart || editingGallery.EndTime != fullEnd;

            if (editingGallery.Status == GalleryStatus.Upcoming)
            {
                if (fullStart < DateTime.Now)
                {
                    await ShowError("The gallery cannot start in the past!");
                    return true;
                }

                return false;
            }

            // For ongoing and previous galleries
            if (timeChanged)
            {
                await ShowError("Time editing is not permitted for galleries that are ongoing or complete!");
                return true;
            }

            return false;
        }

        public async Task<bool> ValidateDate()
        {

            if (editingGallery != null && editingGallery.Date.Date != Date.Date && editingGallery.Status != GalleryStatus.Upcoming)
            {
                await ShowError("Date editing is not permitted for galleries that are ongoing or complete!");
                return true;
            }

            return false;
        }

        public void LoadGalleryForEdit(Gallery gallery)
        {
            if (gallery == null)
            {
                return; 
            }

            editingGallery = gallery;

            Title = gallery.Title;
            Description = gallery.Description;
            Date = gallery.Date;
            StartTime = gallery.StartTime.TimeOfDay;
            EndTime = gallery.EndTime.TimeOfDay;

            if (gallery.Host != null)
            {
                Host.FirstName = gallery.Host.FirstName;
                Host.LastName = gallery.Host.LastName;
            }
        }

        private async Task ShowError(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Error!", message, "OK");
        }

        private async Task ShowSuccess(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Success!", message, "OK");
        }
    }
}
