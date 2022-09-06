using AutoMapper;
using MassTransit;
using MessageCliCommunication.Contract.Dtos;
using ModificationMessageConsumer.MessageTransit.Modify;
using ModificationMessageConsumer.Models;

namespace ModificationMessageConsumer.MessageTransit.Upload
{
    internal class UploadConsumer : IConsumer<IMessageUploadDto>
    {
        private readonly IModifySender _modifySender;
        private readonly IMapper _mapper;

        public UploadConsumer(IModifySender modifySender, IMapper mapper)
        {
            _modifySender = modifySender;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<IMessageUploadDto> context)
        {
            var messageUploadModel = context.Message;

            Console.WriteLine($"Received message with id \"{messageUploadModel.Id}\": " +
                $"\"{messageUploadModel.Value}\"");

            var messageValue = messageUploadModel.Value;
            if(messageValue.StartsWith(" ") || messageValue.EndsWith(" "))
            {
                bool isMessageModifyForEveryone = messageValue.StartsWith(" ");

                var message = _mapper.Map<Message>(messageUploadModel);
                message.Value = message.Value.Trim();

                await _modifySender.PublishMessageAsync(message, isMessageModifyForEveryone);

                var modifyType = isMessageModifyForEveryone ? "for everyone" : "only for current service";

                Console.WriteLine($"\tMessage with {message.Id} should be modified {modifyType}: " +
                    $"\"{message.Value}\"");
            }
        }
    }
}
