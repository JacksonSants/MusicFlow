using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Model.Library;

public class Library
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("albumIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> AlbumIds { get; set; } = new();

    [BsonElement("playlistIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> PlaylistIds { get; set; } = new();

    [BsonElement("followedArtistIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> FollowedArtistIds { get; set; } = new();

    [BsonElement("dateCreated")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
