using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Domain.User
{
    public class Role : BaseEntity<string>
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string DisplayName { get; set; }
    }
}