namespace MusicFlow.Data.Dto.ArtistWithMusic;

public class ArtistWithMusicsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
    public string? ProfileImage { get; set; }
    public string Country { get; set; }
    public List<string> Genre { get; set; }
    public int Followers { get; set; }
    public bool Verified { get; set; }
    public DateTime DateCreated { get; set; }

    public List<MusicDto> Musics { get; set; } = new();
}
