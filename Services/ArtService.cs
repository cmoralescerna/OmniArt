using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniArt.Models;
using OmniArt.EF_Data;
using Microsoft.EntityFrameworkCore;

namespace OmniArt.Services
{
    public class ArtService
    {
        private AppDbContext context;

        public ArtService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> UploadArtAsync(Gallery gallery, Art art)
        {     
            if (gallery == null || gallery.Status != GalleryStatus.Upcoming)
            {
                return false;
            }

            gallery.ArtPieces.Add(art);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveArtAsync(Gallery gallery, Art art)
        {
            if (gallery == null)
            {
                return false;
            }

            if (art == null)
            {
                return false;
            }

            gallery.ArtPieces.Remove(art);
            await context.SaveChangesAsync();
            return true;      
        }

        public async Task<List<Art>> GetArtByParticipantAsync(string participantId)
        {
            var participant = await context.Participants.Include(p => p.Portfolio).SingleOrDefaultAsync(p => p.ParticipantId == participantId);

            if (participant == null) 
            {
                throw new Exception("Participant does not exist!");

            } else if (participant.Portfolio == null)
            {
                throw new Exception("Portfolio does not exist!");
            }

            return participant.Portfolio;
        }
    }
}
