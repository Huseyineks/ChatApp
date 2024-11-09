using ChatApp.BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.PresentationLayer.Views.ViewComponents
{
    public class ConfirmMailModalViewComponent : ViewComponent
    {


        public IViewComponentResult Invoke()
        {
            ConfirmMailDTO confirmMailDTO = new ConfirmMailDTO();


            return View(confirmMailDTO);
        }


    }
}
