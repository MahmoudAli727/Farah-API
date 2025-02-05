using Application.DTOS;

namespace Application.Interfaces
{
    public interface IUserOTPService
    {
        public Task SaveAndSendOTPAsync(string email, string firstName, string lastName);
        public Task<bool> VerifyOTPAsync(string email, string otp);
        public Task<AuthUserDTO> SendNewOTPAsync(string email, string firstName, string lastName);
        public Task<bool> SendForgetPasswordLinkAsync(string email, string token, string firstName, string lastName);

    }
}
