using CalifornianHealthLib.Models;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealthBookingServer.TestContext;

public class TestDbContext: DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
    public DbSet<ConsultantCalendar>? ConsultantCalendars { get; set; }
    
    public DbSet<Consultant>? Consultants { get; set; }
}