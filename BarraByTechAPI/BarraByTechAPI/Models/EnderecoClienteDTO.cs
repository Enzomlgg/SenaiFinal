namespace BarraByTechAPI.Models
{
    public class EnderecoClienteDTO
    {
        public Guid? EnderecoId { get; set; }
        public Guid UserId { get; set; }
        public int Cep { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
    }
}
