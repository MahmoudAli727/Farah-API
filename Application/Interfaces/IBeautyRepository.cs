using Core.Entities;

namespace Application.Interfaces
{
    public interface IBeautyRepository : IRepository<BeautyCenter>
    {
        public List<BeautyCenter>? GetBeautyCenterByName(string name);
        public IQueryable<BeautyCenter> GetAllBeautyCenters();

        public void InsertService(ServiceForBeautyCenter service);
        public List<BeautyCenter> GetOwnerServices(string ownerID);
        public List<string> getAllImages(int id);
        public void RemoveService(ServiceForBeautyCenter service);
        public ServiceForBeautyCenter GetBeautyService(int beautyID, int serviceID);
        public BeautyCenter GetServiceById(int id);
    }
}
