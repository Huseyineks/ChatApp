using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
    public class ScriptsViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {



            return View();
        }
    }
}
