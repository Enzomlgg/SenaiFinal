namespace APIEletrica.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Tipo { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }

        // Metadata
        public DateTime DataCriacao { get; set; }
    }
}
