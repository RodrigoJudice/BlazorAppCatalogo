using BlazorAppCatalogo.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorAppCatalogo.Client.Pages.Categorias
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

        protected Categoria categoria { get; set; } = new Categoria();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetFocus("txtCategoriaNome");
            }

        }

        protected async Task GetFocus(string elementId)
        {
            await Js!.InvokeVoidAsync("focusById", elementId);
        }

        protected override async Task OnParametersSetAsync()
        {
            categoria = await http!.GetFromJsonAsync<Categoria>($"api/Categorias/{Id}") ?? new Categoria();

        }
        protected async Task SalvarCategoria()
        {
            try
            {
                if (Id == 0)
                    await http!.PostAsJsonAsync("api/Categorias", categoria);

                else
                    await http!.PutAsJsonAsync($"api/Categorias", categoria);

                navigation!.NavigateTo("categorias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
