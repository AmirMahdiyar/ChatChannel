using ChatChannel.Application.Dtos;
using ChatChannel.Application.Exception;
using ChatChannel.Domain.Model.Contracts;
using ChatChannel.Domain.Model.Enums;
using ChatChannel.Infraustructure.Substructure.Base.ApplicationException;
using ChatChannel.Infraustructure.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
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

        public ChatHub(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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
                    _userRepository.AddUser(username.ToSupportUser());
                    savingResult = await _unitOfWork.Commit(CancellationToken.None);
                    savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();
                }

                else
                {
                    _userRepository.AddUser(username.ToCustomerUser());
                    savingResult = await _unitOfWork.Commit(CancellationToken.None);
                    savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();
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
            var savingResult = new SavingResult();
            savingResult = await _unitOfWork.Commit(CancellationToken.None);
            savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();

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
                var messagesText = string.Join(" | ", userWithUnreadMessage.Content.Select(x => $"{x.Content} Date: {x.SentDate:yyyy-MM-dd HH:mm}"));

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
            var savingResult = new SavingResult();
            savingResult = await _unitOfWork.Commit(CancellationToken.None);
            savingResult.ThrowIfNoChanges<NoChangedHappenedApplicationException>();

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
                    ("Messages", $"username : {message.Username} - Content : {message.Content} - DateOfSent : {message.SentDate}");
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
        #endregion
    }
}
