using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCore.Domain.User
{
    public class Role
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
        public string CreatedBy { get; set; }

        [MaxLength(32)]
        [Required]
        public string ModifiedBy { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string DisplayName { get; set; }
    }
}