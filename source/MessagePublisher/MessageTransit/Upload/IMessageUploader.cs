using MessagePublisher.Models;

namespace MessagePublisher.MessageTransit.Upload
{
    internal interface IMessageUploader
    {
        Task PublishMessageAsync(Message message);
    }
}
