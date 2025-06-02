using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniArt.Models;

namespace OmniArt.ViewModels
{
    public class ParticipantViewModel : BindableObject
    {
        private string firstName;
        private string lastName;
        private Country country;

        public string FirstName
        {
            get { return this.firstName; }

            set
            {
                this.firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get { return this.lastName; }

            set
            {
                this.lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}".Trim(); }
        }

        public Country Country
        {
            get { return this.country; } 
             
            set
            {
                this.country = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Country));
            }
        }

        // Factory method to create from the Host Model
        public static ParticipantViewModel FromModel(Participant participant) => new ParticipantViewModel
        {
            FirstName = participant.FirstName,
            LastName = participant.LastName
        };

        // Map back to domain model
        public Participant ToModel() => new Participant
        {
            FirstName = this.FirstName,
            LastName = this.LastName
        };
    }
}

