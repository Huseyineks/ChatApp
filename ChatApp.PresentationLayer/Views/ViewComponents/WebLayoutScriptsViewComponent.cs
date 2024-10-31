using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
	public class WebLayoutScriptsViewComponent : ViewComponent
	{

		public IViewComponentResult Invoke()
		{



			return View();
		}
	}
}
