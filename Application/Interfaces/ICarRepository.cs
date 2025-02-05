using Core.Entities;

namespace Application.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        public List<Car> GetOwnerServices(string ownerID);
        public new IQueryable<Car> GetAll();
        public List<string> getAllImages(int id);
        public Car GetCarById(int id);
    }
}
