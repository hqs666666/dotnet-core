using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Domain.User
{
    public class UserProfile : BaseEntity<string>
    {
        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }

        [MaxLength(50)]
        [Required]
        public string NickName { get; set; }

        [MaxLength(11)]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public int UserType { get; set; }

        [Required]
        [DefaultValue(2)]
        public int Gender { get; set; }

        [MaxLength(32)]
        public string HeadImgUrl { get; set; }
    }
}
