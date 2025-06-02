using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniArt.Models
{
    public class Host
    {
        //private string hostId;
        private string firstName;
        private string lastName;
        private string fullName;

        // Constructor
        public Host()
        {
        }

        // Properties
        public string FirstName 
        {
            get { return firstName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    firstName = value;

                } else
                {
                    Console.WriteLine("Error, first name cannot be empty!");
                    return;
                }
            }
        }

        public string LastName 
        {
            get { return lastName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    lastName = value;

                } else
                {
                    Console.WriteLine("Error, last name cannot be empty!");
                }
            }
        }

        public string FullName
        {
           get
           {
                if (FirstName != null)
                {
                    FirstName = FirstName.Trim();
                }

                if (LastName != null)
                {
                    LastName = LastName.Trim();
                }

                return (FirstName + " " + LastName);
            }
        }

        public string HostId { get; set; } = Guid.NewGuid().ToString();

    }
}
