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
    public class UserGroupService : BaseService<AppUserGroup>, IUserGroupService
    {

        public UserGroupService(IBaseRepository<AppUserGroup> repository) : base(repository) { }
    }
}
