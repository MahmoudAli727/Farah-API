using System.ComponentModel.DataAnnotations;

namespace Application.DTOS
{
    public interface GoogleSignInDTO
    {
        [Required]
        public string IdToken { get; set; }
    }
}
