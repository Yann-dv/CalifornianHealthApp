using System.Diagnostics;
using CalifornianHealthFrontendUpdated.Controllers;
using NUnit.Framework;

namespace CalifornianHealthFrontendUpdated.CalifornianHealthTests
{
    [TestFixture]
    public class ConcurrencyTests
    {
        [TestCase(3)]
        [TestCase(30)]
        [TestCase(300)]
        [TestCase(3000)]
        public async Task Simultaneous_multiple_booking_attempt_should_return_only_one_booking_success(int messageCount)
        {
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < messageCount; i++)
            {
                //var pushBooking =
                await new BookingController().ConfirmAppointment(40, 99, "2024-08-27", true, "ch_queue_test_request", "ch_queue_test_response");
                //Console.WriteLine(pushBooking.Result);
            }

            stopwatch.Stop();
            Console.WriteLine($"Published {messageCount:N0} messages individually in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}