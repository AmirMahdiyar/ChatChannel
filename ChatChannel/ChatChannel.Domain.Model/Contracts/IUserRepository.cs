using ChatChannel.Domain.Model.Entity;
using ChatChannel.Domain.Model.Enums;

namespace ChatChannel.Domain.Model.Contracts
{
    public interface IUserRepository
    {
        DatabaseTypes DbType { get; }

        Task<User> GetUserByUsername(string username);
        Task<bool> IsUserExist(string username);
        Task<bool> IsUserASupport(string username);
        void AddUser(User user);
        Task<User> GetUserWithMessages(string username);
        Task<List<User>> SeeAllUnreadMessages();
        Task UpdateUser(User user, Message messageToPush, bool haveRead);
    }
}
