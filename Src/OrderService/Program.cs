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

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order service", Version = "v1" });

                c.AddSecurityDefinition("custom_auth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "GUID token, передаваемый в заголовке Authorization"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "custom_auth"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddHttpClient("ApiGateway", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"] ?? "");
            });

            builder.Services.AddDbServices(builder.Configuration);
            builder.Services.AddScoped<IOrderStorage, PostgresOrderStorage>();

            builder.Services.AddKafka<KafkaConsumerHandler>(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}