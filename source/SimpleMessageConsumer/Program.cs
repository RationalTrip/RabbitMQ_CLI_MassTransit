using Microsoft.Extensions.Configuration;
using MessageCliCommunication.Contract.Configuration;
using MassTransit;
using SimpleMessageConsumer.MessageTransit.Upload;

IConfiguration configuration = new ConfigurationBuilder()
    .AddRabbitMQSharedConfiguration()
    .AddJsonFile("appconfig.json")
    .Build();

var bus = Bus.Factory.CreateUsingRabbitMq(conf =>
{
    conf.SharedRabbitMqHost(configuration);

    conf.ReceiveEndpoint(configuration["Transit:messaging:receiveEndpoint"], endpointConfig =>
    {
        endpointConfig.Consumer<UploadConsumer>();
    });
});

await bus.StartAsync();

try
{
    Console.WriteLine("Enter any key to close the program");
    Console.ReadKey();
}
finally
{
    await bus.StopAsync();
}