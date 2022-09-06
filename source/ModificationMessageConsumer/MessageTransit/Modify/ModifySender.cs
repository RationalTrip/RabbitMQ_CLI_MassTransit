using AutoMapper;
using MassTransit;
using MessageCliCommunication.Contract.Configuration;
using MessageCliCommunication.Contract.Dtos;
using Microsoft.Extensions.Configuration;
using ModificationMessageConsumer.Models;

namespace ModificationMessageConsumer.MessageTransit.Modify
{
    internal class ModifySender : IModifySender
    {
        private readonly ISendEndpointProvider _endpointProvider;
        private readonly IMapper _mapper;

        private readonly string _modifyEndpoint;
        private readonly string _responseEndpoint;

        public ModifySender(ISendEndpointProvider endpointProvider, IMapper mapper, IConfiguration config)
        {
            _endpointProvider = endpointProvider;
            _mapper = mapper;

            var url = config.GetRabbitMqUrl();

            var messageSection = config.GetSection("Transit:messaging");

            _modifyEndpoint = url + messageSection["modifyRequestEndpoint"];
            _responseEndpoint = url + messageSection["receiveEndpoint"];
        }

        public async Task PublishMessageAsync(Message message, bool modifyForEveryone)
        {
            var messageModifyModel = _mapper.Map<MessageModifyDto>(message);
            messageModifyModel.EndPointOutput = modifyForEveryone ? "" : _responseEndpoint;

            var modifyEndpoint = await _endpointProvider.GetSendEndpoint(new Uri(_modifyEndpoint));

            await modifyEndpoint.Send<IMessageModifyDto>(messageModifyModel);
        }
    }
}
