using Application.Helpers;
using Application.Interfaces;
using Application.Services;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FavoriteRepository : Repository<FavoriteService>, IFavoriteRepository
    {
        private readonly ApplicationDBContext context;

        public FavoriteRepository(ApplicationDBContext _context) : base(_context)
        {
            context = _context;
        }

        public List<FavoriteService> GetAllFavoritesForCustomer(string customerId)
        {
            return context.FavoriteService
                .Include(f => f.Service)
                    .ThenInclude(s => (s as Hall).Pictures)
                .Include(f => f.Service)
                    .ThenInclude(s => (s as Car).Pictures)
                .Include(f => f.Service)
                    .ThenInclude(s => (s as BeautyCenter).ImagesBeautyCenter)
                .Include(f => f.Service)
                    .ThenInclude(s => (s as Photography).Images)
                .Where(f => f.CustomerId == customerId)
                .ToList();
        }

        public FavoriteService GetFavService(int serviceID, string CustomerID)
        {
            return context
                .FavoriteService
                .FirstOrDefault(f => f.ServiceId == serviceID && f.CustomerId == CustomerID);
        }
    }
}
