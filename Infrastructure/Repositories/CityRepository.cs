using Application.Interfaces;
using Core.Entities;

namespace Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDBContext dbContext;

        public CityRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<City> GetAll(int governorateI)
        {
            return dbContext.Set<City>()
                .Where(c => c.GovernorateID == governorateI)
                .ToList();
        }

        public City GetById(int Id)
        {
            return dbContext.Set<City>()
                .FirstOrDefault(c => c.Id == Id);
        }
    }
}
