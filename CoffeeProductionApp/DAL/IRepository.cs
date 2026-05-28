using System.Data;

namespace CoffeeProductionApp.DAL
{
    public interface IRepository<T>
    {
        DataTable GetAll();
        T GetById(int id);
        int Add(T entity);
        int Update(T entity);
        int Delete(int id);
    }
}