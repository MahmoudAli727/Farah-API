using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface Iservices<T> where T : class
    {
        public List<T> GetAll();
        public T GetById(int id);
        public T GetById(string id);
        public void Insert(T obj);
        public void Update(T obj);
        public void Delete(int id);
        public void Save();
    }
}

