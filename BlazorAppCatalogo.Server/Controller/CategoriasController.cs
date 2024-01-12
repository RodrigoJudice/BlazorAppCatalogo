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
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get(
            [FromQuery] Paginacao paginacao,
            [FromQuery] string nome = "")
        {
            var queryable = _context.Categorias.AsQueryable();

            queryable = queryable.OrderBy(p => p.Id);

            if (!string.IsNullOrEmpty(nome))
            {
                queryable = queryable.Where(x => x.Nome!.Contains(nome));
            }

            await HttpContext.InserirParametroEmPageReponse(queryable, paginacao.QuantidadePorPagina);

            return await queryable.Paginar(paginacao).ToListAsync();

        }

        [HttpGet("todas")]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            return await _context.Categorias.AsNoTracking().OrderBy(p => p.Nome).ToListAsync();
        }


        [HttpGet("{id}", Name = "GetCategoria")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            return await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) ?? new Categoria();
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetCategoria",
                new { id = categoria.Id }, categoria);
        }

        [HttpPut]
        public async Task<ActionResult<Categoria>> Put(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            var categoria = new Categoria { Id = id };
            _context.Remove(categoria);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
;