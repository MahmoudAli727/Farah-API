using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
        public Task SendMessageAsync(ChatMessage message);
        public  Task AddAsync(ChatMessage cmessage);

        public Task SaveChangesAsync();

    }
}
