namespace MyApi.Models
{
    public class Produs
    {
        public int Id { get; set; }
        public string? Nume { get; set; }
        public double Pret { get; set; }

        public int CategorieId { get; set; }
        public Categorie? Categorie { get; set; }

        public Produs() { }
        public Produs(int id, string nume, double pret, int categorieId)
        {
            Id = id;
            Nume = nume;
            Pret = pret;
            CategorieId = categorieId;
        }
    }
}
