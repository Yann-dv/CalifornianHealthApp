using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalifornianHealthFrontendUpdated.Models
{
    public class ConsultantModelList
    {
        public List<ConsultantCalendar> ConsultantCalendars { get; set; }
        public List<Consultant> Consultants { get; set; }
        public int SelectedConsultantId { get; set; }
        public SelectList ConsultantsList { get; set; }
    }
}