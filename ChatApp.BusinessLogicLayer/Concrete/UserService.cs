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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) { 
        
            _userRepository = userRepository;
        
        }

        public AppUser Get(Func<AppUser, bool> filter)
        {
            return _userRepository.Get(filter);
        }

        public List<AppUser> GetAll()
        {
            return _userRepository.GetAll();
        }

        public List<AppUser> GetFilteredList(Func<AppUser, bool> filter)
        {
            return _userRepository.GetFilteredList(filter);
        }

        public async  Task<AppUser> GetHostUser()
        {
           return await _userRepository.GetHostUser();
        }

        public AppUser IncludeGroup(Func<AppUser, bool> filter)
        {
            return _userRepository.IncludeGroup(filter);
        }
    }
}
