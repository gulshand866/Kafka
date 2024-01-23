
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.IRepo;
using Product.API.KafkaProducer;
using Product.API.Repo;

namespace Product.API
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

            builder.Services.AddScoped<IProductRepo, ProductRepo>();

            builder.Services.AddSingleton<IKafkaProducer, KafkaProducers>();
            builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

            // Database Connections
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDB")));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
