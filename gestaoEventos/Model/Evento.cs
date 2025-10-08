using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIMongoDB.Model
{
    public class Evento
    {
        [BsonId] // Indica que é a chave primária do documento.
        [BsonRepresentation(BsonType.ObjectId)] // Permite que o MongoDB converta automaticamente string ↔ ObjectId.
        public string? Id { get; set; } // Agora o Swagger entende como string (ou null).

        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Local { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CapacidadeMaxima { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
