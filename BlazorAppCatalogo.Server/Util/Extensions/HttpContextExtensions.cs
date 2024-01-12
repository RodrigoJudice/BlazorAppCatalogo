using Microsoft.EntityFrameworkCore;

namespace BlazorAppCatalogo.Server.Util.Extensions
{
    public static class HttpContextExtensions
    {
        public async static Task InserirParametroEmPageReponse<T>(
            this HttpContext context,
            IQueryable<T> queryable,
            int quantidadeTotalRegistrosAExbir)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            double quantidadeRegistroTotal = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(quantidadeRegistroTotal / quantidadeTotalRegistrosAExbir);

            context.Response.Headers.Append("quantidadeRegistrosTotal", quantidadeTotalRegistrosAExbir.ToString());
            context.Response.Headers.Append("totalPaginas", totalPaginas.ToString());


        }
    }
}
