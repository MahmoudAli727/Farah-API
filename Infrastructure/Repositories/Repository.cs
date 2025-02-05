using Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ApplicationDBContext context;
        public Repository(ApplicationDBContext _context)
        {
            context = _context;
        }
        public void Delete(int id)
        {
            var obj = context.Set<T>().Find(id);
            if (obj != null)
            {
                context.Set<T>().Remove(obj);
            }
        }
        public List<T> GetAll()
        {

            return context.Set<T>().ToList();

        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public T GetById(string id)
        {
            return context.Set<T>().Find(id);
        }

        public void Insert(T obj)
        {
            context.Set<T>().Add(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(T obj)
        {
            context.Update(obj);
        }
    }
}
