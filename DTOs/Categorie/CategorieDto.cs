namespace MyApi.DTOs.Categorie
{
    public class CategorieDto
    {
        public int Id { get; set; }
        public string? Nume { get; set; }
        public byte[] RowVersion { get; set; } = [];
    }
}
