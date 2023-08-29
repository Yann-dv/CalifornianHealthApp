using CalifornianHealthLib.Models;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealthLib.Context
{
    public class ChDbContext : DbContext
    {
        public ChDbContext(DbContextOptions<ChDbContext> options)
            : base(options)
        {
        
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Consultant> Consultants { get; set; }

        public DbSet<ConsultantCalendar> ConsultantCalendars { get; set; }

        public DbSet<Patient> Patients { get; set; }
    }
}