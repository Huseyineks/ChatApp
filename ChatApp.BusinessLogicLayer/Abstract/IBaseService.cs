using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Abstract
{
    public interface IBaseService<T> where T : class
    {
        public void Add(T entity);

        public void Update(T entity);

        public void Remove(T entity);

        public List<T> GetAll();

        public List<T> GetList(Expression<Func<T, bool>> filter);

        public T Get(Expression<Func<T, bool>> filter);
        public void Save();
    }
}
