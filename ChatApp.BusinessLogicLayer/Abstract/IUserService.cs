using ChatApp.EntitiesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BusinessLogicLayer.Abstract
{
    public interface IUserService
    {
        public List<AppUser> GetAll();

        public AppUser Get(Func<AppUser, bool> filter);

        public Task<AppUser> GetHostUser();

        public AppUser IncludeGroup(Func<AppUser, bool> filter);

        public List<AppUser> GetFilteredList(Func<AppUser, bool> filter);
    }
}
