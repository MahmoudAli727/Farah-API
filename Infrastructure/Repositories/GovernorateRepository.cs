using Application.Interfaces;
using Core.Entities;

namespace Infrastructure.Repositories
{
    public class GovernorateRepository : IGovernorateRepository
    {
        private readonly ApplicationDBContext dbContext;

        public GovernorateRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Governorate> GetAll()
        {
            return dbContext.Set<Governorate>()
                .ToList();
        }

        public Governorate GetById(int governorateId)
        {
            Governorate? governorate = dbContext.Set<Governorate>()
                .FirstOrDefault(g => g.GovernorateID == governorateId);
            return governorate;
        }
    }
}
