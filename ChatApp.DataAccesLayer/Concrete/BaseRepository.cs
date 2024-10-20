using ChatApp.DataAccesLayer.Abstract;
using ChatApp.DataAccesLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccesLayer.Concrete
{
    
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;
        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public List<T> GetList(Expression<Func<T, bool>> filter)
        {
            List<T> list = _dbSet.Where(filter).ToList();

            return list;
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            T entity = _dbSet.FirstOrDefault(filter);

            return entity;
        }
    }
}
