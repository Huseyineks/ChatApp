using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.DataAccesLayer.Abstract;
using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Concrete
{
    public class MessageService : BaseService<Message>, IMessageService
    {
        private readonly IBaseRepository<Message> _repository;
        private readonly IMessageRepository _messageRepository;
        public MessageService(IBaseRepository<Message> repository, IMessageRepository messageRepository) : base(repository)
        {
            _messageRepository = messageRepository;
        }

        public List<Message> GetSortedData()
        {
            var sortedData = _messageRepository.GetSortedData();

            return sortedData;
        }

        public List<Message> GetSortedList(Func<Message, bool> filter)
        {
           return _messageRepository.GetSortedList(filter);
        }

        public Message GetMessageWithAuthor(Func<Message, bool> filter)
        {

            return _messageRepository.GetMessageWithAuthor(filter);
        }

        public int? MaxValueOfGroupMessageId()
        {
            return _messageRepository.MaxValueOfGroupMessageId();
        }
    }
}
