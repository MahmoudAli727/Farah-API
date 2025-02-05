namespace Application.DTOS
{
    public class ForgetPasswordDTO
    {
        public bool IsUserFounded { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
