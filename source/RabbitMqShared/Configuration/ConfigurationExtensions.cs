using MassTransit;
using Microsoft.Extensions.Configuration;

namespace MessageCliCommunication.Contract.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddRabbitMQSharedConfiguration(this IConfigurationBuilder builder) 
            => builder.AddJsonFile("masstransitSharedConfig.json");

        public static void SharedRabbitMqHost(this IRabbitMqBusFactoryConfigurator configurator, IConfiguration configuration)
        {
            var hostConfig = configuration.GetRequiredSection("Transit:connection");

            configurator.Host(hostConfig["host"], ushort.Parse(hostConfig["port"]), hostConfig["virtualHost"], host =>
            {
                host.Username(hostConfig["login"]);
                host.Password(hostConfig["Password"]);
            });
        }

        public static string GetRabbitMqUrl(this IConfiguration config)
        {
            var hostConfig = config.GetRequiredSection("Transit:connection");

            return $"amqps://{hostConfig["host"]}:{hostConfig["port"]}/{hostConfig["virtualHost"]}/";
        }
    }
}
