using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFavoriteRepository : IRepository<FavoriteService>
    {
        public List<FavoriteService> GetAllFavoritesForCustomer(string customerId);
        public FavoriteService GetFavService(int serviceID, string CustomerID);
    }
}
