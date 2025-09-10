using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MusicFlow.Data.Dto.Auth;

public class Login
{

    [Required(ErrorMessage = "O email é obrigatório.")]
    [BsonElement("email")]
    [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [BsonElement("password")]
    public string Password { get; set; }
}
