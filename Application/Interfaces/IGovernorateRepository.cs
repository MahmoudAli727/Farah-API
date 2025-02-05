using Core.Entities;

namespace Application.Interfaces
{
    public interface IGovernorateRepository
    {
        public List<Governorate> GetAll();
        public Governorate GetById(int governorateId);
    }
}
