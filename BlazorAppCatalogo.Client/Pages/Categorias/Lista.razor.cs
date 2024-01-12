using BlazorAppCatalogo.Client.Shared;
using BlazorAppCatalogo.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BlazorAppCatalogo.Client.Pages.Categorias;

public partial class Lista
{
    protected int QuantidadeTotalPaginas;
    protected int paginaAtual = 1;
    protected string nomeFiltro = string.Empty;


    [Inject]
    protected HttpClient? _http { get; set; }

    [Inject]
    protected NavigationManager? _navigation { get; set; }

    protected List<Categoria>? categorias { get; set; } = null;

    protected Confirma? confirmaExclusao;
    protected int codigoCategoriaExclusao;

    protected override async Task OnInitializedAsync()
    {
        await CarregaCagetorias();
    }

    protected void ConfirmarExcluisaoTarefa(int id)
    {
        confirmaExclusao!.Exibir();
        codigoCategoriaExclusao = id;
    }

    protected void CancelaExclusao()
    {
        confirmaExclusao!.Ocultar();
        codigoCategoriaExclusao = 0;
    }

    protected async Task ConfirmaExclusao()
    {
        await _http!.DeleteAsync($"api/categorias/{codigoCategoriaExclusao}");
        confirmaExclusao!.Ocultar();
        await CarregaCagetorias(paginaAtual);
    }

    protected async Task PaginaSelecionada(int pagina)
    {
        paginaAtual = pagina;
        await CarregaCagetorias(pagina);
    }

    protected async Task Filtrar()
    {
        paginaAtual = 1;
        await CarregaCagetorias();

    }

    protected async Task LimparFiltro()
    {
        nomeFiltro = string.Empty;
        paginaAtual = 1;
        await CarregaCagetorias();


    }

    private async Task CarregaCagetorias(int pagina = 1, int quantidadePorPagina = 5)
    {
        var httpResponse =
            await _http!.GetAsync($"api/categorias?pagina={pagina}" +
            $"&quantidadePorPagina={quantidadePorPagina}" +
            $"&nome={nomeFiltro}");

        if (!httpResponse.IsSuccessStatusCode) return;

        QuantidadeTotalPaginas =
            int.Parse(httpResponse.Headers.GetValues("totalPaginas")
            .FirstOrDefault()!);

        var content = await httpResponse.Content.ReadAsStringAsync();

        categorias = JsonSerializer.Deserialize<List<Categoria>>(content,
                                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
