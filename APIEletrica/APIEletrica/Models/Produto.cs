namespace APIEletrica.Models
{
    public class Produto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagens { get; set; }
        public int Estoque { get; set; }
        public int EstoqueMinimo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
