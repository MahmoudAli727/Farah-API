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
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        private readonly ApplicationDBContext context;

        public ChatMessageRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            context.ChatMessages.Add(message);
            await context.SaveChangesAsync();
        }

        public async Task AddAsync(ChatMessage message)
        {
            await context.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
