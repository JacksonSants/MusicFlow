using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Music;

public class Playlist
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("musicIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> MusicIds { get; set; } = new();

    [BsonElement("coverImage")]
    public string CoverImage { get; set; } = string.Empty;

    [BsonElement("isPublic")]
    public bool IsPublic { get; set; } = false;

    [BsonElement("dateCreated")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [BsonElement("dateModified")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}
