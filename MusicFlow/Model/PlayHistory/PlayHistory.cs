using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MusicFlow.Music;

public class PlayHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("musicId")]
    [BsonRepresentation(BsonType.String)]
    public Guid MusicId { get; set; }

    [BsonElement("playedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("duration")]
    public int Duration { get; set; } // Tempo ouvido em segundos

    [BsonElement("completed")]
    public bool Completed { get; set; }
}
