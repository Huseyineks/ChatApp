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
    public class GroupService : BaseService<Group>,IGroupService
    {
        

        public GroupService(IBaseRepository<Group> repository) : base(repository) { }
       
    }
}
