using Application.DTOS;
using Core.Entities;

namespace Application.Interfaces
{
    public interface IHallService
    {
        public CustomResponseDTO<List<HallDTO>> GetAllHalls(string customerId,int page, int pageSize, string priceRange,int govId,int cityId);
        public Task<HallDTO> AddHall(AddHallDTO HallDto);
        public CustomResponseDTO<HallDTO> GetHallById(int id);
        public Task<HallDTO> EditHall(int id, HallDTO hallDto);
        public void Delete(int id);
    }
}
