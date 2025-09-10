using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MusicFlow.Exception.UserException;

namespace MusicFlow.Model.User;

public class User
{
    public User()
    {
        Id = Guid.NewGuid();
        DateCreated = DateTime.UtcNow;
        Subscription = "ordinary";
        Level = UserLevel.ordinary_user;
        LastLogin = null;
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonSerializer(typeof(GuidSerializer))]
    public Guid Id { get; set; }

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
    public DateTime DateCreated { get; set; }

    [BsonElement("lastLogin")]
    public DateTime? LastLogin { get; set; }

    [BsonRepresentation(BsonType.String)]
    [BsonElement("level")]
    public UserLevel Level { get; private set; }


    [BsonElement("subscription")]
    public string? Subscription { get; set; }

    public void UpdateLevel(UserLevel newLevel)
    {
        Level = newLevel;
    }

    public void Validate(bool isUpdate = false)
    {
        if (string.IsNullOrEmpty(Username))
            throw new UserValidationException("O nome de usuário é obrigatório.");

        var nomeSobrenomeRegex = new Regex(@"^[A-ZÁÉÍÓÚÂÊÎÔÛÃÕÀÇ][a-záéíóúâêîôûãõàç]+( [A-ZÁÉÍÓÚÂÊÎÔÛÃÕÀÇ][a-záéíóúâêîôûãõàç]+)+$");
        if (!nomeSobrenomeRegex.IsMatch(Username))
            throw new UserValidationException("O nome deve conter nome e sobrenome, ambos iniciando com letra maiúscula e acentos permitidos.");

        if (string.IsNullOrEmpty(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new UserValidationException("O email é inválido.");

        if (!isUpdate || !string.IsNullOrEmpty(Password))
        {
            if (!ValidatePassword(Password))
                throw new UserValidationException("A senha deve conter ao menos 8 caracteres, com letras maiúsculas, minúsculas, números e caracteres especiais.");
        }
    }

    private bool ValidatePassword(string password)
    {
        return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
    }
}