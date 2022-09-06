using AutoMapper;
using MessagePublisher.Models;
using MessagePublisher.Profiles;
using Microsoft.Extensions.Configuration;
using MessageCliCommunication.Contract.Configuration;
using MassTransit;
using MessagePublisher.MessageTransit.Upload;
using MessagePublisher.MessageTransit.Modify;

IConfiguration configuration = new ConfigurationBuilder()
    .AddRabbitMQSharedConfiguration()
    .Build();

IMapper mapper = new MapperConfiguration(opt => opt.AddProfile<MessageProfile>())
    .CreateMapper();

IBusControl bus = null!;
bus = Bus.Factory.CreateUsingRabbitMq(conf =>
{
    conf.SharedRabbitMqHost(configuration);

    conf.ReceiveEndpoint(configuration["Transit:messaging:modifyRequestEndpoint"], endpointConfig =>
    {
        endpointConfig.Consumer(() => new ModifyConsumer(bus, mapper));
    });
});

IMessageUploader uploadHandler = new MessageUploader(bus, mapper);

await bus.StartAsync();

try
{
    string exitString = "/exit";

    for (int messageId = 0; ; messageId++)
    {
        Console.Write($"Print message or \"{exitString}\" to end the program: ");

        var input = Console.ReadLine();

        if (input == null || input.Trim() == exitString)
            break;

        var message = new Message { Id = messageId, Value = input };

        await uploadHandler.PublishMessageAsync(message);
    }
}
finally
{
    await bus.StopAsync();
}