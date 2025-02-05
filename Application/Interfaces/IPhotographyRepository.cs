using Core.Entities;

namespace Application.Interfaces
{
    public interface IPhotographyRepository : IRepository<Photography>
    {
        public List<Photography> GetOwnerServices(string ownerID);
        public new IQueryable<Photography> GetAll();

        public List<string> getAllImages(int id);
    }
}
