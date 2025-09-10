using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicFlow.Data.Dto.User;

public class UpdateUserDto
{
    [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    [BsonElement("username")]
    public string Username { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("profileImage")]
    public string? ProfileImage { get; set; }

    [BsonElement("subscription")]
    public string? Subscription { get; set; }
}
