namespace MessageCliCommunication.Contract.Dtos
{
    public class MessageModifyDto : IMessageModifyDto
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string EndPointOutput { get; set; } = string.Empty;
    }
}
