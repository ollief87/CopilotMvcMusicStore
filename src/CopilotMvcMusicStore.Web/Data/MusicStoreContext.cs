using CopilotMvcMusicStore.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CopilotMvcMusicStore.Web.Data
{
    public class MusicStoreContext : DbContext
    {
        //constructor that accepts dbcontextoptions type musicstorecontext
        public MusicStoreContext(DbContextOptions<MusicStoreContext> options) : base(options)
        {
        }

        // override onmodelcreating method with mappings for albums genres and artists
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>().ToTable("Album");
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<Artist>().ToTable("Artist");
            modelBuilder.Entity<MusicStoreSummary>().ToView("vwMusicStoreSummary").HasNoKey();
        }

        // dbset properties for albums genres and artists
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<MusicStoreSummary> MusicStoreSummaries { get; set;}
    }
}
