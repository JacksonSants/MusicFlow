using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Data.Dto.Artist;

public class UpdateArtistDto
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("biography")]
    public string Biography { get; set; }

    [BsonElement("profileImage")]
    public string? ProfileImage { get; set; }

    [BsonElement("country")]
    public string Country { get; set; }

    [BsonElement("genre")]
    public List<string> Genre { get; set; } = new();

    [BsonElement("followers")]
    public int Followers { get; set; } = 0;

    [BsonElement("verified")]
    public bool Verified { get; set; } = false;
}
