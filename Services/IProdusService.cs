using MyApi.Models;
namespace MyApi.Services
{
    public interface IProdusService
    {
        List<Produs> ObtineToateProdusele();
        Produs? ObtineProdus(int id);
        Produs AdaugaProdus(Produs produs);
        public Produs? ActualizeazaProdus(int id, Produs produsActualizat);
        public bool StergeProdus(int id);
    }
}
