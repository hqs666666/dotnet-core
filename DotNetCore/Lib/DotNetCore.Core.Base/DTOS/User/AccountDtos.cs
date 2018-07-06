

namespace DotNetCore.Core.Base.DTOS.User
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Repass { get; set; }
        public string Code { get; set; }
        public string Agreement { get; set; }
        public string NickName { get; set; }
        public bool IsVaild
        {
            get
            {
                if (string.IsNullOrEmpty(UserName) ||
                    string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(Code))
                    return false;
                if (Password != Repass || Agreement != "on")
                    return false;
                return true;
            }
        }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
