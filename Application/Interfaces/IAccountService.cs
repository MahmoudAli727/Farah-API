using Application.DTOS;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        public AllServicesDTO GetOwnerServices(string ownerID);
        public Task<CustomResponseDTO<CustomerAccountInfoDTO>> GetCustomerInfo(string email);
        public Task<CustomResponseDTO<CustomerAccountInfoDTO>> UpdateCustomerInfo(CustomerAccountInfoDTO infoDTO, string email);
        public Task<CustomResponseDTO<AuthUserDTO>> OwnerRegisterAsync(OwnerRegisterDTO RegisterModel);
        public Task<CustomResponseDTO<AuthUserDTO>> CustomerRegisterAsync(CustomerRegisterDTO RegisterModel);
        public Task<CustomResponseDTO<AuthUserDTO>> Login(LoginUserDTO loginUser);
        public Task<CustomResponseDTO<bool>> ChangePasswordAsync(ChangePasswordDTO changePasswordModel, string userId);
        public Task<CustomResponseDTO<AuthUserDTO>> ConfirmEmailAsync(string email, string otp);
        public Task<CustomResponseDTO<string>> SendNewOTPAsync(string email);
        public Task<CustomResponseDTO<string>> ForgetPassword(string Email);
        public Task<CustomResponseDTO<OwnerAccountInfoDTO>> GetOwnerInfo(string Email);
        public Task<CustomResponseDTO<OwnerAccountInfoDTO>> UpdateOwnerInfo(OwnerAccountInfoDTO infoDTO, string Emai);


    }
}
