namespace MyApi.DTOs.Produs
{
    public class ProdusDto
    {
        public int Id { get; set; }
        public string? Nume { get; set; }
        public decimal Pret { get; set; }
        public int CategorieId {  get; set; }
        public string? NumeCategorie { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}
