

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
        public string Code { get; set; }
    }
}
