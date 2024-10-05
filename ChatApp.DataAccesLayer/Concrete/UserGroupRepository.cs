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
    public class UserGroupRepository : BaseRepository<AppUserGroup> , IUserGroupRepository
    {

        public UserGroupRepository(ApplicationDbContext db) : base(db){ }
    }
}
