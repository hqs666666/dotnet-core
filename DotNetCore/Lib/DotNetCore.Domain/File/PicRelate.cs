 

/*****************************************************************************
 * 
 * Created On: 2018-05-31
 * Purpose:    pic_relate
 * 
 ****************************************************************************/


using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Domain.File
{
    public class PicRelate : BaseEntity<string>
    {
        [Required]
        [MaxLength(30)]
        public string TableName { get; set; }
        [Required]
        [MaxLength(32)]
        public string MasterId { get; set; }
        [Required]
        [MaxLength(32)]
        public string PicResourceId { get; set; }
        [Required]
        [MaxLength(30)]
        public string Category { get; set; }
        [Required]
        public int SortIndex { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } 
    }
}
