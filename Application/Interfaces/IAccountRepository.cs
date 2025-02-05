using Application.DTOS;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        public Task<Customer> GetCustomerInfo(string email);
        public Task<bool> UpdateCustomerInfo(Customer customer);
        public Task<AuthUserDTO> OwnerRegisterAsync(Owner owner, OwnerRegisterDTO registerDto);
        public Task<AuthUserDTO> CustomerRegisterAsync(Customer customer, CustomerRegisterDTO RegisterModel);
        public Task<AuthUserDTO> Login(LoginUserDTO loginUser);
        public Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordModel);
        public Task<AuthUserDTO> ConfirmEmailAsync(string email, string otp);
        public Task<bool> SendNewOTPAsync(string email);
        public Task<bool> ForgetPassword(string Email);

        public Task<Owner> GetOwnerInfo(string Email);
        public Task<bool> UpdateOwnerInfo(Owner owner);
        public Task<AuthUserDTO> GoogleSignIn(string model);


    }
}
