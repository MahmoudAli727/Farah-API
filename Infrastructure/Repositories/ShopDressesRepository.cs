using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Enums;

namespace Infrastructure.Repositories
{
    public class ShopDressesRepository : Repository<ShopDresses>, IShopDressesRepository
    {

        ApplicationDBContext context;
        public ShopDressesRepository(ApplicationDBContext _context) : base(_context)
        {
            context = _context;
        }
        public IQueryable<ShopDresses> GetAllShopDresses()
        {
            return context.ShopDresses
                          .Include(b => b.Dresses)
                          .Where(b => b.ServiceStatus == ServiceStatus.Accepted);
        }

        public List<ShopDresses> GetOwnerServices(string ownerID)
        {
            return context
                    .ShopDresses
                    .Where(c => c.OwnerID == ownerID)
                    .Include(c => c.Dresses)
                    .ToList();
        }

        public List<ShopDresses>? GetShopDressesByName(string name)
        {
            return context.ShopDresses
                         .Where(b => b.ShopName.Contains(name))
                         .Include(b => b.Dresses)
                         .ToList();
        }

    }
}
