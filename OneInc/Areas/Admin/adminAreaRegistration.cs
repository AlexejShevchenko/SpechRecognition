using System.Web.Mvc;

namespace OneInc.Areas.Admin.Controllers
{
    public class adminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller="Question", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}