using Application.DTOS;
using Application.Helpers;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFavoriteServiceLayer
    {
       public CustomResponseDTO<AllServicesDTO> GetAll(string CustomerID);
        public bool ToggleFavorite(int serviceID, string CustomerID);
        public void AddServiceToFav(int serviceID, string CustomerID);

        void RemoveServiceFromFav(int serviceId, string customerId);
    }
}
