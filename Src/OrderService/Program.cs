using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.OrderService.Kafka;
using Kurier.RedisStorage;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace Kurier.OrderService
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
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order service", Version = "v1" }); });

            builder.Services.AddHttpClient("ApiGateway", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"]);
            });

            builder.Services.AddKafka<KafkaConsumerHandler>(builder.Configuration);

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionAddress"]));
            builder.Services.AddSingleton<IOrderStorage, RedisOrderStorage>();

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