using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccesLayer.Abstract
{
    public interface IBaseRepository<T> where T : class
    {
        public void Add(T entity);

        public void Update(T entity);

        public void Remove(T entity);

        public List<T> GetAll();
        public void Save();

       
    }
}
