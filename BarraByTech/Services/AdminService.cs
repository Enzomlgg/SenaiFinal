using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http.Headers;
using System.IO;
using BarraByTech.Models.DTO;

namespace BarraByTech.Services
{
    public class AdminService
    {
        private readonly HttpClient _http;
        private readonly ILogger<AdminService> _logger;

        public AdminService(HttpClient http, ILogger<AdminService> logger)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7289/");
            _http.DefaultRequestHeaders.Accept.Clear();
            _logger = logger;
        }

        #region Funções Admin - Categorias

        public async Task<CategoriaDTO> CriarCategoria(CategoriaDTO categoria)
        {
            var response = await _http.PostAsJsonAsync("api/Categorias", categoria);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CategoriaDTO>()!;
        }

        public async Task EditarCategoria(CategoriaDTO categoria)
        {
            var response = await _http.PutAsJsonAsync($"api/Categorias/{categoria.CategoriaId}", categoria);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoverCategoria(Guid categoriaId)
        {
            var response = await _http.DeleteAsync($"api/Categorias/{categoriaId}");
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region Funções Admin - Marcas

        public async Task<MarcaDTO> CriarMarca(MarcaDTO marca)
        {
            var response = await _http.PostAsJsonAsync("api/Marcas", marca);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MarcaDTO>()!;
        }

        public async Task EditarMarca(MarcaDTO marca)
        {
            var response = await _http.PutAsJsonAsync($"api/Marcas/{marca.MarcaId}", marca);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoverMarca(Guid marcaId)
        {
            var response = await _http.DeleteAsync($"api/Marcas/{marcaId}");
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region Funções Admin - Tipos de Marca

        public async Task<TiposMarcaDTO> CriarTipoMarca(TiposMarcaDTO tipoMarca)
        {
            var response = await _http.PostAsJsonAsync("api/TiposMarcas", tipoMarca);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TiposMarcaDTO>()!;
        }

        public async Task EditarTipoMarca(TiposMarcaDTO tipoMarca)
        {
            var response = await _http.PutAsJsonAsync($"api/TiposMarcas/{tipoMarca.TipoMarcaId}", tipoMarca);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoverTipoMarca(Guid tipoMarcaId)
        {
            var response = await _http.DeleteAsync($"api/TiposMarcas/{tipoMarcaId}");
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region Funções Admin - Produtos

        public async Task<List<ProdutoDTO>> GetProdutos()
        {
            // Endpoint para buscar a lista de produtos (apenas dados de visualização, sem imagens)
            var produtos = await _http.GetFromJsonAsync<List<ProdutoDTO>>("api/Produtos");
            return produtos ?? new List<ProdutoDTO>();
        }

        public async Task<ProdutoDTO> GetProdutoById(Guid produtoId)
        {
            // Novo endpoint para buscar o produto completo (com todos os detalhes para edição)
            var produto = await _http.GetFromJsonAsync<ProdutoDTO>($"api/Produtos/{produtoId}");
            return produto!;
        }

        public async Task<ProdutoDTO> CriarProduto(ProdutoDTO produto, IFormFileCollection files)
        {
            using var form = new MultipartFormDataContent();

            // Adiciona o objeto ProdutoDTO serializado em JSON
            var produtoJson = JsonSerializer.Serialize(produto);
            form.Add(new StringContent(produtoJson, null, "application/json"), "ProdutoDTO");

            // Adiciona os arquivos
            foreach (var file in files)
            {
                using var stream = file.OpenReadStream();
                var fileContent = new StreamContent(stream);

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "files",
                    FileName = file.FileName
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                form.Add(fileContent, "files");
            }

            var response = await _http.PostAsync("api/Produtos", form);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProdutoDTO>()!;
        }

        // Método de edição atualizado para aceitar o produto e novas/antigas imagens (para sua API)
        public async Task EditarProduto(ProdutoDTO produto, IFormFileCollection newFiles)
        {
            using var form = new MultipartFormDataContent();

            // Adiciona o objeto ProdutoDTO serializado em JSON
            var produtoJson = JsonSerializer.Serialize(produto);
            form.Add(new StringContent(produtoJson, null, "application/json"), "ProdutoDTO");

            // Adiciona as novas imagens
            foreach (var file in newFiles)
            {
                using var stream = file.OpenReadStream();
                var fileContent = new StreamContent(stream);

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "newFiles", // Nome diferente para novas imagens
                    FileName = file.FileName
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                form.Add(fileContent, "newFiles");
            }

            // Sua API deve ser capaz de processar os dados do produto (incluindo imagens existentes)
            // e as novas imagens em uma única requisição PUT
            var response = await _http.PutAsync($"api/Produtos/{produto.ProdutoId}", form);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoverProduto(Guid produtoId)
        {
            var response = await _http.DeleteAsync($"api/Produtos/{produtoId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> EnviarProdutoComArquivos(
        Guid? produtoId,
        MultipartFormDataContent content,
        bool isUpdate)
        {
            if (isUpdate)
            {
                if (!produtoId.HasValue || produtoId.Value == Guid.Empty)
                {
                    throw new ArgumentException("ID do produto é obrigatório para atualização.");
                }
                // Requisição PUT para edição
                return await _http.PutAsync($"api/Produtos/{produtoId.Value}", content);
            }
            else
            {
                // Requisição POST para criação
                return await _http.PostAsync("api/Produtos", content);
            }
        }

        #endregion

    }
}