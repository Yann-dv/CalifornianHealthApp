using CalifornianHealthFrontendUpdated.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

var builder = WebApplication.CreateBuilder(args);
var scope = Environment.GetEnvironmentVariable("ASPNETCORE_SCOPE");

Console.WriteLine("Defined scope: " + scope);
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDataProtection().UseCryptographicAlgorithms(
    new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

var connectionString = scope == "docker"
    ? builder.Configuration.GetConnectionString("DockerConnection") ??
      throw new InvalidOperationException("Connection string 'DockerConnection' not found.")
    : builder.Configuration.GetConnectionString("DefaultConnection") ??
      throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

/*builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));*/
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

var retryCount = 6; // Number of times to retry connecting to RabbitMQ
var retryDelay = TimeSpan.FromSeconds(5); // Delay between each retry attempt

for (int retryAttempt = 1; retryAttempt <= retryCount; retryAttempt++)
{
    try
    {
        await ConnectRabbitMqListener();
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

app.Run();

Task ConnectRabbitMqListener()
{
    //RabbitMqResponseListening
    var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    Console.WriteLine("Defined host name: " + hostName);

    var responseList = new List<string>();
    responseList.Add("ch_queue_booking_response");
    responseList.Add("ch_queue_test_response");

    try
    {
        new ResponseConsumer().ReceiveBookingMessage(responseList, hostName);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }

    return Task.CompletedTask;
}