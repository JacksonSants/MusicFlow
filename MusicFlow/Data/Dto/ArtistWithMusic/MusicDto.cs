namespace MusicFlow.Data.Dto.ArtistWithMusic;

public class MusicDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
}
