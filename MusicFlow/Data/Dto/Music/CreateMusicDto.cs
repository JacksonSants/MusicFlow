using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MusicFlow.Data.Dto.Music;

public class CreateMusicDto
{
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("artistId")]
    [BsonRepresentation(BsonType.String)]
    public Guid ArtistId { get; set; }

    [BsonElement("albumId")]
    [BsonRepresentation(BsonType.String)]
    public Guid AlbumId { get; set; }

    [BsonElement("duration")]
    public int Duration { get; set; } 

    [BsonElement("fileUrl")]
    public string FileUrl { get; set; } = string.Empty;

    [BsonElement("genre")]
    public List<string> Genre { get; set; } = new();

    [BsonElement("trackNumber")]
    public int TrackNumber { get; set; }

    [BsonElement("plays")]
    public int Plays { get; set; } = 0;

    [BsonElement("likes")]
    public int Likes { get; set; } = 0;

    [BsonElement("dateAdded")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}


