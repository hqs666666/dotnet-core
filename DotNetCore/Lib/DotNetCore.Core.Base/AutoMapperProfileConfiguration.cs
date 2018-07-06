


/*****************************************************************************
 * 
 * Created On: 2018-07-03
 * Purpose:    AutoMapper Configure
 * 
 ****************************************************************************/

using AutoMapper;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Domain.User;

namespace DotNetCore.Core.Base
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
