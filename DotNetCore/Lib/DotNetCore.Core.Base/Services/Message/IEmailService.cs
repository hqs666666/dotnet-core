

using DotNetCore.Core.Base.DTOS;
using DotNetCore.Core.Base.DTOS.User;

namespace DotNetCore.Core.Base.Services.Message
{
    public interface IEmailService
    {
        ResultMsg VaildEmail(UserDto user);
        ResultMsg Send(Email email);
    }
}
