using Application.DTOS;
using Application.Interfaces;

namespace Application.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IAccountRepository accountRepository;

        public GoogleAuthService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<CustomResponseDTO<AuthUserDTO>> GoogleSignIn(string model)
        {
            var result = await accountRepository.GoogleSignIn(model);
            if (result.Succeeded)
            {
                return new CustomResponseDTO<AuthUserDTO>()
                {
                    Data = result,
                    Succeeded = true,
                    Message = result.Message
                };
            }
            else
            {
                return new CustomResponseDTO<AuthUserDTO>()
                {
                    Data = null,
                    Succeeded = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
        }

    }
}
