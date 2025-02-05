using Application.Helpers;
using Core.Entities;
using Core.Enums;

namespace Application.Interfaces
{
    public interface IAdminRepository : IRepository<Owner>
    {
        public List<Service> GetAllServices(ServiceStatus ServiceStatus);
        public IQueryable<Owner> GetAllOwners();
        public IQueryable<Customer> GetAllCustomers(string? customerName,bool? isBlocked);

        public Owner GetOwnerById(string id);
        public Customer GetCustomerById(string id);
        public IQueryable<Owner> GetOwnersByStatus(string? ownerName,UserType? userType, OwnerAccountStatus? status, bool? isBlocked);
        public List<ApplicationUser> SearchUsersByName(string name);
        public Service GetServiceById(int id);
        public void updateServices(List<Service> obj);
        public void updateOwners(List<Owner> obj);
    }
}
