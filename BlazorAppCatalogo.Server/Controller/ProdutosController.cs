using BlazorAppCatalogo.Server.Context;
using BlazorAppCatalogo.Server.Util.Extensions;
using BlazorAppCatalogo.Shared.Models;
using BlazorAppCatalogo.Shared.Recursos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this namespace

namespace BlazorAppCatalogo.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProdutosController> _logger; // Add logger field

        public ProdutosController(AppDbContext context, ILogger<ProdutosController> logger) // Add logger parameter in constructor
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get(
                    [FromQuery] Paginacao paginacao,
                    [FromQuery] string nome = "")
        {
            var queryable = _context.Produtos.AsQueryable();

            queryable = queryable.OrderBy(p => p.Id);

            if (!string.IsNullOrEmpty(nome))
            {
                queryable = queryable.Where(x => x.Nome!.Contains(nome));
            }

            await HttpContext.InserirParametroEmPageReponse(queryable, paginacao.QuantidadePorPagina);

            return await queryable.Paginar(paginacao).ToListAsync();

        }

        [HttpGet("{id}", Name = "GetProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) ?? new Produto();
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post(Produto produto)
        {
            _logger.LogInformation("Novo produto cadastrado");
            _logger.LogInformation($"Produto: {produto.Nome}");
            _logger.LogInformation($"Produto: {produto.CategoriaId}");
            _context.Add(produto);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetProduto",
                new { id = produto.Id }, produto);
        }

        [HttpPut]
        public async Task<ActionResult<Produto>> Put(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Produto>> Delete(int id)
        {
            var produto = new Produto { Id = id };
            _context.Remove(produto);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
;