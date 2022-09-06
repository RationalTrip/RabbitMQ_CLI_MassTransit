using AutoMapper;
using MessagePublisher.Models;
using MessageCliCommunication.Contract.Dtos;

namespace MessagePublisher.Profiles
{
    internal class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageUploadDto>();

            CreateMap<IMessageModifyDto, Message>();

            CreateMap<IMessageModifyDto, MessageUploadDto>();
        }
    }
}
