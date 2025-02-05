using Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOS
{
    public class OwnerRegisterDTO
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "SSN is required.")]
        //[StringLength(14, MinimumLength = 14, ErrorMessage = "SSN must be exactly 14 digits long.")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public UserType UserType { get; set; }

        [Required(ErrorMessage = "Government  is required.")]
        public int GovID { get; set; }

        [Required(ErrorMessage = "City  is required.")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "PhoneNumber  is required.")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Your Favorite Person is required.")]
        [StringLength(100, ErrorMessage = "Your Favorite Person cannot be longer than 100 characters.")]
        public string YourFavirotePerson { get; set; }

        [Required(ErrorMessage = "ID Front Image File is required.")]
        public IFormFile IDFrontImageFile { get; set; }

        public string? IDFrontImage { get; set; }

        public string? IDBackImage { get; set; }

        [Required(ErrorMessage = "ID Back Image File is required.")]
        public IFormFile IDBackImageFile { get; set; }

        public string? ProfileImage { get; set; }

        [Required(ErrorMessage = "Profile Image File is required.")]
        public IFormFile ProfileImageFile { get; set; }
    }
}
