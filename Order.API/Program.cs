using Microsoft.EntityFrameworkCore;
using Order.API.Controllers;
using Order.API.Data;
using Order.API.IRepo;
using Order.API.KafkaConsumer;
using Order.API.Repo;
using System;

namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumers>();

            // Database Connections
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDB")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            var kafkaConsumers = app.Services.GetRequiredService<IKafkaConsumer>();
            kafkaConsumers.RunInBackground();

            app.Run();
        }
    }
}
