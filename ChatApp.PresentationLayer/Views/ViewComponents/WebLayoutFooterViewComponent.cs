using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
	public class WebLayoutFooterViewComponent : ViewComponent
	{

		public IViewComponentResult Invoke()
		{


			return View();
		}
	}
}
