using Confluent.Kafka;
using Kurier.ClientService.Kafka;
using Microsoft.OpenApi.Models;

namespace Kurier.ClientService
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
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client service", Version = "v1" }); });

            builder.Services.AddSingleton<IConsumer<string, string>>(sp =>
            {
                var config = new ConsumerConfig
                {
                    GroupId = "order-service-group",
                    BootstrapServers = "localhost:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };
                return new ConsumerBuilder<string, string>(config).Build();
            });

            builder.Services.AddHostedService<KafkaConsumerHandler>();

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