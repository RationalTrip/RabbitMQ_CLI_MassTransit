using AutoMapper;
using ModificationMessageConsumer.Models;
using MessageCliCommunication.Contract.Dtos;

namespace ModificationMessageConsumer.Profiles
{
    internal class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<IMessageUploadDto, Message>();
            CreateMap<Message, MessageModifyDto>();
        }
    }
}
