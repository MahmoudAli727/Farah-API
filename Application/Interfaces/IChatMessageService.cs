using Application.DTOS;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IChatMessageService
    {
        public Task SendMessageAsync(string senderId, string receiverId, string message, bool isSenderOwner);
        public CustomResponseDTO<List<AllChatsDTO>> GetMyChats(int page, int pageSize,string userId,bool isOwner);

        public Task<ChatDetailsDTO> GetChatByIdAsync(int chatId, string userId, bool isOwner);
        public Task<ChatDetailsDTO> GetChatBetweenOwnerandCustomer(string customerId,string ownerID);
    }
}
