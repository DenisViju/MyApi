namespace MyApi.Services
{
    public class MesajService : IMesajService
    {
        public string ObtineMesaj(string nume)
        {
            return $"Salut, {nume}";
        }
    }
}
