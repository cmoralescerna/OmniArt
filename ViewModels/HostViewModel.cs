using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniArt.Models;

namespace OmniArt.ViewModels
{
    public class HostViewModel : BindableObject
    {
        private string firstName;
        private string lastName;

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

        // Factory method to create from the Host Model
        public static HostViewModel FromModel(Host host) => new HostViewModel
        {
            FirstName = host.FirstName,
            LastName = host.LastName
        };

        // Map back to domain model
        public Host ToModel() => new Host
        {
            FirstName = this.FirstName,
            LastName = this.LastName
        };


    }
}
