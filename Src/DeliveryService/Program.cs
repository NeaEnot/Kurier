using Kurier.Common.ApiConfiguration;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.DeliveryService.Controllers;
using Kurier.DeliveryService.Kafka;
using Kurier.RedisStorage;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using StackExchange.Redis;
using Kurier.DeliveryService.OpenTelemetry;

namespace Kurier.DeliveryService
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
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Delivery service", Version = "v1" }); });

            builder.Services.AddKafka<KafkaConsumerHandler>(builder.Configuration);
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { builder.Configuration["Redis:ConnectionAddress"] ?? "127.0.0.1:6379" },
                ConnectTimeout = 5000, // Время ожидания в миллисекундах
                SyncTimeout = 5000,     // Таймаут синхронных операций
                AbortOnConnectFail = false
            };

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(configurationOptions));
            builder.Services.AddSingleton<IDeliveryStorage, RedisDeliveryStorage>();
            builder.Services.AddSingleton<IAuthTokenStorage, RedisAuthTokenStorage>();

            builder.AddOpenTelemetry();

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