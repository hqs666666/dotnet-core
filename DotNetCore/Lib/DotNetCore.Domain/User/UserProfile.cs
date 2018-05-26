using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCore.Domain.User
{
    public class UserProfile
    {
        [Key]
        [MaxLength(32)]
        //[Column(TypeName = "VARCHAR")]
        public string Id { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Ctime { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Mtime { get; set; }

        [MaxLength(32)]
        [Required]
        //[Column(TypeName = "VARCHAR")]
        public string CreatedBy { get; set; }

        [MaxLength(32)]
        [Required]
        //[Column(TypeName = "VARCHAR")]
        public string ModifiedBy { get; set; }

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

        [MaxLength(1024)]
        public string HeadImgUrl { get; set; }
    }
}
