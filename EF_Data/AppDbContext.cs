using Microsoft.EntityFrameworkCore;
using OmniArt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniArt.EF_Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Art> ArtPieces { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Host> Hosts { get; set; }

        // Modify AppDbContext setup to use an in-memory database instead of a real one
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
