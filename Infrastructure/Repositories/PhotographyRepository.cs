using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Enums;

namespace Infrastructure.Repositories
{
    public class PhotographyRepository : Repository<Photography>, IPhotographyRepository
    {
        private readonly ApplicationDBContext context;

        public PhotographyRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable<Photography> GetAll()
        {
            return context.Photographies
                .Include(c => c.Images)
                .Where(c => c.ServiceStatus == ServiceStatus.Accepted);
        }

        public List<string> getAllImages(int id)
        {
            return context.ImagePhotography
                        .Where(s => s.PhotographerId == id)
                        .Select(s => s.ImageURL)
                        .ToList();
        }

        public Photography GetById(int id)
        {
            return context.Photographies
                .Include(c => c.Images)
                .Include(c => c.FavoriteServices)
                .FirstOrDefault(c => c.ID == id);
        }

        public List<Photography> GetOwnerServices(string ownerID)
        {
            return context
                    .Photographies
                    .Where(c => c.OwnerID == ownerID)
                    .Include(c => c.Images)
                    .ToList();
        }
    }
}
