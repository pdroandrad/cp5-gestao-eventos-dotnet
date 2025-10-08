using APIMongoDB.Model;
using MongoDB.Driver;
using MongoDB.Bson;
using APIMongoDB.DTO;

namespace APIMongoDB.Services
{
    public class ServicoEventos
    {
        private readonly IMongoCollection<Evento> _colecaoEventos;

        public ServicoEventos(IMongoCollection<Evento> colecaoEventos)
        {
            _colecaoEventos = colecaoEventos;
        }

        // ==============================
        // MÉTODOS CRUD BÁSICOS
        // ==============================

        public async Task<List<Evento>> ObterTodosAsync() =>
            await _colecaoEventos.Find(_ => true).ToListAsync();

        public async Task<Evento?> ObterPorIdAsync(string id)
        {
            // Converte a string em ObjectId de forma segura
            var filtro = Builders<Evento>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _colecaoEventos.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<Evento> CriarAsync(Evento evento)
        {
            await _colecaoEventos.InsertOneAsync(evento);
            return evento;
        }

        public async Task<bool> AtualizarAsync(string id, Evento eventoAtualizado)
        {
            var filtro = Builders<Evento>.Filter.Eq("_id", ObjectId.Parse(id));
            var resultado = await _colecaoEventos.ReplaceOneAsync(filtro, eventoAtualizado);
            return resultado.MatchedCount > 0;
        }

        public async Task<bool> RemoverAsync(string id)
        {
            var filtro = Builders<Evento>.Filter.Eq("_id", ObjectId.Parse(id));
            var resultado = await _colecaoEventos.DeleteOneAsync(filtro);
            return resultado.DeletedCount > 0;
        }

        // ==============================
        // PAGINAÇÃO
        // ==============================

        public async Task<List<Evento>> ObterPaginadoAsync(int pageNumber, int pageSize)
        {
            return await _colecaoEventos
                .Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<long> ContarAsync()
        {
            return await _colecaoEventos.CountDocumentsAsync(_ => true);
        }

        // ==============================
        // HATEOAS
        // ==============================

        public EventoHateoasDto MontarEventoComLinks(Evento evento, string urlBase)
        {
            var dto = new EventoHateoasDto
            {
                Id = evento.Id,
                Titulo = evento.Titulo,
                Descricao = evento.Descricao,
                Data = evento.Data,
                Local = evento.Local,
                Categoria = evento.Categoria,
                CapacidadeMaxima = evento.CapacidadeMaxima,
                DataCriacao = evento.DataCriacao,
                Links = new List<LinkDto>
                {
                    new LinkDto { Href = $"{urlBase}/eventos/{evento.Id}", Rel = "self", Method = "GET" },
                    new LinkDto { Href = $"{urlBase}/eventos/{evento.Id}", Rel = "update", Method = "PUT" },
                    new LinkDto { Href = $"{urlBase}/eventos/{evento.Id}", Rel = "delete", Method = "DELETE" }
                }
            };

            return dto;
        }
    }
}
