using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
    public class ResultMessagesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {



            return View();
        }
    }
}
