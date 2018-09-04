using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCore.Domain.User
{
    public class User: BaseEntity<string>
    {
        [MaxLength(50)]
        [Required]
        [Column(TypeName = "text")]
        public string UserName { get; set; }

        [MaxLength(50)]
        [Required]
        [Column(TypeName = "text")]
        public string NickName { get; set; }

        [MaxLength(256)]
        [DataType(DataType.Password)]
        [Required]
        //[Column(TypeName = "varchar")]
        public string PasswordHash { get; set; }

        [MaxLength(11)]
        [DataType(DataType.PhoneNumber)]
        [Required]
        //[Column(TypeName = "varchar")]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        [Required]
        [DataType(DataType.EmailAddress)]
        //[Column(TypeName = "varchar")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int UserType { get; set; }
    }
}
