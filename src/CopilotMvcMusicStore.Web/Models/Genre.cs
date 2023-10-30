namespace CopilotMvcMusicStore.Web.Models
{
    public class Genre
    {
        //genreid
        public int GenreId { get; set; }
        // name
        public required string Name { get; set; }
        // description
        public string? Description { get; set; }
        //albums
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
