using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCore.Domain.Log
{
    public class Log
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Ctime { get; set; }

        [MaxLength(32)]
        [Required]
        public string CreatedBy { get; set; }
        [MaxLength(20)]
        [Required]
        public string LogType { get; set; }
        [MaxLength(1024)]
        [Required]
        public string Url { get; set; }
        [MaxLength(30)]
        [Required]
        public string Ip { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
