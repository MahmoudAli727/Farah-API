namespace Application.DTOS
{
    public class CustomerRegisterDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string SSN { get; set; }
        public int GovID { get; set; }
        public int CityID { get; set; }
        public string Password { get; set; }
        public string YourFavirotePerson { get; set; }
    }
}
