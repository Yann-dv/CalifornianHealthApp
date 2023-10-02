using CalifornianHealthBookingServer.Controllers;
using CalifornianHealthBookingServer.TestContext;
using CalifornianHealthLib.Context;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealthBookingServer;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var scope = Environment.GetEnvironmentVariable("ASPNETCORE_SCOPE");
        Console.WriteLine("Defined scope: " + scope);

        builder.Services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        if(scope == "docker")
        {
            builder.Services.AddDbContext<ChDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DockerConnection")));
            builder.Services.AddDbContext<TestDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DockerTestConnection")));
            Console.WriteLine("Docker connection string: " + builder.Configuration.GetConnectionString("DockerConnection"));
        }
        else
        {
            builder.Services.AddDbContext<ChDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services
                .AddDbContext<TestDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection")));
            Console.WriteLine("Default connection string: " + builder.Configuration.GetConnectionString("DefaultConnection"));
        }

        var app = builder.Build();

        //app.MapGet("/", () =>
        //Display();
        //Console.WriteLine("Press any key to exit...");

        var retryCount = 6; // Number of times to retry connecting to RabbitMQ
        var retryDelay = TimeSpan.FromSeconds(5); // Delay between each retry attempt

        for (int retryAttempt = 1; retryAttempt <= retryCount; retryAttempt++)
        {
            try
            {
                await TryStart(builder, retryAttempt);
                break; // If successful, break out of the retry loop
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Retry attempt {retryAttempt}/{retryCount} failed: {ex.Message}");
                if (retryAttempt < retryCount)
                {
                    Console.WriteLine($"Retrying in {retryDelay.TotalSeconds} seconds...");
                    await Task.Delay(retryDelay);
                }
                else
                {
                    Console.WriteLine("All retry attempts failed. Exiting...");
                }
            }
        }

        // Block the main thread to keep the application running
        var cancellationTokenSource = new CancellationTokenSource();
        await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);

        //return 0;


    }

    static async Task TryStart(WebApplicationBuilder builder, int retryAttempt)
    {
        Display();

        void Display()
        {
            var scopeEnv = Environment.GetEnvironmentVariable("ASPNETCORE_SCOPE");
            var input = string.Empty;

            var connectionString = "DefaultConnection";
            var connectionStringTest = "TestConnection";

            if (scopeEnv == "docker")
            {
                connectionString = "DockerConnection";
                connectionStringTest = "DockerConnectionTests";
            }


            var listener = new RabbitMqController(new ChDbContext(
                    new DbContextOptionsBuilder<ChDbContext>()
                        .UseSqlServer(builder.Configuration.GetConnectionString(connectionString))
                        .Options),
                new TestDbContext(
                    new DbContextOptionsBuilder<TestDbContext>()
                        .UseSqlServer(builder.Configuration.GetConnectionString(connectionStringTest))
                        .Options)
            );

            const string requestQueueName = "ch_queue_booking_request";
            const string testRequestQueueName = "ch_queue_test_request";

            var queueList = new List<string>
            {
                requestQueueName,
                testRequestQueueName
            };

            var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
            Console.WriteLine("Defined host name: " + hostName);

            listener.ReceiveBookingMessage(queueList, hostName);

            //Disabled for docker
            //InputCommands.Commands();

            return;
        }
    }


}