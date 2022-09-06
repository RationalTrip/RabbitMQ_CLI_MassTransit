using ModificationMessageConsumer.Models;

namespace ModificationMessageConsumer.MessageTransit.Modify
{
    internal interface IModifySender
    {
        Task PublishMessageAsync(Message message, bool modifyForEveryone);
    }
}
