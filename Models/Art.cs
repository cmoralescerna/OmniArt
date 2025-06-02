using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniArt.Models
{
    public class Art
    {
        private string artId;
        private string title;
        private string description;
        private string imagePath;
        private Participant participant;

        // Constructor
        public Art()
        {
        }

        // Properties
        public string ArtId { get; set; } = Guid.NewGuid().ToString();

        public string Title
        {
            get { return title; }
            set 
            { 
                if (!string.IsNullOrEmpty(value))
                {
                    title = value;

                } else
                {
                    Console.WriteLine("Error, title cannot be empty");
                    return;
                }   
            }
        }

        public string Description
        {
            get { return description; }
            set 
            { 
                
                description = value; 
            }
        }
        
        // Use the local image path 
        public string ImagePath
        {
            get { return imagePath; }
            set 
            { 
                if (!string.IsNullOrEmpty(value))
                {
                    imagePath = value;

                } else
                {
                    Console.WriteLine("Error, imagepath cannot be empty");
                    return;
                }
            }
        }

        public Participant Participant { get; set; }

    }
}
