using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;


namespace Application.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IMapper _mapper;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public ChatMessageService(IMapper mapper, IChatMessageRepository chatMessageRepository, IChatRepository chatRepository, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _chatMessageRepository = chatMessageRepository;
            _chatRepository = chatRepository;
            _userManager = userManager;
        }

        public CustomResponseDTO<List<AllChatsDTO>> GetMyChats(int page, int pageSize, string userId, bool isOwner)
        {
            try
            {
                IEnumerable<AllChatsDTO> myChats = _chatRepository.GetMyChats(userId, isOwner);

                var paginatedList = PaginationHelper.Paginate(myChats, page, pageSize);
                var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

                return new CustomResponseDTO<List<AllChatsDTO>>
                {
                    Data = paginatedList.Items,
                    Message = "تمااااااااااام",
                    Succeeded = true,
                    PaginationInfo = paginationInfo
                };
            }
            catch (Exception ex)
            {

                return new CustomResponseDTO<List<AllChatsDTO>>
                {
                    Data = null,
                    Message = $"ايرور يا معلم ابلع: {ex.Message}",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ChatDetailsDTO> GetChatByIdAsync(int chatId, string userId, bool isOwner)
        {
            try
            {
                var chat = await _chatRepository.GetChatByIdAsync(chatId);

                if (chat == null || (isOwner && chat.OwnerId != userId) || (!isOwner && chat.CustomerId != userId))
                {
                    throw new Exception("Chat not found or user does not have access to this chat.");
                }

                // Make is Readed true
                await _chatRepository.MarkMessagesAsReadAsync(chatId, userId);

                ApplicationUser user;
                if (isOwner)
                {
                    user = chat.Customer;
                }
                else
                {
                    user = chat.Owner;
                }

                var chatDetailsDTO = new ChatDetailsDTO
                {
                    ChatId = chat.Id,
                    User = new UserDTO
                    {
                        Id = user.Id,
                        UserName = $"{user.FirstName} {user.LastName}",
                        ProfileImage = user.ProfileImage
                    },
                    Messages = chat.Messages.Select(m => new MessageDTO
                    {
                        SenderId = m.SenderId,
                        ReceiverId = m.ReceiverId,
                        Message = m.Message,
                        SentAt = m.SentAt,
                        IsRead = m.IsRead
                    }).ToList()
                };

                return chatDetailsDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"ابلع ايروووووووووووور: {ex.Message}");
            }
        }

        public async Task SendMessageAsync(string senderId, string receiverId, string message, bool isSenderOwner)
        {
            var chat = await _chatRepository.GetChatByParticipantsAsync(senderId, receiverId, isSenderOwner);

            if (chat == null)
            {
                chat = new Chat
                {
                    OwnerId = isSenderOwner ? senderId : receiverId,
                    CustomerId = isSenderOwner ? receiverId : senderId,
                    CreatedAt = DateTime.Now
                };
                await _chatRepository.AddAsync(chat);
                await _chatRepository.SaveChangesAsync();
            }

            var chatMessage = new ChatMessage
            {
                ChatId = chat.Id,
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                SentAt = DateTime.Now,
                IsRead = false
            };

            await _chatMessageRepository.AddAsync(chatMessage);
            await _chatMessageRepository.SaveChangesAsync();
        }

        public async Task<ChatDetailsDTO> GetChatBetweenOwnerandCustomer(string customerId, string ownerID)
        {
            var chat = _chatRepository.GetChatBetweenOwnerandCustomer(customerId, ownerID);

            ApplicationUser customer = await _userManager.FindByIdAsync(customerId);
            ApplicationUser owner = await _userManager.FindByIdAsync(ownerID);

            if (customer is not Customer || owner is not Owner)
            {
                throw new Exception("IDS فيها حاجه غلط ");
            }

            if (chat == null)
            {
                chat = new Chat
                {
                    OwnerId = ownerID,
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow
                };
                await _chatRepository.AddAsync(chat);
                await _chatRepository.SaveChangesAsync();

                return new ChatDetailsDTO
                {
                    ChatId = chat.Id,
                    User = new UserDTO
                    {
                        Id = chat.Owner.Id,
                        UserName = $"{chat.Owner.FirstName} {chat.Owner.LastName}",
                        ProfileImage = chat.Owner.ProfileImage
                    },
                    Messages = null
                };
            }

            await _chatRepository.MarkMessagesAsReadAsync(chat.Id, customerId);
            ApplicationUser user = chat.Customer;
            var chatDetailsDTO = new ChatDetailsDTO
            {
                ChatId = chat.Id,
                User = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    ProfileImage = user.ProfileImage
                },
                Messages = chat.Messages.Select(m => new MessageDTO
                {
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Message = m.Message,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead
                }).ToList()
            };
            return chatDetailsDTO;
        }
    }
}
