using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Music
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty; // ex: "new_release", "follow", "like"

        [BsonElement("message")]
        public string Message { get; set; } = string.Empty;

        [BsonElement("isRead")]
        public bool IsRead { get; set; } = false;

        [BsonElement("dateCreated")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [BsonElement("relatedId")]
        [BsonRepresentation(BsonType.String)]
        public Guid? RelatedId { get; set; } // Pode ser null se não tiver objeto relacionado
    }
}
