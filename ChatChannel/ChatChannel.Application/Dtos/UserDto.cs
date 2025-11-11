using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Application.Dtos
{
    public class UserDto
    {
        public UserDto(string username, Guid userId, List<MessageDto> content)
        {
            Username = username;
            UserId = userId;
            Content = content;
        }

        public string Username { get; private set; }
        public Guid UserId { get; private set; }
        public List<MessageDto> Content { get; private set; }
    }
}
