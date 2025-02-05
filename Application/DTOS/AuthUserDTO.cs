namespace Application.DTOS
{
    public class AuthUserDTO
    {
        public int NotSeenServicesCount { get; set; }
        public int NotSeenRegisteredOwners { get; set; }
        public int NotSeenMessages { get; set; }
        public string Message { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool Succeeded { get; set; } = false;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string AccountStatus { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime ExpireTIme { get; set; }
        public List<string> Errors { get; set; }
        public string FullName { get; set; }

        public string? ProfileImage { get; set; }
    }
}
