using AutoMapper;
using Microsoft.Extensions.Configuration;
using ModificationMessageConsumer.Profiles;
using MessageCliCommunication.Contract.Configuration;
using MassTransit;
using ModificationMessageConsumer.MessageTransit.Upload;
using ModificationMessageConsumer.MessageTransit.Modify;

IConfiguration configuration = new ConfigurationBuilder()
    .AddRabbitMQSharedConfiguration()
    .AddJsonFile("appconfig.json")
    .Build();

IMapper mapper = new MapperConfiguration(opt => opt.AddProfile<MessageProfile>())
    .CreateMapper();

IBusControl bus = null!;
bus = Bus.Factory.CreateUsingRabbitMq(conf =>
{
    conf.SharedRabbitMqHost(configuration);

    conf.ReceiveEndpoint(configuration["Transit:messaging:receiveEndpoint"], endpointConfig =>
    {
        endpointConfig.Consumer(
            () => new UploadConsumer(new ModifySender(bus, mapper, configuration), mapper));
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