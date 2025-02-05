using Application.DTOS;

namespace Application.Interfaces
{
    public interface ICityServices
    {
        public List<CityDTO> GetAll(int governorateId);
        public CityDTO GetById(int Id);
    }
}
