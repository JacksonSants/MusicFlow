using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Album;

public class Album
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("artistId")]
    [BsonRepresentation(BsonType.String)]
    public Guid ArtistId { get; set; }

    [BsonElement("coverImage")]
    public string CoverImage { get; set; } = string.Empty;

    [BsonElement("releaseDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime ReleaseDate { get; set; }

    [BsonElement("genre")]
    public List<string> Genre { get; set; } = new();

    [BsonElement("totalTracks")]
    public int TotalTracks { get; set; }

    [BsonElement("duration")]
    public int Duration { get; set; } // em segundos

    [BsonElement("label")]
    public string Label { get; set; } = string.Empty;
}
