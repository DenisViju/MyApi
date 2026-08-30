namespace MyApi.DTOs
{
    public class ProdusDto
    {
        public int Id { get; set; }
        public string? Nume { get; set; }
        public double Pret { get; set; }
        public int CategorieId {  get; set; }
        public string? NumeCategorie { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}
