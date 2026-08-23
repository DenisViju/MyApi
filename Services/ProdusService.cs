using MyApi.Models;
namespace MyApi.Services
{
    public class ProdusService : IProdusService
    {
        public List<Produs> produse = new List<Produs> 
        {new Produs(1, "Laptop", 2000),
         new Produs(2, "Televizor", 2200),
         new Produs(3, "Frigider", 1800)
        };
        public List<Produs> ObtineToateProdusele()
        {
            return produse;
        }
        public Produs? ObtineProdus(int id)
        {
            return produse.FirstOrDefault(x => x.Id == id);
        }

        public Produs AdaugaProdus(Produs produs)
        {
            
            produs.Id = produse.Max(p => p.Id) + 1;

            produse.Add(produs);
            return produs;
        }

        public Produs? ActualizeazaProdus(int id, Produs produsActualizat) 
        {
            Produs? produs = produse.FirstOrDefault(p => p.Id == id);
            if (produs == null)
            {
                return null;
            }
            produs.Nume = produsActualizat.Nume;
            produs.Pret = produsActualizat.Pret;
            return produs;
        }

        public bool StergeProdus(int id)
        {
            Produs? produs = produse.FirstOrDefault(p =>p.Id == id);
            if(produs == null)
            {
                return false;
            }
            produse.Remove(produs);
            return true;

        }
    }
}
