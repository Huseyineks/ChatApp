using ChatApp.DataAccesLayer.Abstract;
using ChatApp.EntitiesLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccesLayer.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IHttpContextAccessor _contextAccessor;
        public UserRepository(UserManager<AppUser> userManager,IHttpContextAccessor contextAccessor) {
        
        
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        
        }

        public List<AppUser> GetAll()
        {
            return _userManager.Users.ToList();
        }

        public AppUser Get(Func<AppUser,bool> filter)
        {

            return GetAll().FirstOrDefault(filter);
        }

        public async Task<AppUser> GetHostUser()
        {
            var user = _contextAccessor.HttpContext.User;

            AppUser hostUser = await _userManager.GetUserAsync(user);
            return hostUser;
        }

        public AppUser IncludeGroup(Func<AppUser,bool> filter)
        {
            return _userManager.Users.Include(i => i.Groups).ThenInclude(i => i.Group).FirstOrDefault(filter);
        }

        public List<AppUser> GetFilteredList(Func<AppUser,bool> filter)
        {

            return GetAll().Where(filter).ToList();
        }

      
    }
}
