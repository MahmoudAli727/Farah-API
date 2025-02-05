using Application.DTOS;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICarService
    {
        public CustomResponseDTO<List<CarDTO>> GetAllCars(string customerId,int page, int pageSize, string priceRange, int govId, int cityId);
        public Task<CarDTO> AddCar(AddCarDTO carDto);
        public Task<CarDTO> EditCar(int id, CarDTO carDto);
        public void Delete(int id);

        public CustomResponseDTO<CarDTO> GetCarById(int id);

    }
}
