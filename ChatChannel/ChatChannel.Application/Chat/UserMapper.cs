using ChatChannel.Domain.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatChannel.Domain.Model.Enums;
using ChatChannel.Application.Dtos;

namespace ChatChannel.Application.Chat
{
    public static class UserMapper
    {
        public static User ToCustomerUser(this string user)
        {
            return new User(user, Role.Customer);
        }
        public static User ToSupportUser(this string user)
        {
            return new User(user, Role.Support);
        }
        public static Message ToMessage(this string message , string role)
        {
            if (role == "Support")
                return new Message(message, true);
            return new Message(message, false);
        }

    }
}
