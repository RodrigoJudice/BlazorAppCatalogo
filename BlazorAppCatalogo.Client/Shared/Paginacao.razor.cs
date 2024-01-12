using Microsoft.AspNetCore.Components;

namespace BlazorAppCatalogo.Client.Shared
{

    public partial class Paginacao
    {
        [Parameter]
        public int paginaAtual { get; set; } = 1;

        [Parameter]
        public int QuantidadeTotalPaginas { get; set; }

        [Parameter]
        public int Raio { get; set; } = 3;

        [Parameter]
        public EventCallback<int> PaginaSelecionada { get; set; }

        List<LinkModel> links = new List<LinkModel>();

        private async Task PaginaSelecionadaLink(LinkModel link)
        {
            if (link.Pagina == paginaAtual) return;
            if (!link.Ativo) return;

            paginaAtual = link.Pagina;
            await PaginaSelecionada.InvokeAsync(link.Pagina);


        }

        protected override void OnParametersSet()
        {
            CarregaPaginas();
        }

        private void CarregaPaginas()
        {
            links = new List<LinkModel>();

            //tratar o link da pagina anterior
            var isLinkAnteriorHabilitado = paginaAtual != 1;
            var paginaAnterior = paginaAtual - 1;

            links.Add(new LinkModel(paginaAnterior, isLinkAnteriorHabilitado, "Anterior"));

            //tratar os links das paginas especificas
            for (int i = 1; i <= QuantidadeTotalPaginas; i++)
            {
                if (i > paginaAtual - Raio && i <= paginaAtual + Raio)
                {
                    links.Add(new LinkModel(i)
                    { Destacado = paginaAtual == i });
                }
            }

            //tratar o link da pagina posterior
            var isLinkPosteriorHabilitado = paginaAtual != QuantidadeTotalPaginas;
            var paginaPosterior = paginaAtual + 1;

            links.Add(new LinkModel(paginaPosterior, isLinkPosteriorHabilitado, "Próximo"));


        }
    }



    class LinkModel
    {
        public LinkModel(int pagina) : this(pagina, true)
        {

        }
        public LinkModel(int pagina, bool ativo) : this(pagina, ativo, pagina.ToString())
        {

        }
        public LinkModel(int pagina, bool ativo, string texto)
        {
            Pagina = pagina;
            Ativo = ativo;
            Texto = texto;
        }

        public string Texto { get; set; }
        public int Pagina { get; set; }
        public bool Ativo { get; set; } = true;
        public bool Destacado { get; set; } = false;
    }
}
