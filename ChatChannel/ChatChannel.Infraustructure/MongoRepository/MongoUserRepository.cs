using ChatChannel.Domain.Model.Contracts;
using ChatChannel.Domain.Model.Entity;
using ChatChannel.Infraustructure.Substructure.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure.MongoRepository
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly MongoSetting _mongoSetting;
        public MongoUserRepository(IOptions<MongoSetting> mongoSetting)
        {
            _mongoSetting = mongoSetting.Value;
            var mongoClient = new MongoClient(_mongoSetting.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_mongoSetting.DatabaseName);
            _users = mongoDatabase.GetCollection<User>(_mongoSetting.UserCollectionName);
        }
        public void AddUser(User user)
        {
            var userr =  _users.Find(u => u.Id == Guid.Parse("af437a6f-704c-4e08-84ff-53b5a56f7b1f")).FirstOrDefault();

            Console.WriteLine(userr.Messages.Count());
            _users.InsertOne(user);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _users.FindAsync(x=>x.Username == username).Result.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserWithMessages(string username)
        {
            return await _users.FindAsync(x => x.Username == username).Result.FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserASupport(string username)
        {
            var user = await _users.FindAsync(x => x.Username == username).Result.FirstOrDefaultAsync();
            if (user.role == Domain.Model.Enums.Role.Customer)
            {
                return false;
            }
            else
                return true;
        }

        public async Task<bool> IsUserExist(string username)
        {
            return await _users.FindAsync(x => x.Username == username).Result.AnyAsync();
        }

        public async Task<List<User>> SeeAllUnreadMessages()
        {
            return await _users.FindAsync(x => x.HaveUnreadMessage == true).Result.ToListAsync();
        }

        public async Task UpdateUser(User user, Message messageToPush , bool haveRead)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, user.Username);

            var update = Builders<User>.Update
                .Push("Messages", messageToPush)           
                .Set(u => u.HaveUnreadMessage, haveRead);
            await _users.UpdateOneAsync(filter, update);


        }
    }
}
