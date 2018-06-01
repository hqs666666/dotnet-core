using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Domain.User
{
    public class User: BaseEntity<string>
    {
        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }

        [MaxLength(50)]
        [Required]
        public string NickName { get; set; }

        [MaxLength(256)]
        [DataType(DataType.Password)]
        [Required]
        public string PasswordHash { get; set; }

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
    }
}
