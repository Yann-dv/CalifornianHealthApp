using CalifornianHealthBookingServer.TestContext;
using CalifornianHealthLib.Context;
using CalifornianHealthLib.Models;
using CalifornianHealthLib.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CalifornianHealthBookingServer.Controllers
{
    public partial class RabbitMqController : RabbitMqBookingRequestConsumer
    {
        private readonly ChDbContext? _context;
        private readonly TestDbContext? _testContext;
        private string _hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";


        public RabbitMqController(ChDbContext? context, TestDbContext? testContext)
        {
            _context = context;
            _testContext = testContext;
        }

        protected override async Task<bool> UpdateBookings(string message, string requestQueueName)
        {
            try
            {
                string responseQueueName = requestQueueName switch
                {
                    "ch_queue_booking_request" => "ch_queue_booking_response",
                    "ch_queue_test_request" => "ch_queue_test_response",
                    _ => null
                } ?? string.Empty;

                if (requestQueueName == "ch_queue_test_request")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    //Console.WriteLine($" [+] TestListener -> Received test booking request: {message}");
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (await TryUpdateDb(message, responseQueueName, null, _testContext))
                    {
                    }
                    else
                    {
                        //Console.WriteLine(" -> Booking request failed to update test db");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($" [+] Listener -> Received booking request: {message}");
                    Console.ForegroundColor = ConsoleColor.Black;
                    await TryUpdateDb(message, responseQueueName, _context, null);
                }

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(false);
            }
        }

        private async Task<bool> TryUpdateDb(string message, string responseQueue, ChDbContext? context,
            TestDbContext? testContext)
        {
            Query? booking;
            try
            {
                booking = JsonConvert.DeserializeObject<Query>(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deserializing message: " + e.Message);
                return false;
            }

            var consultantId = booking.ConsultantId.Value;

            //App queue
            if (Task.FromResult(ConsultantExists(consultantId, testContext)).Result.IsCompletedSuccessfully)
            {
                //Console.WriteLine(" ... Consultant found");
            }
            else
            {
                //Console.WriteLine(" ... Consultant not found");
                return false;
            }

            var consultantCalendar = new List<ConsultantCalendar>();
            if (_testContext != null && context == null)
            {
                if (_testContext.ConsultantCalendars != null)
                {
                    consultantCalendar = await _testContext.ConsultantCalendars.Select(c => c)
                        .Where(c => c.ConsultantId == consultantId).ToListAsync();
                }
            }

            else
            {
                consultantCalendar = await _context.ConsultantCalendars.Select(c => c)
                    .Where(c => c.ConsultantId == consultantId).ToListAsync();
            }

            var consultantCalendarDate = consultantCalendar.Find(c => c.Date == booking.Date && c.Available == true);

            if (consultantCalendarDate == null)
            {
                //Console.WriteLine(" ... Consultant not available on this date");
                return false;
            }
            else
            {

                //Update available to false = booked
                consultantCalendarDate.Available = false;

                if (testContext != null)
                {
                    _testContext.Entry(consultantCalendarDate).State = EntityState.Modified;
                }
                else
                {
                    _context.Entry(consultantCalendarDate).State = EntityState.Modified;
                }
                

                try
                {
                    Console.WriteLine(" ... Try update db from booking request");
                    if (testContext != null)
                    {
                        await _testContext.SaveChangesAsync();

                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("Test db updated with booking -> " + "messageId: " + booking.MessageId);
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        await _context.SaveChangesAsync();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" [v] Booking updated: " + "messageId: " + booking.MessageId + " - " +
                                          "id: " +
                                          booking.Id + " - " + "ConsultantId: " + booking.ConsultantId + " - " +
                                          "Date: " +
                                          booking.Date + " - " + "Status: booked");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    try
                    {
                        Console.WriteLine(" -> Success booking response send to the response queue");
                        var rabbitMqPublisher = new RabbitMqBookingRequestPublisher();

                        var query = new Query()
                        {
                            MessageId = booking.MessageId,
                            Id = booking.Id,
                            ConsultantId = booking.ConsultantId,
                            Date = booking.Date,
                            Available = consultantCalendarDate.Available
                        };
                        
                        rabbitMqPublisher.PublishBookingMessage(query, responseQueue, _hostName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return false;
                    }

                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    /*if (!ConsultantExists(consultantId, testContext))
                    {
                        return false;
                    }*/

                    if (testContext == null) throw;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Test db non updated -> already booked by previous queue message");
                    Console.ForegroundColor = ConsoleColor.Black;
                    return false;
                }
            }
        }

        private async Task<bool> ConsultantExists(int id, TestDbContext? testDbContext)
        {
            if (testDbContext != null)
            {
                var testDbSet = testDbContext.Consultants ?? throw new NullReferenceException();
                return await Task.FromResult((testDbContext.Consultants?.Any(e => e.Id == id) ?? false));
            }
            var dbSet = _context.Consultants ?? throw new NullReferenceException();
            return await Task.FromResult((_context.Consultants?.Any(e => e.Id == id) ?? false));
        }
    }
}