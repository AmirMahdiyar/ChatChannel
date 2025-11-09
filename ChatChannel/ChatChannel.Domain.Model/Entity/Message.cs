using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatChannel.Domain.Model.Entity
{
    public class Message
    {
        private Message() { } // ef
        public Message(string context, bool isFromSupport)
        {
            Context = JsonSerializer.Serialize(context);
            Date = DateTime.Now;
            IsFromSupport = isFromSupport;
        }

        public int Id { get; private set; }
        public string Context { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsFromSupport { get; private set; }
    }
}
