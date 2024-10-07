using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccesLayer.Abstract
{
    public interface IUserGroupRepository : IBaseRepository<AppUserGroup>
    {
        public Group GetGroup(Guid guid);

        public List<AppUser> GetUsers(Guid guid);
    }
}
