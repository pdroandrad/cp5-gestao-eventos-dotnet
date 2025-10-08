using Microsoft.AspNetCore.Mvc;
using APIMongoDB.Services;
using APIMongoDB.Model;
using Microsoft.AspNetCore.Authorization;

namespace APIMongoDB.Controllers
{
    [ApiController] // Indica que esta classe é um controller de API.
    [Route("api/[controller]")] // Define a rota base para este controller: "api/eventos".
    [Authorize]
    public class EventosController : Controller
    {
        private readonly ServicoEventos _servico; // Serviço responsável pelas operações com eventos.

        // Construtor que recebe o serviço de eventos via injeção de dependência.
        public EventosController(ServicoEventos servico)
        {
            _servico = servico;
        }

        // ==============================
        // MÉTODO GET COM PAGINAÇÃO
        // ==============================
        [HttpGet] // Responde a requisições HTTP GET na rota base.
        public async Task<ActionResult<List<Evento>>> ObterTodos(
            [FromQuery] int pageNumber = 1, // Número da página (padrão: 1).
            [FromQuery] int pageSize = 10)  // Tamanho da página (padrão: 10).
        {
            // Busca apenas os eventos da página solicitada.
            var eventos = await _servico.ObterPaginadoAsync(pageNumber, pageSize);

            // Conta o total de registros no banco.
            var total = await _servico.ContarAsync();

            // Adiciona o total no cabeçalho da resposta HTTP.
            Response.Headers.Add("X-Total-Count", total.ToString());

            // Retorna os eventos da página solicitada.
            return eventos;
        }

        // ==============================
        // MÉTODO GET POR ID
        // ==============================
        [HttpGet("{id:length(24)}", Name = "ObterEvento")] // Rota: api/eventos/{id}
        public async Task<ActionResult<Evento>> ObterPorId(string id)
        {
            var evento = await _servico.ObterPorIdAsync(id);
            if (evento == null) return NotFound(); // Retorna 404 se não encontrar.

            // Monta URL base da API (ex: http://localhost:5000/api)
            var urlBase = $"{Request.Scheme}://{Request.Host}/api";

            // Adiciona links HATEOAS ao evento retornado.
            var eventoHateoas = _servico.MontarEventoComLinks(evento, urlBase);

            return Ok(eventoHateoas); // Retorna 200 OK com o evento e os links.
        }

        // ==============================
        // MÉTODO POST (CRIAR)
        // ==============================
        [HttpPost] // Rota: api/eventos
        public async Task<ActionResult<Evento>> Criar(Evento evento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Retorna 400 se o modelo for inválido.

            await _servico.CriarAsync(evento); // Cria o evento no banco.

            // Retorna 201 Created com link para o novo evento.
            return CreatedAtRoute("ObterEvento", new { id = evento.Id }, evento);
        }

        // ==============================
        // MÉTODO PUT (ATUALIZAR)
        // ==============================
        [HttpPut("{id:length(24)}")] // Rota: api/eventos/{id}
        public async Task<IActionResult> Atualizar(string id, Evento eventoAtualizado)
        {
            var eventoExistente = await _servico.ObterPorIdAsync(id);
            if (eventoExistente is null) return NotFound();

            eventoAtualizado.Id = eventoExistente.Id; // Garante que o ID não muda.
            var atualizado = await _servico.AtualizarAsync(id, eventoAtualizado);

            return atualizado ? NoContent() : NotFound();
        }

        // ==============================
        // MÉTODO DELETE (REMOVER)
        // ==============================
        [HttpDelete("{id:length(24)}")] // Rota: api/eventos/{id}
        public async Task<IActionResult> Remover(string id)
        {
            var eventoExistente = await _servico.ObterPorIdAsync(id);
            if (eventoExistente is null) return NotFound();

            var removido = await _servico.RemoverAsync(id);
            return removido ? NoContent() : NotFound();
        }
    }
}
