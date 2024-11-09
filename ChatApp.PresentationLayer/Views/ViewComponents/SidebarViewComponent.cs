using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {


            return View();

        }
    }
}
