using BarraByTech.Models;
using BarraByTech.Models.DTO;
using BarraByTech.Models.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BarraByTech.Services
{
    public class ClienteService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(HttpClient http, ILogger<ClienteService> logger)
        {
            _http = http;
            _http.DefaultRequestHeaders.Accept.Clear();
            _logger = logger;
        }

        /* --------------------------------------------------------------
         *                           CLIENTE
         * -------------------------------------------------------------- */

        public async Task<bool> PostCliente(ClienteDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/Clientes", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<ClienteDTO?> GetClienteByUserId(string userId)
        {
            var response = await _http.GetAsync($"api/Clientes/GetByUser/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClienteDTO>(content);
        }


        /* --------------------------------------------------------------
         *                           PRODUTOS
         * -------------------------------------------------------------- */

        public async Task<List<ProdutoDTO>> GetProdutos()
        {
            var response = await _http.GetAsync("api/Produtos");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProdutoDTO>>(json)!;
        }

        public async Task<List<ProdutoDTO>> SearchProdutos(string query)
        {
            var response = await _http.GetAsync($"api/Produtos/search?q={query}");
            if (!response.IsSuccessStatusCode)
                return new List<ProdutoDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProdutoDTO>>(json)!;
        }

        public async Task<ProdutoDTO?> GetProdutoById(Guid produtoId)
        {
            var response = await _http.GetAsync($"api/Produtos/{produtoId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProdutoDTO>(json);
        }

        public async Task<List<ProdutoDTO>> GetProdutoCategoria(Guid categoriaId)
        {
            var produtos = await _http.GetFromJsonAsync<List<ProdutoDTO>>(
                $"api/Produtos/GetPorCategoria/{categoriaId}"
            );

            return produtos ?? new List<ProdutoDTO>();
        }


        /* --------------------------------------------------------------
         *                           CATEGORIAS
         * -------------------------------------------------------------- */

        public async Task<List<CategoriaDTO>> GetCategorias()
        {
            var response = await _http.GetAsync("api/Categorias");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CategoriaDTO>>(json)!;
        }

        public async Task<CategoriaDTO?> GetNomeCategoria(string categoriaId)
        {
            var response = await _http.GetAsync($"api/Categorias/GetByCategoria/{categoriaId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CategoriaDTO>(content);
        }


        /* --------------------------------------------------------------
         *                           MARCAS
         * -------------------------------------------------------------- */

        public async Task<List<MarcaDTO>> GetMarcas()
        {
            var response = await _http.GetAsync("api/Marcas");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<MarcaDTO>>(json)!;
        }

        public async Task<MarcaDTO?> GetMarca(string marcaId)
        {
            var response = await _http.GetAsync($"api/Marcas/{marcaId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MarcaDTO>(content);
        }

        public async Task<List<TiposMarcaDTO>> GetTiposMarcaPorMarcaId(Guid marcaId)
        {
            var response = await _http.GetAsync($"api/TiposMarcas/PorMarca?marcaId={marcaId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TiposMarcaDTO>>(json)!;
        }

        public async Task<TiposMarcaDTO?> GetTipoMarca(string tipoMarcaId)
        {
            var response = await _http.GetAsync($"api/TiposMarcas/{tipoMarcaId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TiposMarcaDTO>(content);
        }

        public async Task<List<TiposMarcaDTO>> GetTodosTiposMarca()
        {
            var response = await _http.GetAsync("api/TiposMarcas");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<TiposMarcaDTO>>() ?? new List<TiposMarcaDTO>();
        }


        /* --------------------------------------------------------------
         *                       FAVORITOS DO CLIENTE
         * -------------------------------------------------------------- */

        public async Task<List<ItensFavoritos>> GetFavoritos()
        {
            var response = await _http.GetAsync("api/ItensFavoritos");
            if (!response.IsSuccessStatusCode)
                return new List<ItensFavoritos>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ItensFavoritos>>(json) ?? new List<ItensFavoritos>();
        }

        public async Task<bool> AdicionarFavorito(ItensFavoritos favorito)
        {
            var response = await _http.PostAsJsonAsync("api/ItensFavoritos", favorito);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoverFavorito(Guid itemFavoritoId)
        {
            var response = await _http.DeleteAsync($"api/ItensFavoritos/{itemFavoritoId}");
            return response.IsSuccessStatusCode;
        }


        /* --------------------------------------------------------------
         *                       CARRINHO DE COMPRAS
         * -------------------------------------------------------------- */

        public async Task<List<ItemCarrinhoDTO>> GetCarrinho()
        {
            var response = await _http.GetAsync("api/ItemCarrinho");
            if (!response.IsSuccessStatusCode)
                return new List<ItemCarrinhoDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ItemCarrinhoDTO>>(json)!;
        }

        public async Task<ItemCarrinhoDTO?> GetItemCarrinho(Guid id)
        {
            var response = await _http.GetAsync($"api/ItemCarrinho/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ItemCarrinhoDTO>(json);
        }

        public async Task<bool> AdicionarItemCarrinho(ItemCarrinhoDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/ItemCarrinho", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarItemCarrinho(Guid id, ItemCarrinhoDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/ItemCarrinho/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoverItemCarrinho(Guid id)
        {
            var response = await _http.DeleteAsync($"api/ItemCarrinho/{id}");
            return response.IsSuccessStatusCode;
        }


        /* --------------------------------------------------------------
         *                     ENDEREÇOS DO CLIENTE
         * -------------------------------------------------------------- */

        public async Task<List<EnderecoClienteDTO>> GetEnderecosDoUsuarioAsync(Guid userId)
        {
            return await _http.GetFromJsonAsync<List<EnderecoClienteDTO>>(
                $"api/EnderecosCliente/user/{userId}"
            ) ?? new List<EnderecoClienteDTO>();
        }

        public async Task<EnderecoClienteDTO?> GetEnderecoByIdAsync(Guid enderecoId)
        {
            return await _http.GetFromJsonAsync<EnderecoClienteDTO>(
                $"api/EnderecosCliente/{enderecoId}"
            );
        }

        public async Task<bool> CreateEnderecoAsync(EnderecoClienteDTO dto)
        {
            var result = await _http.PostAsJsonAsync("api/EnderecosCliente", dto);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEnderecoAsync(EnderecoClienteDTO dto)
        {
            var result = await _http.PutAsJsonAsync(
                $"api/EnderecosCliente/{dto.EnderecoId}", dto
            );
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEnderecoAsync(Guid enderecoId, Guid userId)
        {
            var result = await _http.DeleteAsync(
                $"api/EnderecosCliente/{enderecoId}?userId={userId}"
            );
            return result.IsSuccessStatusCode;
        }

        /* --------------------------------------------------------------
         *                           PEDIDOS
         * -------------------------------------------------------------- */

        public async Task<bool> CriarPedidoAsync(PedidoDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/Pedidos", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<PedidoDTO?> GetPedidoAsync(Guid pedidoId)
        {
            var response = await _http.GetAsync($"api/Pedidos/{pedidoId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PedidoDTO>(json);
        }

        public async Task<List<PedidoDTO>> GetPedidosDoUsuarioAsync(Guid userId)
        {
            var response = await _http.GetAsync($"api/Pedidos/PorUsuario/{userId}");
            if (!response.IsSuccessStatusCode)
                return new List<PedidoDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PedidoDTO>>(json) ?? new List<PedidoDTO>();
        }

        public async Task<bool> DeletarPedidoAsync(Guid pedidoId)
        {
            var response = await _http.DeleteAsync($"api/Pedidos/{pedidoId}");
            return response.IsSuccessStatusCode;
        }

        /* --------------------------------------------------------------
        *                          ITENS DO PEDIDO
        * -------------------------------------------------------------- */

        public async Task<bool> CriarItemAsync(ItemPedidoDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/ItemPedidos", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AtualizarItemAsync(Guid id, ItemPedidoDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/ItemPedido/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoverItemAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/ItemPedido/{id}");
            return response.IsSuccessStatusCode;
        }

        /* --------------------------------------------------------------
         *                          PAGAMENTOS
         * -------------------------------------------------------------- */

        public async Task<bool> RegistrarPagamentoAsync(ComprasDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/Compras", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<ComprasDTO?> GetPagamentoAsync(Guid compraId)
        {
            var response = await _http.GetAsync($"api/Compras/{compraId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ComprasDTO>(json);
        }

        public async Task<List<ComprasDTO>> GetPagamentosDoPedidoAsync(Guid pedidoId)
        {
            var response = await _http.GetAsync($"api/Compras/PorPedido/{pedidoId}");
            if (!response.IsSuccessStatusCode)
                return new List<ComprasDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ComprasDTO>>(json) ?? new List<ComprasDTO>();
        }
        public async Task<List<ComprasDTO>> GetComprasDoUsuarioAsync(Guid userId)
        {
            var response = await _http.GetAsync($"api/Compras/PorUsuario/{userId}");
            if (!response.IsSuccessStatusCode)
                return new List<ComprasDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ComprasDTO>>(json) ?? new List<ComprasDTO>();
        }
        public async Task<bool> AtualizarStatusPagamentoAsync(Guid id, StatusPagamento status)
        {
            var response = await _http.PutAsJsonAsync($"api/Compras/AtualizarStatus/{id}", status);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ItemPedidoDTO>> GetItensPedidoAsync(Guid pedidoId)
        {
            var response = await _http.GetFromJsonAsync<List<ItemPedidoDTO>>(
                $"api/ItemPedidos/PedidoId/{pedidoId}"
            );
            return response ?? new List<ItemPedidoDTO>();
        }
    }
}
