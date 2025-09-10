using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Model.Artists;

public class Artist
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

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

    [BsonElement("dateCreated")]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [BsonElement("musics")]
    public List<Guid>? Musics { get; set; } = new();

}