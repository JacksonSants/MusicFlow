using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Music;

public class Favorite
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("musicIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> MusicIds { get; set; } = new();

    [BsonElement("albumIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> AlbumIds { get; set; } = new();

    [BsonElement("artistIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> ArtistIds { get; set; } = new();

    [BsonElement("dateAdded")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}
