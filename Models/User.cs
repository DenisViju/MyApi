namespace MyApi.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsDeleted { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
        public ICollection<Adresa> Adrese {get; set;} = [];
        public ICollection<Comanda> Comenzi {get; set;} = [];
    }
}
