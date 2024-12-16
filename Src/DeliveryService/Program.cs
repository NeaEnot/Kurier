
using Confluent.Kafka;
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

            builder.Services.AddSingleton<IProducer<string, string>>(sp =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092",
                    EnableIdempotence = true,
                    Acks = Acks.All
                };
                return new ProducerBuilder<string, string>(config).Build();
            });

            builder.Services.AddSingleton<IConsumer<string, string>>(sp =>
            {
                var config = new ConsumerConfig
                {
                    GroupId = "delivery-service-group",
                    BootstrapServers = "localhost:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };
                return new ConsumerBuilder<string, string>(config).Build();
            });

            builder.Services.AddHostedService<KafkaConsumerHandler>();
            builder.Services.AddSingleton<KafkaProducerHandler>();

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