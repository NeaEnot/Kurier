using Kurier.Api.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerForOcelotUI(opt => {
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                }).UseOcelot().Wait();
            }

            app.UseMiddleware<AuthServiceMiddleware>();
            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}