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
    public class GalleryService
    {
        private AppDbContext context;

        public GalleryService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Gallery> CreateGalleryAsync(Gallery gallery)
        {
            // Create a gallery in the gallery manager with the following paramete
            if (gallery.Host != null)
            {
                context.Hosts.Add(gallery.Host);
            }

            context.Galleries.Add(gallery);
            await context.SaveChangesAsync();

            return gallery;
        }

        public async Task<List<Gallery>> GetAllGalleriesAsync()
        {
            return await context.Galleries
                                .Include(g => g.Host)
                                .ToListAsync();
        }

        public async Task<bool> DeleteGalleryAsync(string galleryId)
        {
            var gallery = await context.Galleries.FirstOrDefaultAsync(g => g.GalleryId == galleryId);

            if (gallery == null)
            {
                return false;
            }

            context.Galleries.Remove(gallery);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditGalleryAsync(Gallery updatedGallery)
        {
            var currentGallery = await context.Galleries
                                        .Include(g => g.Host)
                                        .FirstOrDefaultAsync(g => g.GalleryId == updatedGallery.GalleryId);

            if (currentGallery == null)
            {
                return false;
            }

            // update the gallery's properties
            currentGallery.Title = updatedGallery.Title;
            currentGallery.Description = updatedGallery.Description;
            currentGallery.Date = updatedGallery.Date;
            currentGallery.StartTime = updatedGallery.StartTime;
            currentGallery.EndTime = updatedGallery.EndTime;
            currentGallery.Host.FirstName = updatedGallery.Host.FirstName;
            currentGallery.Host.LastName = updatedGallery.Host.LastName;

            await context.SaveChangesAsync();
            return true;
        }
    }
}
