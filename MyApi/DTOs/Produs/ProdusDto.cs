namespace MyApi.DTOs.Produs
{
    public class ProdusDto
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;
        public string Descriere { get; set; } = string.Empty;
        public decimal Pret { get; set; }
        public int Stoc {  get; set; }
        public int CategorieId {  get; set; }
        public string? NumeCategorie { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}
