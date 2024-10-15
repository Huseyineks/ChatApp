using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Abstract
{
    public interface IUserGroupService : IBaseService<AppUserGroup>
    {

        public Group GetGroup(Guid guid);

        public List<AppUser> GetUsers(Guid guid);

        public List<Group> GetGroups(int id);
    }
}
