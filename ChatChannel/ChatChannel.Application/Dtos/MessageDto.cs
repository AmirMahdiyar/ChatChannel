using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Application.Dtos
{
    public class MessageDto
    {
        public MessageDto(string username, string content, DateTime sentDate)
        {
            Username = username;
            Context = content;
            SentDate = sentDate;
        }

        public string Username { get; private set; }
        public string Context { get; private set; }
        public DateTime SentDate { get; private set; }
    }
}
