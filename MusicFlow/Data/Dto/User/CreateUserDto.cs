using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MusicFlow.Model.User;

namespace MusicFlow.Data.Dto.CreateUserDto;

public class CreateUserDto
{
    [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    [BsonElement("username")]
    public string Username { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [BsonElement("email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [BsonElement("password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
    [BsonIgnore]
    public string RePassword { get; set; }

    [BsonElement("profileImage")]
    public string? ProfileImage { get; set; }

    [BsonElement("dateCreated")]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [BsonElement("lastLogin")]
    public DateTime? LastLogin { get; set; }

    [BsonElement("level")]
    [BsonRepresentation(BsonType.String)]
    public UserLevel Level { get; set; } = UserLevel.ordinary_user;

    [BsonElement("subscription")]
    public string? Subscription { get; set; }
}