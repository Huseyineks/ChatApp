using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
	public class WebLayoutHeaderViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{

			return View();
		}
	}
}
