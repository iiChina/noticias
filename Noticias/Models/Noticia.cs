using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Noticias.Models;

public class Noticia
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("titulo")]
    public string Titulo { get; set; }

    [BsonElement("resumo")]
    public string Resumo { get; set; }

    [BsonElement("texto")]
    public string Texto { get; set; }

    [BsonElement("autor")]
    public string Autor { get; set; }
}
