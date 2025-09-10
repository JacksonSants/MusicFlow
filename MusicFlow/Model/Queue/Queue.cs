using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Music
{
    public class Queue
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

        [BsonElement("currentIndex")]
        public int CurrentIndex { get; set; } = 0;

        [BsonElement("shuffled")]
        public bool Shuffled { get; set; } = false;

        [BsonElement("repeatMode")]
        public string RepeatMode { get; set; } = "none"; // "none", "one", "all"

        [BsonElement("dateCreated")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
