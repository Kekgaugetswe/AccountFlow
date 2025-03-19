using AccountFlow.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Web.Domain.About.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            // Retrieve content from the static class
            var aboutContent = new
            {
                CompanyName = AboutContent.CompanyName,
                Description = AboutContent.Description,
                Mission = AboutContent.Mission,
                Vision = AboutContent.Vision
            };

            // Pass it to the view
            return View(aboutContent);
        }
    }
}
