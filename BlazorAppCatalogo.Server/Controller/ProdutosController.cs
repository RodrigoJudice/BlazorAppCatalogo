using BlazorAppCatalogo.Server.Context;
using BlazorAppCatalogo.Server.Util.Extensions;
using BlazorAppCatalogo.Shared.Models;
using BlazorAppCatalogo.Shared.Recursos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppCatalogo.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get(
            [FromQuery] Paginacao paginacao,
            [FromQuery] string nome = "")
        {
            var queryable = _context.Produtos.AsQueryable();

            queryable = queryable.OrderBy(p => p.ProdutoId);

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
            return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.ProdutoId == id) ?? new Produto();
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post(Produto produto)
        {
            _context.Add(produto);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetProduto",
                new { id = produto.ProdutoId }, produto);
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
            var produto = new Produto { ProdutoId = id };
            _context.Remove(produto);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
;