using ChatChannel.Domain.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Domain.Model.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<bool> IsUserExist(string username);    
        Task<bool> IsUserASupport(string username);
        void AddUser(User user);
        Task<User> GetUserWithMessages(string username);
        Task<List<User>> SeeAllUnreadMessages();
    }
}
