using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MusicFlow.Music;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("targetId")]
    [BsonRepresentation(BsonType.String)]
    public Guid TargetId { get; set; } // Pode ser música, álbum ou artista

    [BsonElement("targetType")]
    public string TargetType { get; set; } = string.Empty; // "music", "album" ou "artist"

    [BsonElement("rating")]
    public int Rating { get; set; } // Nota de 1 a 5

    [BsonElement("comment")]
    public string Comment { get; set; } = string.Empty;

    [BsonElement("dateCreated")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
