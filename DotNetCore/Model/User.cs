using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Model
{
    public class User
    {
        [Key]
        [MaxLength(40)]
        public string Id { get; set; }
        [MaxLength(20)][Required]
        public string UserName { get; set; }
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [MaxLength(30)]
        public string Email { get; set; }
    }
}
