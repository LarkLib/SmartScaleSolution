using Microsoft.Extensions.Primitives;

namespace SmartScaleWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var LogLevel = hostContext.Configuration.GetValue<string>("Logging:LogLevel:Default");
                    services.AddHostedService<Worker>();
                })
                .Build();
            host.Run();
        }
    }
}