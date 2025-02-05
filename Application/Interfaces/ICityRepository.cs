using Core.Entities;

namespace Application.Interfaces
{
    public interface ICityRepository
    {
        public List<City> GetAll(int governorateI);
        public City GetById(int Id);
    }
}
