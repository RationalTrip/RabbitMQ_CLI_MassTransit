using AutoMapper;
using MassTransit;
using MessageCliCommunication.Contract.Dtos;
using MessagePublisher.Models;

namespace MessagePublisher.MessageTransit.Upload
{
    internal class MessageUploader : IMessageUploader
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly IMapper _mapper;

        public MessageUploader(IPublishEndpoint endpoint, IMapper mapper)
        {
            _endpoint = endpoint;
            _mapper = mapper;
        }

        public async Task PublishMessageAsync(Message message)
        {
            var messageUploadModel = _mapper.Map<MessageUploadDto>(message);

            await _endpoint.Publish<IMessageUploadDto>(messageUploadModel);
        }
    }
}
