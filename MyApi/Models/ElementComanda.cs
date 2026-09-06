namespace MyApi.Models
{
    public class ElementComanda
    {
        public int Id { get; set; }
        public int ComandaId { get; set; }
        public Comanda Comanda { get; set; } = null!;

        public int ProdusId { get; set; }
        public Produs Produs { get; set; } = null!;

        public int Cantitate {  get; set; }
        public decimal PretUnitar { get; set; }
    }
}
