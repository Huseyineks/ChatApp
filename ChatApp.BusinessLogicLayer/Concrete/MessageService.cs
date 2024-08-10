using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.DataAccesLayer.Abstract;
using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Concrete
{
    public class MessageService : BaseService<Message>, IMessageService
    {
        private readonly IBaseRepository<Message> _repository;
        public MessageService(IBaseRepository<Message> repository) : base(repository) { 
        
            
        
        
        }
     
    }
}
