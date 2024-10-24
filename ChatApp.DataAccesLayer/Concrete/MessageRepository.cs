using ChatApp.DataAccesLayer.Abstract;
using ChatApp.DataAccesLayer.Data;
using ChatApp.EntitiesLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccesLayer.Concrete
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _db;

        
        public MessageRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
           

        }

        public List<Message> GetSortedData()
        {
            var sortedData = _db.Messages.Include(i => i.Author).OrderBy(i => i.createdAt).ToList();

            

            return sortedData;
        }

        public List<Message> GetSortedList(Func<Message, bool> filter)
        {
            var sortedData = GetSortedData();
            var sortedList = sortedData.Where(filter).ToList();


            return sortedList;
        }

        public Message GetMessageWithAuthor(Func<Message,bool> filter)
        {

            var message = _db.Messages.Include(i => i.Author).FirstOrDefault(filter);

            return message;
        }

    


    public int? MaxValueOfGroupMessageId()
        {
            var max = _db.Messages.Max(i => i.groupMessageId);
            

            if(max == null)
            {
                return 0;
            }

            return max;
        }
    }
}