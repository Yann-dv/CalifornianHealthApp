using CalifornianHealthFrontendUpdated.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CalifornianHealthFrontendUpdated.Controllers
{
    public class CalendarController : Controller
    {
        private readonly string _calendarApiUri = Environment.GetEnvironmentVariable("ASPNETCORE_SCOPE") == "docker" ? "http://host.docker.internal:5000/api/Consultants" : "https://localhost:44366/api/Consultants";
        
        [HttpGet]
        [ActionName("Index")]
        public JsonResult Index()
        {
            Console.WriteLine("Api uri: " + _calendarApiUri);
            var consultantList = new List<Consultant>();

            try
            {
                using (var response = new HttpClient().GetAsync(_calendarApiUri))
                {
                    if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var apiResponse = response.Result.Content.ReadAsStringAsync().Result;
                        var consultantsFromApi = JsonConvert.DeserializeObject<List<Consultant>>(apiResponse);
                        consultantList = consultantsFromApi;
                    }
                    else
                        ViewBag.StatusCode = response.Result.StatusCode;
                }
            }
            catch (System.Exception e)
            {
                ViewBag.StatusCode = e.Message;
            }

            return Json(consultantList);
        }

        [HttpGet]
        [ActionName("GetConsultantById")]
        public JsonResult GetConsultantById(int? consultantId)
        {
            if (consultantId == null || consultantId == 0)
                return Json(null);

            var consultant = new Consultant();

            try
            {
                using (var response = new HttpClient().GetAsync(_calendarApiUri + "/" + consultantId))
                {
                    if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var apiResponse = response.Result.Content.ReadAsStringAsync().Result;
                        var consultantFromApi = JsonConvert.DeserializeObject<Consultant>(apiResponse);

                        consultant = consultantFromApi;
                    }
                    else
                        ViewBag.StatusCode = response.Result.StatusCode;
                }
            }
            catch (System.Exception e)
            {
                ViewBag.StatusCode = e.Message;
            }

            return Json(consultant);
        }

        [HttpGet]
        [ActionName("GetConsultantCalendar")]
        public JsonResult GetConsultantCalendar(int? consultantId)
        {
            if (consultantId == null || consultantId == 0)
                return Json(null);

            var consultantCalendar = new List<ConsultantCalendar>();
            try
            {
                using (var response = new HttpClient().GetAsync(_calendarApiUri + "/" + consultantId + "/ConsultantAvailabilities"))
                {
                    if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var apiResponse = response.Result.Content.ReadAsStringAsync().Result;
                        var consultantCalendarFromApi = JsonConvert.DeserializeObject<List<ConsultantCalendar>>(apiResponse);

                        consultantCalendar = consultantCalendarFromApi;
                    }
                    else
                        ViewBag.StatusCode = response.Result.StatusCode;
                }
            }
            catch (System.Exception e)
            {
                ViewBag.StatusCode = e.Message;
            }

            return Json(consultantCalendar);
        }
    }
}