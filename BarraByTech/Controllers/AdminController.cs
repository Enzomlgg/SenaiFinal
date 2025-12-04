using BarraByTech.Models;
using BarraByTech.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using BarraByTech.Models.DTO;

namespace BarraByTech.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ClienteService _clienteService;
        private readonly AdminService _adminService;

        public AdminController(ClienteService clienteService, AdminService adminService)
        {
            _clienteService = clienteService;
            _adminService = adminService;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new ProdutoViewModel();

            try
            {
                var listaCategorias = await _clienteService.GetCategorias();
                var listaMarcas = await _clienteService.GetMarcas();
                var listaTiposMarca = await _clienteService.GetTodosTiposMarca();
                viewModel.Categorias = listaCategorias ?? new List<CategoriaDTO>();
                viewModel.Marcas = listaMarcas ?? new List<MarcaDTO>();
                viewModel.TiposMarca = listaTiposMarca ?? new List<TiposMarcaDTO>();            }
            catch (HttpRequestException ex)
            {
                TempData["Erro"] = $"Falha ao conectar com o serviço de dados (API). Verifique a conexão e o endereço da BaseAddress. Detalhes: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro interno ao carregar a página: {ex.Message}";
            }

            return View(viewModel);
        }

        #region Endpoints de Leitura de Dados para JS

        [HttpGet]
        public async Task<IActionResult> GetMarcas()
        {
            var marcas = await _clienteService.GetMarcas();
            return Json(marcas);
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutosData()
        {
            try
            {
                var produtos = await _adminService.GetProdutos();
                return Json(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Falha ao buscar produtos da API.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutoCompleto(Guid produtoId)
        {
            try
            {
                var produto = await _adminService.GetProdutoById(produtoId);
                return Json(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Falha ao buscar detalhes do produto.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTiposMarca(Guid marcaId)
        {
            if (marcaId == Guid.Empty)
            {
                return Json(new List<TiposMarcaDTO>());
            }

            try
            {
                var tipos = await _clienteService.GetTiposMarcaPorMarcaId(marcaId);
                return Json(tipos);
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, new { error = "Falha de comunicação com a API de Tipos de Marca." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasCategorias()
        {
            var categorias = await _clienteService.GetCategorias();
            return Json(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodosTiposMarca()
        {
            var tipos = await _clienteService.GetTodosTiposMarca();
            return Json(tipos);
        }

        #endregion

        #region Funções Admin - Produtos CRUD

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarProduto(ProdutoAdminFormDTO model)
        {
            try
            {
                var produtoData = new ProdutoTransferDTO
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Boleto = model.Boleto,
                    Cartao = model.Cartao,
                    Vista = model.Vista,
                    Promocao = model.Promocao,
                    CategoriaId = model.CategoriaId,
                    MarcaId = model.MarcaId,
                    TipoMarcaId = model.TipoMarcaId
                };

                // 2. Serializar APENAS os dados de texto
                var produtoJson = JsonSerializer.Serialize(produtoData);
                var content = new MultipartFormDataContent();

                // Chave deve ser "ProdutoDTO" para a API
                content.Add(new StringContent(produtoJson, Encoding.UTF8, "application/json"), "ProdutoDTO");

                // CHAMA O MÉTODO ENCAPSULADO NO SERVICE
                var response = await _adminService.EnviarProdutoComArquivos(null, content, false);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Produto criado com sucesso!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Erro"] = $"Erro ao criar produto: {response.StatusCode}. Detalhes: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro inesperado: {ex.Message}";
            }

            return RedirectToAction("Index", "Admin", new { tab = "tab-produtos" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProduto(ProdutoAdminFormDTO model)
        {
            if (model.ProdutoId == null || model.ProdutoId == Guid.Empty)
            {
                TempData["Erro"] = "ID do produto é obrigatório para edição.";
                return RedirectToAction("Index", "Admin", new { tab = "tab-produtos" });
            }

            try
            {
                var produtoData = new ProdutoTransferDTO
                {
                    ProdutoId = model.ProdutoId,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Boleto = model.Boleto,
                    Cartao = model.Cartao,
                    Vista = model.Vista,
                    Promocao = model.Promocao,
                    CategoriaId = model.CategoriaId,
                    MarcaId = model.MarcaId,
                    TipoMarcaId = model.TipoMarcaId,

                };

                // 2. Serializar APENAS os dados de texto
                var produtoJson = JsonSerializer.Serialize(produtoData);
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(produtoJson, Encoding.UTF8, "application/json"), "ProdutoDTO");

                var response = await _adminService.EnviarProdutoComArquivos(model.ProdutoId, content, true);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Produto atualizado com sucesso!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Erro"] = $"Erro ao atualizar produto: {response.StatusCode}. Detalhes: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro inesperado: {ex.Message}";
            }
            return RedirectToAction("Index", "Admin", new { tab = "tab-produtos" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverProduto(Guid produtoId)
        {
            try
            {
                await _adminService.RemoverProduto(produtoId);
                TempData["Sucesso"] = "Produto removido com sucesso!";
                return RedirectToAction("Index", new { tab = "tab-produtos" });
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao remover produto: {ex.Message}";
                return RedirectToAction("Index", new { tab = "tab-produtos" });
            }
        }

        #endregion

        #region Funções Admin - Categorias CRUD

        [HttpPost]
        public async Task<IActionResult> CriarCategoria(CategoriaDTO categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.CategoriaNome))
            {
                TempData["Erro"] = "O nome da categoria é obrigatório!";
                return RedirectToAction("Index", new { tab = "tab-categorias" });
            }

            try
            {
                await _adminService.CriarCategoria(categoria);
                TempData["Sucesso"] = "Categoria criada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao criar categoria: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-categorias" });
        }

        [HttpPost]
        public async Task<IActionResult> EditarCategoria(CategoriaDTO categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.CategoriaNome))
            {
                TempData["Erro"] = "O nome da categoria não pode ficar vazio!";
                return RedirectToAction("Index", new { tab = "tab-categorias" });
            }

            try
            {
                await _adminService.EditarCategoria(categoria);
                TempData["Sucesso"] = "Categoria editada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao editar categoria: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-categorias" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverCategoria(Guid categoriaId)
        {
            try
            {
                await _adminService.RemoverCategoria(categoriaId);
                TempData["Sucesso"] = "Categoria removida com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao remover categoria: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-categorias" });
        }
        #endregion

        #region Funções Admin - Marcas CRUD
        [HttpPost]
        public async Task<IActionResult> CriarMarca(MarcaDTO marca)
        {
            if (string.IsNullOrWhiteSpace(marca.Nome))
            {
                TempData["Erro"] = "O nome da marca é obrigatório!";
                return RedirectToAction("Index", new { tab = "tab-marcas" });
            }

            try
            {
                await _adminService.CriarMarca(marca);
                TempData["Sucesso"] = "Marca criada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao criar marca: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-marcas" });
        }

        [HttpPost]
        public async Task<IActionResult> EditarMarca(MarcaDTO marca)
        {
            if (string.IsNullOrWhiteSpace(marca.Nome))
            {
                TempData["Erro"] = "O nome da marca não pode ficar vazio!";
                return RedirectToAction("Index", new { tab = "tab-marcas" });
            }

            try
            {
                await _adminService.EditarMarca(marca);
                TempData["Sucesso"] = "Marca editada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao editar marca: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-marcas" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverMarca(Guid marcaId)
        {
            try
            {
                await _adminService.RemoverMarca(marcaId);
                TempData["Sucesso"] = "Marca removida com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao remover marca: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-marcas" });
        }
        #endregion

        #region Funções Admin - Tipos de Marca CRUD
        [HttpPost]
        public async Task<IActionResult> CriarTipoMarca(TiposMarcaDTO tipoMarca)
        {
            if (string.IsNullOrWhiteSpace(tipoMarca.Nome) || tipoMarca.MarcaId == Guid.Empty)
            {
                TempData["Erro"] = "Nome do Tipo e Marca são obrigatórios!";
                return RedirectToAction("Index", new { tab = "tab-marcas" });
            }

            try
            {
                await _adminService.CriarTipoMarca(tipoMarca);
                TempData["Sucesso"] = "Tipo de Marca criado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao criar Tipo de Marca: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-marcas" });
        }

        [HttpPost]
        public async Task<IActionResult> EditarTipoMarca(TiposMarcaDTO tipoMarca)
        {
            if (string.IsNullOrWhiteSpace(tipoMarca.Nome) || tipoMarca.MarcaId == Guid.Empty)
            {
                TempData["Erro"] = "Nome do Tipo e Marca são obrigatórios!";
                return RedirectToAction("Index", new { tab = "tab-marcas" });
            }

            try
            {
                await _adminService.EditarTipoMarca(tipoMarca);
                TempData["Sucesso"] = "Tipo de Marca editado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao editar Tipo de Marca: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-marcas" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverTipoMarca(Guid tipoMarcaId)
        {
            try
            {
                await _adminService.RemoverTipoMarca(tipoMarcaId);
                TempData["Sucesso"] = "Tipo de Marca removido com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao remover Tipo de Marca: {ex.Message}";
            }

            return RedirectToAction("Index", new { tab = "tab-marcas" });
        }
        #endregion
    }
}