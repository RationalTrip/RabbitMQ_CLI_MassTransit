using MassTransit;
using MessageCliCommunication.Contract.Dtos;

namespace SimpleMessageConsumer.MessageTransit.Upload
{
    internal class UploadConsumer : IConsumer<IMessageUploadDto>
    {
        public Task Consume(ConsumeContext<IMessageUploadDto> context)
        {
            var messageUploadModel = context.Message;

            Console.WriteLine($"Received message with id \"{messageUploadModel.Id}\": " +
                $"\"{messageUploadModel.Value}\"");

            return Task.CompletedTask;
        }
    }
}
