using Application.Helpers;
using Core.Enums;

namespace Core.Entities
{
    public class Owner : ApplicationUser
    {

        public string IDFrontImage { get; set; }
        public string IDBackImage { get; set; }

        public UserType UserType { get; set; }

        public OwnerAccountStatus AccountStatus { get; set; } = OwnerAccountStatus.Pending;
        public List<Service> Services { get; set; }

        public bool IsAdminSeen { get; set; }

    }
}
