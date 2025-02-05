using Core.Entities;

namespace Application.Interfaces
{
    public interface IUserOTPRepository
    {
        Task SaveOTPAsync(UserOTP userOTP);
        Task<UserOTP> GetOTPAsync(string email, string otp);
        Task DeleteOTPAsync(UserOTP userOTP);
        Task<ApplicationUser> FindUserByEmailAsync(string email);
    }
}
