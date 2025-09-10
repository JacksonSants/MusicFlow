using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MusicFlow.Model.Artists;

namespace MusicFlow.Model.Producer;

public class Producer
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonSerializer(typeof(GuidSerializer))]
    public Guid Id { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; }

    [BsonElement("cnpj")]
    public string CNPJ { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("telefone")]
    public string Telefone { get; set; }

    [BsonElement("dataCadastro")]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    [BsonElement("status")]
    public string Status { get; set; } = "Pendente";

    [BsonElement("artistasVinculados")]
    [BsonIgnoreIfNull]
    public List<Artist> ArtistasVinculados { get; set; }
}
