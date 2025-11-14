using MongoDB.Bson.Serialization.Attributes;

namespace ChatChannel.Domain.Model.Entity
{
    public class Message
    {
        private Message() { } // ef
        public Message(string context, bool isFromSupport)
        {
            Context = context;
            Date = DateTime.Now;
            IsFromSupport = isFromSupport;
        }
        [BsonElement("_id")]
        public int Id { get; private set; }
        public string Context { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsFromSupport { get; private set; }
    }
}
