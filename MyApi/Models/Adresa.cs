namespace MyApi.Models
{
    public class Adresa
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string NumeDestinatar { get; set; } = string.Empty;
        public string Strada { get; set; } = string.Empty;
        public string Oras { get; set; } = string.Empty;
        public string Judet { get; set; } = string.Empty;
        public string CodPostal { get; set; } = string.Empty;
        public string Tara { get; set; } = string.Empty;
        public string NumarTelefon { get; set; } = string.Empty;
        public bool EstePrincipala { get; set; }
        public User User {  get; set; } = null!;
    }
}
