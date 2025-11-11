using ChatChannel.Application.Dtos;
using ChatChannel.Application.Exception;
using ChatChannel.Domain.Model.Contracts;
using ChatChannel.Domain.Model.Enums;
using ChatChannel.Infraustructure;
using ChatChannel.Infraustructure.MongoRepository;
using ChatChannel.Infraustructure.Repository;
using ChatChannel.Infraustructure.Substructure.Base.ApplicationException;
using ChatChannel.Infraustructure.Substructure.Utils;
using ChatChannel.Infraustructure.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ChatChannel.Infraustructure.UnitOfWork.IUnitOfWork;

namespace ChatChannel.Application.Chat
{
    public class ChatHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IsSqlOrNoSqlSetting _isSqlOrNoSqlSetting;
        private readonly IOptions<MongoSetting> _mongoSetting;
        private readonly ChatDbContext _dbContext;

        public ChatHub(IUserRepository userRepository, IUnitOfWork unitOfWork, IOptions<IsSqlOrNoSqlSetting> isSqlOrNoSqlSetting,
            IOptions<MongoSetting> mongoSetting, ChatDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _isSqlOrNoSqlSetting = isSqlOrNoSqlSetting.Value;
            _mongoSetting = mongoSetting;
            _dbContext = dbContext;
            var userRep = CheckSqlOrNoSql(_isSqlOrNoSqlSetting);
            _userRepository = userRep;
        }
        public override async Task OnConnectedAsync()
        {
            var username = GetUsername();
            var userRole = GetRole();
            var savingResult = new SavingResult();
            if (!await _userRepository.IsUserExist(username))
            {
                if (userRole == Role.Support.ToString())
                {
                    if (_isSqlOrNoSqlSetting.SqlOrNoSql)
                    {
                        _userRepository.AddUser(username.ToSupportUser());
                        savingResult = await _unitOfWork.Commit(CancellationToken.None);
                        savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();
                    }
                    else
                    {
                        _userRepository.AddUser(username.ToSupportUser());

                    }
                }

                else
                {
                    if (_isSqlOrNoSqlSetting.SqlOrNoSql)
                    {
                        _userRepository.AddUser(username.ToCustomerUser());
                        savingResult = await _unitOfWork.Commit(CancellationToken.None);
                        savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();
                    }
                    else
                        _userRepository.AddUser(username.ToCustomerUser());

                }

            }


        }

        public async Task SendMessage(string message)
        {
            var username = GetUsername();
            var userRole = GetRole();

            var user = await _userRepository.GetUserWithMessages(username);
            var targetMessage = message.ToMessage(userRole);
            user.AddMessage(targetMessage);
            user.HaveUnreadMessageToTrue();
            if (_isSqlOrNoSqlSetting.SqlOrNoSql)
            {
                var savingResult = new SavingResult();
                savingResult = await _unitOfWork.Commit(CancellationToken.None);
                savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();
            }
            else
            {
                await _userRepository.UpdateUser(user, targetMessage ,true);
            }

            await Clients.Client(Context.ConnectionId).SendAsync("MessageSaved", "Your Message Submitted Successfully");

        }

        public async Task SupportSeeAllRequests()
        {
            var user = GetUsername();
            var userRole = GetRole();
            if (userRole == "Customer")
                throw new CustomerIsNotAllowedException();
            var users = await _userRepository.SeeAllUnreadMessages();
            var unreadMessages = users.Select(x => new UserDto(x.Username, x.Id, x.Messages.Select(x => new MessageDto(user, x.Context, x.Date)).ToList()));
            foreach (var userWithUnreadMessage in unreadMessages)
            {
                var messagesText = string.Join(" | ", userWithUnreadMessage.Context.Select(x => $"{x.Context} Date: {x.SentDate:yyyy-MM-dd HH:mm}"));

                await Clients.Client(Context.ConnectionId).SendAsync(
                    "Messages", $"Username : {userWithUnreadMessage.Username} , UserId : {userWithUnreadMessage.UserId} , Content :{messagesText} ");
            }
        }
        
        public async Task SupportReplyToUser(string customerUsername, string message)
        {
            var user = GetUsername();
            var userRole = GetRole();
            if (userRole == "Customer")
                throw new CustomerIsNotAllowedException();
            var customer = await _userRepository.GetUserByUsername(customerUsername);
            var supportResponseToCustomer = message.ToMessage(userRole);
            customer.AddMessage(supportResponseToCustomer);
            customer.HaveUnreadMessageToFalse();
            if (_isSqlOrNoSqlSetting.SqlOrNoSql)
            {
                var savingResult = new SavingResult();
                savingResult = await _unitOfWork.Commit(CancellationToken.None);
                savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();
            }
            else
            {
                await _userRepository.UpdateUser(customer,supportResponseToCustomer,false);
            }

        }

        public async Task CustomerSeeHisChat()
        {
            var user = GetUsername();
            var userRole = GetRole();
            if (userRole == "Support")
                throw new SupportIsNotAllowedException();
            var customer = await _userRepository.GetUserWithMessages(user);
            var messagesList = customer.Messages.Select(x=> new MessageDto(user,x.Context,x.Date)).ToList();
            foreach (var message in messagesList)
                await Clients.Client(Context.ConnectionId).SendAsync
                    ("Messages", $"username : {message.Username} - Content : {message.Context} - DateOfSent : {message.SentDate}");
        }




        #region Private Methods
        private string GetRole()
        {
            var role = Context.User.Claims.SingleOrDefault(x => x.Type == "Role").Value;
            if (string.IsNullOrEmpty(role))
                throw new RoleNotFoundException();
            return role;
        }

        private string GetUsername()
        {
            return Context.User.Claims.SingleOrDefault(x => x.Type == "Username").Value;
        }
        private IUserRepository CheckSqlOrNoSql(IsSqlOrNoSqlSetting setting)
        {
            if (setting.SqlOrNoSql)
                return new UserRepository(_dbContext);
            else
                return new MongoUserRepository(_mongoSetting);
        }
        #endregion
    }
}
