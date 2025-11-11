using ChatChannel.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Domain.Model.Entity
{
    public class User
    {
        private User() { } // ef
        private readonly List<Message> _messages = new List<Message>();

        public User(string username, Role role)
        {
            Id = Guid.NewGuid();
            Username = username;
            this.role = role;
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public Role role { get; private set; }
        public bool HaveUnreadMessage { get; private set; } = false;

        public IEnumerable<Message> Messages => _messages.AsReadOnly();


        public void AddMessage(Message message) => _messages.Add(message);

        public void HaveUnreadMessageToTrue()
        {
            this.HaveUnreadMessage = true;
        }
        public void HaveUnreadMessageToFalse()
        {
            this.HaveUnreadMessage = false;
        }

    }
}

