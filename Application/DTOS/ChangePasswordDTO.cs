namespace Application.DTOS
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string SecurityQuestionAnswer { get; set; }
    }
}
