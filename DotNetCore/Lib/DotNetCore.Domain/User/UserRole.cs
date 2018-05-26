using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCore.Domain.User
{
    public class UserRole
    {
        [Key]
        [MaxLength(32)]
        //[Column(TypeName = "VARCHAR")]
        public string Id { get; set; }
        [MaxLength(32)]
        [Required]
        //[Column(TypeName = "VARCHAR")]
        public string UserId { get; set; }

        [MaxLength(32)]
        [Required]
        //[Column(TypeName = "VARCHAR")]
        public string RoleId { get; set; }
    }
}