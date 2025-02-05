using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Enums;

namespace Infrastructure.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly ApplicationDBContext context;

        public CarRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable<Car> GetAll()
        {
            return context.Cars
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Pictures)
                .Where(c => c.ServiceStatus == ServiceStatus.Accepted); 
        }

        public List<string> getAllImages(int id)
        {
            return context.CarPictures
                        .Where(s => s.CarID == id)
                        .Select(s => s.Url)
                        .ToList();
        }

        public Car GetCarById(int id)
        {
            return context.Cars
                .Include(c => c.Pictures)
                .Include(c => c.FavoriteServices)
                .FirstOrDefault(c => c.ID == id);
        }

        public List<Car> GetOwnerServices(string ownerID)
        {
            return context
                .Cars
                .Where(c => c.OwnerID == ownerID)
                .Include(c => c.Pictures)
                .Where(c => c.IsDeleted == false).ToList();
        }
    }
}
