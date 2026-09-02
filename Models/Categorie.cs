namespace MyApi.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string? Nume { get; set; }

        public ICollection<Produs> Produse { get; set; } = [];
        public byte[] RowVersion { get; set; } = [];
    }
}
