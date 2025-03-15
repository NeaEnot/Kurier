using Kurier.Api.Middlewares;
using Kurier.Common.Interfaces;
using Kurier.CustomGateway;
using Kurier.CustomGateway.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
//using InfrastructureDB.Data.Seed;

namespace Kurier.Api
{
    public class Program
    {
        private enum GatewayMode { ocelot, custom }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            GatewayMode mode = (GatewayMode)Enum.Parse(typeof(GatewayMode), builder.Configuration["GatewayMode"]);

            builder.Configuration.AddJsonFile("ocelot.json");
            builder.Configuration.AddJsonFile("CustomGatewayConfig.json");

            builder.Services.AddControllers();

            if (mode == GatewayMode.ocelot)
            {
                builder.Services.AddOcelot().AddPolly();
                builder.Services.AddSwaggerForOcelot(builder.Configuration);
            }
            else if (mode == GatewayMode.custom)
            {
                 builder.Services.AddCustomGatewayServices(builder.Configuration);
            }

            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                if (mode == GatewayMode.ocelot)
                    app.UseSwaggerForOcelotUI(opt => { opt.PathToSwaggerGenerator = "/swagger/docs"; }).UseOcelot().Wait();
                else if (mode == GatewayMode.custom)
                    app.UseMiddleware<SwaggerAggregatorMiddleware>();
            }

            if (mode == GatewayMode.custom)
                app.UseCustomGateway();

            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<IApplicationLogger<Program>>();

                try
                {
                    //KurierContextSeed.SeedAsync(builder.Configuration, scope);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error in <{nameof(Program)}>");
                    throw;
                }
            }

            app.UseMiddleware<AuthServiceMiddleware>();
            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}