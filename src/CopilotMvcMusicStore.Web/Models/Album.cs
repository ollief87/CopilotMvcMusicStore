using System.ComponentModel.DataAnnotations;

namespace CopilotMvcMusicStore.Web.Models
{
    public class Album
    {
        // albumid
        public int AlbumId { get; set; }
        //genreid
        public int GenreId { get; set; }
        //artistid
        public int ArtistId { get; set; }
        // Title
        [Required]
        public required string Title { get; set; }
        //price
        [Required]
        public decimal Price { get; set; }
        // albumarturl
        public string? AlbumArtUrl { get; set; }
        // Genre
        public Genre? Genre { get; set; }
        //artist
        public Artist? Artist { get; set; }
    }
}
