using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer GetCustomerById(string id);
    }
}
