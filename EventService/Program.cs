using EventService.Models;
using EventService.Producer;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EventContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("EventConnection")));
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>().GetSection("RabbitMQ");
    return new ConnectionFactory()
    {
        HostName = config["HostName"],
        UserName = config["UserName"],
        Password = config["Password"]
    };
});
builder.Services.AddSingleton<EventMessageProducer>();


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
