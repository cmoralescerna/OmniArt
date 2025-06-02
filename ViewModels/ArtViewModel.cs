using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OmniArt.Models;

namespace OmniArt.ViewModels
{
    public class ArtViewModel : BindableObject
    {
        public Art art { get; }
        public ICommand EditArtCommand { get; }
        public ICommand DeleteArtCommand { get; }

        public ArtViewModel(Art art, INavigation navigation)
        {
            this.art = art;
        }

        public string Title
        {
            get { return art.Title; }
        }

        public string Description 
        { 
            get { return art.Description; }
        }

        public Country Country
        {
            get { return art.Participant.Country; }
        }

        public string FirstName
        {
            get { return art.Participant.FirstName; }
        }

        public string LastName
        {
            get { return art.Participant.LastName; }
        }

        public string FullName
        {
            get { return art.Participant.FullName; }
        }

    }
}
