using AutoMapper;
using MassTransit;
using MessageCliCommunication.Contract.Dtos;

namespace MessagePublisher.MessageTransit.Modify
{
    internal class ModifyConsumer : IConsumer<IMessageModifyDto>
    {
        private readonly IBus _transitBus;
        private readonly IMapper _mapper;

        public ModifyConsumer(IBus transitBus, IMapper mapper)
        {
            _transitBus = transitBus;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<IMessageModifyDto> context)
        {
            var message = context.Message;

            if(message == null)
                throw new ArgumentNullException(nameof(message));

            var messageUploadModel = _mapper.Map<MessageUploadDto>(message);
            
            if (string.IsNullOrWhiteSpace(message.EndPointOutput))
            {
                await _transitBus.Publish<IMessageUploadDto>(messageUploadModel);
            }
            else
            {
                var endPoint = await _transitBus.GetSendEndpoint(new Uri(message.EndPointOutput));

                await endPoint.Send<IMessageUploadDto>(messageUploadModel);
            }

        }
    }
}
