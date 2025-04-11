using Kurier.DeliveryService.Controllers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Kurier.DeliveryService.OpenTelemetry
{
    public static class OpenTelemetryConfigurationHelper
    {
        public static void AddOpenTelemetry(this IHostApplicationBuilder builder)
        {
            builder
               .Services
               .AddOpenTelemetry()
               .WithTracing(tracing =>
               {
                   tracing.AddOtlpExporter(oltp =>
                          {
                              oltp.Endpoint = new Uri("http://oltp:4317");
                          })
                          .ConfigureResource(rb =>
                          {
                              var name = typeof(CourierController).Assembly.GetName();
                              rb.AddService(
                                   serviceName: name.Name!,
                                   serviceVersion: name.Version!.ToString(),
                                   autoGenerateServiceInstanceId: true);
                          });
               });
        }
    }
}
