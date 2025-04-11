using Kurier.Api.Middlewares;
using Kurier.Common.Interfaces;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;
//using InfrastructureDB.Data.Seed;

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
            builder.Host.UseSerilog();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerForOcelotUI(opt => {
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                }).UseOcelot().Wait();
            }
            using (var scope = app.Services.CreateScope())
            {
                //var logger = scope.ServiceProvider.GetRequiredService<IApplicationLogger<Program>>();

                //try
                //{
                //    // TO DO: надо бы допилить сид дату, шобы в базу подгружались ТД автоматом
                //    //KurierContextSeed.SeedAsync(builder.Configuration, scope);
                //}
                //catch (Exception ex)
                //{
                //    logger.LogError(ex, $"Error in <{nameof(Program)}>");
                //    throw;
                //}
            }

            app.UseMiddleware<AuthServiceMiddleware>();
            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}