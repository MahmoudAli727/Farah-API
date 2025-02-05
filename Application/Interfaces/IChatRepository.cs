using Application.DTOS;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat> GetChatByParticipantsAsync(string userId1, string userId2, bool isUser1Owner);
        public IEnumerable<AllChatsDTO> GetMyChats(string userId, bool isOwner);
        public Task<Chat> GetChatByIdAsync(int chatId);
        public Task MarkMessagesAsReadAsync(int chatId, string userId);
        Task AddAsync(Chat chat);
        public Chat GetChatBetweenOwnerandCustomer(string customerId,string ownerID);
        Task SaveChangesAsync();
    }
}
