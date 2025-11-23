using ChatChannel.Domain.Model.Contracts;
using ChatChannel.Domain.Model.Entity;
using ChatChannel.Domain.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace ChatChannel.Infraustructure.EfRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatDbContext _users;

        public UserRepository(ChatDbContext users)
        {
            _users = users;
        }

        public DatabaseTypes DbType => DatabaseTypes.SqlServer;

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _users.Users.SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<User> GetUserWithMessages(string username)
        {
            return await _users.Users.Include(x => x.Messages).SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> IsUserASupport(string username)
        {
            return await _users.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower() && x.role == Domain.Model.Enums.Role.Support);
        }

        public async Task<bool> IsUserExist(string username)
        {
            return await _users.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<List<User>> SeeAllUnreadMessages()
        {
            return await _users.Users.Include(x => x.Messages).Where(x => x.HaveUnreadMessage == true).ToListAsync();
        }

        public Task UpdateUser(User user, Message messageToPush, bool haveRead)
        {
            return Task.CompletedTask;
        }
    }
}
