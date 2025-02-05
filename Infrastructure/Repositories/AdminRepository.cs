using Application.Helpers;
using Application.Interfaces;
using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace Infrastructure.Repositories
{
    public class AdminRepository : Repository<Owner>, IAdminRepository
    {
        private readonly ApplicationDBContext context;

        public AdminRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }
        public IQueryable<Owner> GetAllOwners()
        {

            return context.Owners
                .Include(o => o.Services);


        }
        public IQueryable<Customer> GetAllCustomers(string? customerName, bool? isBlocked)
        {
            var query = context.Customers.AsQueryable();
            if (customerName != null)
            {
                query = query.Where(o => o.FirstName.Contains(customerName) || o.LastName.Contains(customerName));
            }


            if (isBlocked.HasValue)
            {
                query = query.Where(o => o.IsBlocked == isBlocked.Value);
            }

            return query;
        }
        public Owner GetOwnerById(string id)
        {
            return context.Owners
                .Include(o => o.Services)
                .FirstOrDefault(o => o.Id == id);

        }



        public List<Service> GetAllServices(ServiceStatus ServiceStatus)
        {
            var beautyCenterServices = context.Services
                .OfType<BeautyCenter>()
                .Where(b => b.ServiceStatus == ServiceStatus)
                .Include(b => b.ImagesBeautyCenter)
                .Include(s => s.ServicesForBeautyCenter)
                .ToList();

            var shopDressesServices = context.Services
                .OfType<ShopDresses>()
                .Where(b => b.ServiceStatus == ServiceStatus)
                .Include(s => s.Dresses)
                .ToList();

            var hallServices = context.Services
                .OfType<Hall>()
                .Where(b => b.ServiceStatus == ServiceStatus)
                .Include(h => h.Features)
                .Include(h => h.Pictures)
                .ToList();

            var carServices = context.Services
                .OfType<Car>()
                .Where(b => b.ServiceStatus == ServiceStatus)
                .Include(c => c.Pictures)
                .ToList();

            var photographerServices = context.Services
                .OfType<Photography>()
                .Where(b => b.ServiceStatus == ServiceStatus)
                .Include(c => c.Images)
                .ToList();

            // Combine all service types
            var allServices = new List<Service>();
            allServices.AddRange(beautyCenterServices);
            allServices.AddRange(shopDressesServices);
            allServices.AddRange(hallServices);
            allServices.AddRange(carServices);
            allServices.AddRange(photographerServices);

            allServices = allServices.OrderBy(s => s.CreatedAt)
                                     .ThenBy(s => !s.IsAdminSeen)
                                        .ToList();

            context.SaveChanges();
            return allServices;
        }

        public void updateServices(List<Service> obj)
        {
            foreach (var service in obj)
            {
                service.IsAdminSeen = true;
                context.Update(service);
            }
        }
        public void updateOwners(List<Owner> owners)
        {
            foreach (var owner in owners)
            {
                owner.IsAdminSeen = true;
                context.Update(owner);
            }
        }
        public Service GetServiceById(int id)
        {
            return context.Services
                .Include(s => (s as Hall).Pictures)
                .Include(s => (s as Hall).Features)
                .Include(s => (s as Car).Pictures)
                .Include(s => (s as BeautyCenter).ImagesBeautyCenter)
                .Include(s => (s as BeautyCenter).ServicesForBeautyCenter)
                .Include(s => (s as Photography).Images)
                .FirstOrDefault(s => s.ID == id);
        }
        public IQueryable<Owner> GetOwnersByStatus(string? ownerName, UserType? userType, OwnerAccountStatus? status, bool? isBlocked)
        {
            var query = context.Owners.AsQueryable();

            if(ownerName != null)
            {
                query = query.Where(o => o.FirstName.Contains(ownerName) || o.LastName.Contains(ownerName));
            }
            if(userType.HasValue)
            {
                query = query.Where(o => o.UserType == userType);
            }
            if (status.HasValue)
            {
                query = query.Where(o => o.AccountStatus == status.Value);
            }

            if (isBlocked.HasValue)
            {
                query = query.Where(o => o.IsBlocked == isBlocked.Value);
            }

            return query;
        }


        public List<ApplicationUser> SearchUsersByName(string name)
        {
            return context.Users
                           .Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name))
                           .ToList();
        }

        public Customer GetCustomerById(string id)
        {
            return context.Customers
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
