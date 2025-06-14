using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using VenueService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VenueContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("VenueConnection")));
//builder.Services.AddSingleton(sp =>
//{
//    var config = sp.GetRequiredService<IConfiguration>().GetSection("RabbitMQ");
//    return new ConnectionFactory()
//    {
//        HostName = config["HostName"],
//        UserName = config["UserName"],
//        Password = config["Password"]
//    };
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
