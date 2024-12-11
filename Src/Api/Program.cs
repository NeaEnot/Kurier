using Ocelot.DependencyInjection;

namespace Kurier.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOcelot();
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<SwaggerAggregator>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapGet("/swagger/v1/swagger.json", async (IServiceProvider serviceProvider) =>
                {
                    var aggregator = serviceProvider.GetRequiredService<SwaggerAggregator>();
                    var aggregatedDoc = await aggregator.AggregateSwaggerDocsAsync();
                    return Results.Json(aggregatedDoc);
                });

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