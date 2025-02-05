using Microsoft.AspNetCore.Http;

namespace Application.DTOS
{
    public class OwnerAccountInfoDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? SSN { get; set; }
        public int? GovID { get; set; }
        public int? CityID { get; set; }
        public string? YourFavirotePerson { get; set; }
        public string? ProfileImage { get; set; }
        public IFormFile? SetNewProfileImage { get; set; }
    }
}
