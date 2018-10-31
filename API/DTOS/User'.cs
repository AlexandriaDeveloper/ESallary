using System;

namespace API.DTOS {
    public class LoginUserDto {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
    public class SignupDto {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
    }

    public class UserToReturnDto {
        public string UserName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime LastActive { get; set; }
        public string City { get; set; }
    }
}