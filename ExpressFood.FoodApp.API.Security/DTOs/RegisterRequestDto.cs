using ExpressFood.FoodApp.Core.Enums;

namespace ExpressFood.FoodApp.API.Security.DTOs
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string NationalCode { get; set; }
        public ApplicationUserType Type { get; set; }
        public string EmailAddress { get; set; }
        public string Cellphone { get; set; }
    }
}
