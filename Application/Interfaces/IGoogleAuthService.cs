using Application.DTOS;

namespace Application.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<CustomResponseDTO<AuthUserDTO>> GoogleSignIn(string model);

    }
}
