using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniArt.Models
{
    public class Participant
    {
        //private string participantId;
        private string firstName;
        private string lastName;
        private List<Art> portfolio;
        private Country country;

        // Constructor
        public Participant()
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
                    return;
                }
            } 
        }

        public string FullName
        {
            get
            {
                if (FirstName != null)
                {
                    firstName = FirstName.Trim();
                }

                if (LastName != null)
                {
                    lastName = LastName.Trim();
                }

                string fullName = (firstName + " " + lastName).Trim();
                return fullName;
            }
        }

        public string ParticipantId { get; set; } = Guid.NewGuid().ToString();

        public List<Art> Portfolio
        {
            get { return portfolio; }

            set { portfolio = value; }
        }

        public Country Country
        {
            get { return country; }

            set { country = value; }
        }
    }
}
