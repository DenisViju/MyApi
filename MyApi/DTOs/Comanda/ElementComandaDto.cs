namespace MyApi.DTOs.Comanda
{
    public class ElementComandaDto
    {
        public int Id { get; set; }
        public int ProdusId { get; set; }
        public string NumeProdus { get; set; } = string.Empty;
        public int Cantitate { get; set; }
        public decimal PretUnitar { get; set; }
    }
}
