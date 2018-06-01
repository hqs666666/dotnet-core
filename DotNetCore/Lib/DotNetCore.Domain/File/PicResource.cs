 


/*****************************************************************************
 * 
 * Created On: 2018-05-31
 * Purpose:   pic_resource 
 * 
 ****************************************************************************/


using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Domain.File
{
    public class PicResource : BaseEntity<string>
    {
        [Required]
        [MaxLength(10)]
        public string PicType { get; set; }
        [Required]
        [MaxLength(32)]
        public string PicName { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public decimal PicSize { get; set; }
        [Required]
        [MaxLength(256)]
        public string PicUrl { get; set; } 
    }
}
