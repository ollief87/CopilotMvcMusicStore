namespace CopilotMvcMusicStore.Web.Models
{
    public class MusicStoreSummary
    {
        // Properties - summarizes the number of albums & artists per genre
        public int GenreId { get; set; }
        public string? GenreName { get; set; }
        public int AlbumCount { get; set; }
        public int ArtistCount { get; set; }
    }
}
