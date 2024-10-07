using ChatApp.DataAccesLayer.Abstract;
using ChatApp.DataAccesLayer.Data;
using ChatApp.EntitiesLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccesLayer.Concrete
{
    public class UserGroupRepository : BaseRepository<AppUserGroup> , IUserGroupRepository
    {
        private readonly ApplicationDbContext _db;
        public UserGroupRepository(ApplicationDbContext db) : base(db){ 
            _db = db;
    
        }

        public Group GetGroup(Guid guid)
        {


            Group group = _db.UserGroups.Include(i  => i.Group).Where(i => i.Group.RowGuid == guid).Select(i => i.Group).FirstOrDefault();




            return group;




        }

        public List<AppUser> GetUsers(Guid guid)
        {

            List<AppUser> users = _db.UserGroups.Include(i => i.Group).Where(i => i.Group.RowGuid == guid).Select(i => i.User).ToList();






            return users;




        }
    }
}
