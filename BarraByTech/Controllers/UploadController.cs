using BarraByTech.Models;
using BarraByTech.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO; // Necessário para FileInfo e Directory

public class UploadController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly ClienteService _clienteService;

    public UploadController(IWebHostEnvironment env, ClienteService clienteService)
    {
        _env = env;
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        var categorias = await _clienteService.GetCategorias();
        return View(categorias);
    }

    [HttpGet]
    public async Task<JsonResult> GetProdutos(Guid categoriaId)
    {
        var produtos = await _clienteService.GetProdutoCategoria(categoriaId);
        // Retorna ProdutoId e Nome (necessário para o JS)
        return Json(produtos.Select(p => new { id = p.ProdutoId, nome = p.Nome }));
    }

    [HttpPost]
    public async Task<IActionResult> EnviarImagem(Guid produtoId, IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            TempData["Erro"] = "Nenhuma imagem enviada!";
            return RedirectToAction("Index");
        }

        string pastaProduto = Path.Combine(_env.WebRootPath, "Imagens", "Produtos", produtoId.ToString());
        if (!Directory.Exists(pastaProduto))
            Directory.CreateDirectory(pastaProduto);

        // Adiciona um timestamp para evitar colisões e garantir unicidade (além do Guid)
        string nomeArquivo = Guid.NewGuid() + Path.GetExtension(arquivo.FileName);
        string caminhoFinal = Path.Combine(pastaProduto, nomeArquivo);

        using (var stream = new FileStream(caminhoFinal, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        TempData["UrlImagem"] = $"/Imagens/Produtos/{produtoId}/{nomeArquivo}";
        TempData["ProdutoId"] = produtoId;
        TempData["Sucesso"] = $"Imagem '{nomeArquivo}' enviada com sucesso!";

        return RedirectToAction("Index");
    }

    // NOVO MÉTODO: Listar caminhos de imagens de um produto
    [HttpGet]
    public JsonResult GetImagens(Guid produtoId)
    {
        if (produtoId == Guid.Empty)
        {
            return Json(new List<string>());
        }

        string pastaProduto = Path.Combine(_env.WebRootPath, "Imagens", "Produtos", produtoId.ToString());

        if (!Directory.Exists(pastaProduto))
        {
            return Json(new List<string>()); // Retorna lista vazia se a pasta não existir
        }

        // Obtém todos os arquivos JPG, JPEG, PNG e GIF na pasta
        var arquivos = Directory.GetFiles(pastaProduto)
            .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                           file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                           file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                           file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            .ToList();

        // Converte o caminho físico para o caminho URL relativo
        var caminhosUrl = arquivos.Select(filePath =>
        {
            var relativePath = filePath.Substring(_env.WebRootPath.Length).Replace('\\', '/');
            return new
            {
                caminho = relativePath,
                nomeArquivo = Path.GetFileName(filePath)
            };
        }).ToList();

        return Json(caminhosUrl);
    }

    // NOVO MÉTODO: Excluir imagem
    [HttpPost]
    public IActionResult ExcluirImagem([FromBody] ExcluirImagemRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.Caminho))
        {
            return BadRequest("Caminho da imagem é obrigatório.");
        }

        // Converte o caminho URL (ex: /Imagens/Produtos/GUID/nome.jpg) para caminho físico
        string caminhoFisico = Path.Combine(_env.WebRootPath, model.Caminho.TrimStart('/'));

        if (!System.IO.File.Exists(caminhoFisico))
        {
            return NotFound("Arquivo não encontrado no servidor.");
        }

        try
        {
            System.IO.File.Delete(caminhoFisico);
            // Opcional: Adicionar lógica para remover do BD se estivesse lá
            return Ok(new { message = "Imagem excluída com sucesso." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao excluir o arquivo: {ex.Message}");
        }
    }
}

public class ExcluirImagemRequest
{
    public string Caminho { get; set; }
    // O produtoId não é estritamente necessário para a exclusão, mas é bom para logs
    public Guid ProdutoId { get; set; }
}