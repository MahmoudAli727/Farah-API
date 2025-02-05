using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Core.Enums;


namespace Infrastructure.Repositories
{
    public class BeautyRepository : Repository<BeautyCenter>, IBeautyRepository
    {
        private readonly ApplicationDBContext context;

        public BeautyRepository(ApplicationDBContext _context) : base(_context)
        {
            context = _context;
        }

        public IQueryable<BeautyCenter> GetAllBeautyCenters()
        {
            return context.BeautyCenters
                          .Include(b => b.ImagesBeautyCenter)
                          .Include(b => b.ServicesForBeautyCenter)
                          .Include(b => b.Reviews)
                          .Where(b=>b.ServiceStatus == ServiceStatus.Accepted) ;
                         
            
        }

        public BeautyCenter GetServiceById(int id) 
        {
            return context.BeautyCenters
                .Include(b => b.ImagesBeautyCenter)
                .Include(b => b.ServicesForBeautyCenter)
                .Include(b => b.FavoriteServices)
                .Where(b => b.ID == id)
                .FirstOrDefault();

        }

        public List<BeautyCenter> GetBeautyCenterByName(string name)
        {
            return context.BeautyCenters
                          .Where(b => b.Name.Contains(name))
                          .Include(b => b.ServicesForBeautyCenter)
                          .Include(b => b.ImagesBeautyCenter)
                          .Include(b => b.Reviews)
                          .ToList();
        }

        public BeautyCenter GetBeautyCenterById(int id)
        {
            return context.BeautyCenters
                .Include(b => b.ImagesBeautyCenter)
                .Include(b => b.ServicesForBeautyCenter)
                .Include(b => b.Reviews)
                .SingleOrDefault(b => b.ID == id);
        }

        public List<BeautyCenter> GetOwnerServices(string ownerID)
        {
            return context
                    .BeautyCenters
                    .Where(c => c.OwnerID == ownerID)
                    .Include(c => c.Reviews)
                    .ToList();
        }

        public void InsertService(ServiceForBeautyCenter service)
        {
            context.Add(service);
        }

        public void RemoveService(ServiceForBeautyCenter service)
        {
            context.Remove(service);
        }

        public ServiceForBeautyCenter GetBeautyService(int beautyID, int serviceID)
        {
            return context.servicesForBeautyCenter
                .Where(sb => sb.BeautyCenterId == beautyID && sb.Id == serviceID)
                .FirstOrDefault();
        }

        public  List<string> getAllImages(int id)
        {
            return context.BeautyCenterPicture
            .Where(s => s.BeautyCenterId == id)
            .Select(s => s.ImageUrl)
            .ToList();
        }
    }
}
