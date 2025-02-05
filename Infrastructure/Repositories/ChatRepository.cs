using Application.DTOS;
using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        private readonly ApplicationDBContext _context;

        public ChatRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Chat> GetChatByParticipantsAsync(string userId1, string userId2, bool isUser1Owner)
        {
            if (isUser1Owner)
            {
                return await _context.Chats
                    .FirstOrDefaultAsync(c => c.OwnerId == userId1 && c.CustomerId == userId2);
            }
            else
            {
                return await _context.Chats
                    .FirstOrDefaultAsync(c => c.OwnerId == userId2 && c.CustomerId == userId1);
            }
        }

        public async Task AddAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<AllChatsDTO> GetMyChats(string userId, bool isOwner)
        {
            var chatsQuery = isOwner ?

                _context.Chats
                    .Where(c => c.OwnerId == userId)
                    .Include(c => c.Messages)
                    .Include(c => c.Customer)
                    .Select(c => new
                    {
                        Chat = c,
                        LastMessage = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault()
                    }).ToList() :

                _context.Chats
                    .Include(c => c.Messages)
                    .Include(c => c.Owner)
                    .Where(c => c.CustomerId == userId)
                    .Select(c => new
                    {
                        Chat = c,
                        LastMessage = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault()
                    }).ToList();

            var chatsDTO = chatsQuery.Select(c => new AllChatsDTO
            {
                chatId = c.Chat.Id,
                User = isOwner ?

                    new UserDTO
                    {
                        Id = c.Chat.Customer.Id,
                        UserName = $"{c.Chat.Customer.FirstName} {c.Chat.Customer.LastName}",
                        ProfileImage = c.Chat.Customer.ProfileImage
                    } :

                    new UserDTO
                    {
                        Id = c.Chat.Owner.Id,
                        UserName = $"{c.Chat.Owner.FirstName} {c.Chat.Owner.LastName}",
                        ProfileImage = c.Chat.Owner.ProfileImage
                    },

                lastMessage = c.LastMessage?.Message,
                IamTheLastMessageSender = c.LastMessage != null && c.LastMessage.SenderId == userId ? true : false,
                lastMessageSentAt = c.LastMessage?.SentAt,
                isRead = c.LastMessage?.IsRead ?? false
            }).ToList();

            return chatsDTO;
        }
      

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .Include(c => c.Owner)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task MarkMessagesAsReadAsync(int chatId, string userId)
        {
            var unreadMessages = await _context.ChatMessages
                .Where(m => m.ChatId == chatId && m.ReceiverId == userId && !m.IsRead)
                .ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                }

                await _context.SaveChangesAsync();
            }
        }

        public Chat GetChatBetweenOwnerandCustomer(string customerId, string ownerID)
        {
            return  _context.Chats
                    .Include(c => c.Messages)
                    .Include(c => c.Owner)
                    .Include(c => c.Customer)
                    .FirstOrDefault(c => c.CustomerId == customerId && c.OwnerId == ownerID);
        }
    }
}
