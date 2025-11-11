using ChatChannel.Domain.Model.Contracts;
using ChatChannel.Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(ChatDbContext users)
        {
            _users = users.Set<User>();
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _users.SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<User> GetUserWithMessages(string username)
        {
            return await _users.Include(x => x.Messages).SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> IsUserASupport(string username)
        {
            return await _users.AnyAsync(x => x.Username.ToLower() == username.ToLower() && x.role == Domain.Model.Enums.Role.Support);
        }

        public async Task<bool> IsUserExist(string username)
        {
            return await _users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<List<User>> SeeAllUnreadMessages()
        {
            return  await _users.Include(x => x.Messages).Where(x=>x.HaveUnreadMessage == true).ToListAsync();
        }
    }
}
