using Application.Interfaces;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDBContext _context;

        public CustomerRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public Customer GetCustomerById(string id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }
    }
}
