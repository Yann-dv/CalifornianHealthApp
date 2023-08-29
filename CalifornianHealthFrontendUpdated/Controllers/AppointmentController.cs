using CalifornianHealthFrontendUpdated.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CalifornianHealthFrontendUpdated.Controllers
{

    public class AppointmentController : Controller
    {
        private readonly string _calendarApiUri = Environment.GetEnvironmentVariable("ASPNETCORE_SCOPE") == "docker" ? "http://host.docker.internal:5000/api/Consultants" : "https://localhost:44366/api/Consultants";

        // GET
        public async Task<ActionResult> Index()
        {
            var appointmentsList = new List<ConsultantCalendar>();

            var consultantIds = 4;
            for (var i = 0; i < consultantIds; i++)
            {
                var getConsultantCalendar = await Task.Run(() => new CalendarController().GetConsultantCalendar(i + 1));

                var consultantCalendarData = getConsultantCalendar.Value as List<ConsultantCalendar>;
                appointmentsList.AddRange(consultantCalendarData);
            }
            
            return View(appointmentsList);
        }

        private async Task<ConsultantModelList> GetConsultantModelList()
        {
            var conList = new ConsultantModelList();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_calendarApiUri))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var apiResponse = await response.Content.ReadAsStringAsync();
                            var consultantsFromData = JsonConvert.DeserializeObject<List<Consultant>>(apiResponse);
                            foreach (var c in consultantsFromData)
                            {
                                c.DropDownTitle = "Dr." + " " + c.FName + " " + c.LName;
                            }

                            conList.ConsultantsList = new SelectList(consultantsFromData, "Id", "DropDownTitle");
                            conList.Consultants = consultantsFromData;
                            //For Debug
                            //Console.WriteLine("Api response: " + apiResponse);
                        }
                        else
                            ViewBag.StatusCode = response.StatusCode;
                    }
                }
            }
            catch (System.Exception e)
            {
                ViewBag.StatusCode = e.Message;
            }

            return conList;
        }
    }
}