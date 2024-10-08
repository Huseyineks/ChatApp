using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.DataAccesLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Concrete
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseRepository<T> _repository;
        public BaseService(IBaseRepository<T> repository) { 
        
            _repository = repository;
        
        }
        public void Add(T entity)
        {
            _repository.Add(entity);
        }

        public void Remove(T entity)
        {
            _repository.Remove(entity);
        }

        public void Update(T entity)
        {
           _repository.Update(entity);
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }
        public void Save()
        {
            _repository.Save();
        }

        public List<T> GetList(Expression<Func<T, bool>> filter)
        {
            return _repository.GetList(filter);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return _repository.Get(filter);
        }
    }
}
