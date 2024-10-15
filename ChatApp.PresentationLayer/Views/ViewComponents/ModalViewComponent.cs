using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.VMs;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
    public class ModalViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

       
        public ModalViewComponent(IUserService userService) { 

            _userService = userService;
      
        }

        public async Task<IViewComponentResult> InvokeAsync(string page)
        {
            var hostUser = await _userService.GetHostUser();

            var Users = _userService.GetFilteredList(i => i.RowGuid != hostUser.RowGuid);

            var Groups = _userService.IncludeGroup(i => i.RowGuid == hostUser.RowGuid);

            ChatPartialViewModel modalViewModel = new ChatPartialViewModel()
            {
                Groups = Groups.Groups,
                Users = Users

            };
          
           
            return View(page,modalViewModel);
        }
    }
}
