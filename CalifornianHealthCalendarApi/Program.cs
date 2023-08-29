using CalifornianHealthLib.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var scope = Environment.GetEnvironmentVariable("ASPNETCORE_SCOPE");
Console.WriteLine("Defined scope: " + scope);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

if(scope == "docker")
{
    builder.Services.AddDbContext<ChDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DockerConnection")));
    Console.WriteLine(builder.Configuration.GetConnectionString("DockerConnection"));
}
else
{
    builder.Services.AddDbContext<ChDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
}

Console.WriteLine("Defined scope: " + scope);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Californian Health API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
});
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();