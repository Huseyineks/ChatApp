using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.DataAccesLayer.Abstract;
using ChatApp.DataAccesLayer.Concrete;
using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Concrete
{
    public class OnlineUsersService : BaseService<OnlineAppUsers>,IOnlineUsersService
    {
        public OnlineUsersService(IBaseRepository<OnlineAppUsers> db) : base(db) { }
    }
}
