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
        private readonly IBaseRepository<AppUserGroup> _repository;

        private readonly IUserGroupRepository _userGroupRepository;

        public UserGroupService(IBaseRepository<AppUserGroup> repository,IUserGroupRepository userGroupRepository) : base(repository) {

            _repository = repository; 
            _userGroupRepository = userGroupRepository;
        
    }

        public Group GetGroup(Guid guid)
        {


            
            return _userGroupRepository.GetGroup(guid);




        }


        public List<AppUser> GetUsers(Guid guid)
        {


            return _userGroupRepository.GetUsers(guid);


        }






    }
}
