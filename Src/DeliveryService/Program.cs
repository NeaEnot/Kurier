using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.DeliveryService.Kafka;
using Microsoft.OpenApi.Models;

namespace Kurier.DeliveryService
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
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Delivery service", Version = "v1" }); });

            builder.Services.AddKafka<KafkaConsumerHandler>(builder.Configuration);

            builder.Services.AddSingleton<IUserStorage, IUserStorage>(); // TODO: Заменить на реализацию

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
        }
    }
}