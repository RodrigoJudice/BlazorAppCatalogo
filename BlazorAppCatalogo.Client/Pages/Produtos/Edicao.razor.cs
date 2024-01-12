using BlazorAppCatalogo.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorAppCatalogo.Client.Pages.Produtos
{
    public partial class Edicao
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected HttpClient? http { get; set; }

        [Inject]
        protected NavigationManager? navigation { get; set; }

        [Inject]
        protected IJSRuntime? Js { get; set; }

        protected Produto Produto { get; set; } = new Produto();

        protected List<Categoria> Categorias { get; set; } = [];


        protected override async Task OnInitializedAsync()
        {
            Categorias = await http!.GetFromJsonAsync<List<Categoria>>("api/categorias/todas") ?? [];

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetFocus("txtProdutoNome");
            }

        }

        protected async Task GetFocus(string elementId)
        {
            await Js!.InvokeVoidAsync("focusById", elementId);
        }

        protected override async Task OnParametersSetAsync()
        {
            Produto = await http!.GetFromJsonAsync<Produto>($"api/Produtos/{Id}") ?? new Produto();

        }
        protected async Task SalvarProduto()
        {

            try
            {
                if (Id == 0)
                    await http!.PostAsJsonAsync("api/Produtos", Produto);

                else
                    await http!.PutAsJsonAsync($"api/Produtos", Produto);

                navigation!.NavigateTo("Produtos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
