using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MusicFlow.Music;

public class Follow
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("followerId")]
    [BsonRepresentation(BsonType.String)]
    public Guid FollowerId { get; set; }

    [BsonElement("followedId")]
    [BsonRepresentation(BsonType.String)]
    public Guid FollowedId { get; set; }

    [BsonElement("followedType")]
    public string FollowedType { get; set; } = string.Empty; // "user" ou "artist"

    [BsonElement("dateFollowed")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateFollowed { get; set; } = DateTime.UtcNow;
}
