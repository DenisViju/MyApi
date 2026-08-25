namespace MyApi.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string? Nume { get; set; }

        public List<Produs> Produse { get; set; } = new List<Produs>();
    }
}
