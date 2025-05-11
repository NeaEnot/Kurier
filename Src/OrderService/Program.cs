using Kurier.Common.ApiConfiguration;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.OrderService.Kafka;
using Kurier.RedisStorage;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using InfrastructureDB.Storages;
using InfrastructureDB.Data;

namespace Kurier.OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureLogging();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order service", Version = "v1" }); });

            builder.Services.AddHttpClient("ApiGateway", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"] ?? "");
            });
            builder.Services.AddScoped<IOrderStorage, PostgresOrderStorage>();

            builder.Services.AddDbServices(builder.Configuration);

            builder.Services.AddKafka<KafkaConsumerHandler>(builder.Configuration);

            builder.Host.UseSerilog();

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