using ExpressFood.FoodApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressFood.FoodApp.Core.Entities
{
    public class ApplicationUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public ApplicationUserType Type { get; set; }
        public string EmailAddress { get; set; }
        public bool Verified { get; set; } = false;
        public string VerificationCode { get; set; }
    }
}
