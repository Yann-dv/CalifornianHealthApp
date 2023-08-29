using CalifornianHealthFrontendUpdated.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalifornianHealthFrontendUpdated.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await GetDoctorTeam();
            return View();
        }

        [HttpGet]
        public async Task<PartialViewResult> GetDoctorTeam()
        {
            var calendarController = new CalendarController();
            var getConsultants = await Task.Run(() => calendarController.Index());
            
            var fetchConsultants = getConsultants.Value as List<Consultant>;

            return PartialView("_DoctorTeam", fetchConsultants);
        }
    }
}