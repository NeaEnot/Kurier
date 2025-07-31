using Kurier.Api.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;
using InfrastructureDB.Data;

namespace Kurier.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json");

            builder.Services.AddControllers();
            builder.Services.AddOcelot().AddPolly();
            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();
            builder.Services.AddDbServices(builder.Configuration);
            builder.Host.UseSerilog();

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

            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseMiddleware<AuthServiceMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerForOcelotUI(opt =>
                {
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                });
            }

            //app.UseHttpsRedirection();
            app.UseOcelot().Wait();
            app.MapControllers();

            app.Run();
        }
    }
}