namespace APIMongoDB.DTO
{
    public class EventoHateoasDto
    {
        public string? Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string Local { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        public int CapacidadeMaxima { get; set; }

        public DateTime DataCriacao { get; set; }

        public List<LinkDto> Links { get; set; } = new();
    }
}
