using ChatApp.DataAccesLayer.Abstract;
using ChatApp.DataAccesLayer.Data;
using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var sortedData = _db.Messages.OrderBy(i => i.createdAt).ToList();

            return sortedData;
        }
    }
}