namespace MyApi.Models
{
    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;
        public decimal Pret { get; set; }
        public int Stoc {  get; set; }

        public int CategorieId { get; set; }
        public Categorie? Categorie { get; set; }

        public ICollection<ElementComanda> ElementeComanda { get; set; } = [];
        public byte[] RowVersion { get; set; } = [];

    }
}
