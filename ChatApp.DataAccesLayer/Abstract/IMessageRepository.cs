using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccesLayer.Abstract
{
    public interface IMessageRepository : IBaseRepository<Message>
    {

        public List<Message> GetSortedData();
    
        public List<Message> GetSortedList(Func<Message, bool> filter);
    
    }

     
}
