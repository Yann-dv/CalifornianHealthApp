using CalifornianHealthFrontendUpdated.Models;
using CalifornianHealthLib.Models;
using CalifornianHealthLib.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Consultant = CalifornianHealthFrontendUpdated.Models.Consultant;
using ConsultantCalendar = CalifornianHealthFrontendUpdated.Models.ConsultantCalendar;

namespace CalifornianHealthFrontendUpdated.Controllers
{
    public class BookingController : Controller
    {
        private readonly string _calendarApiUri = Environment.GetEnvironmentVariable("ASPNETCORE_SCOPE") == "docker" ? "http://host.docker.internal:5000/api/Consultants" : "https://localhost:44366/api/Consultants";
        
        // GET: Booking
        //the consultant's availability;

        [HttpGet]
        [ActionName("Index")]
        public async Task<ActionResult> Index(int? consultantId)
        {
            var consultantsList = await GetConsultantModelList();
            if(consultantId > 0)
            {
                consultantsList.ConsultantCalendars = await GetSingleConsultantCalendar(consultantId);
                ViewBag.Availabilities = consultantsList.ConsultantCalendars;
                ViewBag.ConsultantId = consultantsList.Consultants.Find(c => c.Id == consultantId).Id;
                ViewBag.SelectedConsultant = consultantsList.Consultants.Find(c => c.Id == consultantId).FName + " " + consultantsList.Consultants.Find(c => c.Id == consultantId).LName;
                return View(consultantsList);
            }
            
            return View(consultantsList);
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
        
        [HttpGet]
        private async Task<Consultant> GetConsultantById(int id)
        {
            var consultant = new Consultant();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_calendarApiUri + "/" + id))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var apiResponse = await response.Content.ReadAsStringAsync();
                            var consultantsFromData = JsonConvert.DeserializeObject<Consultant>(apiResponse);

                            consultant = consultantsFromData;
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

            return consultant;
        }

        private async Task<List<ConsultantCalendar>> GetSingleConsultantCalendar(int? consultantId)
        {
            var calendarController = new CalendarController();
            var getConsultantCalendar = await Task.Run(() => calendarController.GetConsultantCalendar(consultantId));
            
            var consultantCalendarData = getConsultantCalendar .Value as List<ConsultantCalendar>;
            
            if (consultantCalendarData != null)
            {
                return consultantCalendarData;
            }
            return null;
        }

        [ActionName("ConfirmAppointment")]
        public async Task<ActionResult> ConfirmAppointment(int id, int consultantId, string bookingDate, bool available, string bookingRequest, string bookingResponse)
        {
            if (string.IsNullOrWhiteSpace(bookingRequest))
            {
                bookingRequest = "ch_queue_booking_request";
            }
            if(string.IsNullOrWhiteSpace(bookingResponse))
            {
                bookingResponse = "ch_queue_booking_response";
            }
            
            var bookingDone = new ConsultantCalendar()
            {
                Id = id,
                ConsultantId = consultantId,
                Date = DateTime.Parse(bookingDate),
                Available = false
            };

            //Redirect to the booking page if the booking is unsuccessful
            var checkBookingIsUpdated = Task.Run(() => GetSingleConsultantCalendar(consultantId));
            var sendBookingRequest = await SendBooking(id, consultantId, DateTime.Parse(bookingDate), available, bookingRequest, bookingResponse);
            
            if (!sendBookingRequest || checkBookingIsUpdated.Result == null)
            {
                TempData["ErrorMessage"] = "Sorry, the booking was unsuccessful. Please try again.";
                return RedirectToAction("Index", "Booking", new { consultantId });
            }
            
            if (bookingResponse != "ch_queue_test_response")
            {
                Thread.Sleep(500);
                if (Task.Run(() => GetSingleConsultantCalendar(consultantId)).Result
                        ?.Find(c => c.Date == DateTime.Parse(bookingDate) && c.Available == false) == null)
                {
                    TempData["ErrorMessage"] = "Sorry, the booking was unsuccessful. Please try again.";
                    return RedirectToAction("Index", "Booking", new { consultantId });
                }
                else
                {
                    TempData["SuccessMessage"] = "Booking successful!";
                    var consultant = await GetConsultantById(consultantId);
                    return View(Tuple.Create(consultant, bookingDone));
                    
                }
            }

            return RedirectToAction("Index", "Booking", new { consultantId });
        }

        private async Task<bool> SendBooking(int id, int consultantId, DateTime dateBook, bool available, string bookingRequest, string bookingResponse)
        {
            var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

            var sendBooking = new Query()
            {
                MessageId = Guid.NewGuid(),
                Id = id,
                ConsultantId = consultantId,
                Date = dateBook,
                Available = available
            };
            var rabbitMqSender = new RabbitMqBookingRequestPublisher();
            try
            {
                var tasks = new List<Task>()
                {
                    Task.Run(() => rabbitMqSender.PublishBookingMessage(sendBooking, bookingRequest, hostName)),
                };
                await Task.WhenAll(tasks);
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}