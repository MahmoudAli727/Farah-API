using Application.DTOS;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task sendEmailAsync(EmailDTO emailDTO);
    }
}
