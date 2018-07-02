using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Domain
{
    public abstract class BaseEntity<TKey>
    {
        [Key]
        [MaxLength(32)]
        public TKey Id { get; set; }

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
    }
}
