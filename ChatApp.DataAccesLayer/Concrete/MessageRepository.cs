using ChatApp.DataAccesLayer.Abstract;
using ChatApp.DataAccesLayer.Data;
using ChatApp.EntitiesLayer.Model;
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

        private static int _id = 1;
        public MessageRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
           

        }

        public List<Message> GetSortedData()
        {
            var sortedData = _db.Messages.OrderBy(i => i.createdAt).ToList();

            return sortedData;
        }

        public List<Message> GetSortedList(Func<Message, bool> filter)
        {
            var sortedData = GetSortedData();
            var sortedList = sortedData.Where(filter).ToList();


            return sortedList;
        }

       public int SetNextCommonId()
        {


            _id++;

            return _id;
        }

    public int GetCommonId()
        {
            return _id;
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