using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VT.Business.DTOs
{
    public class CatalogDto : BaseDto
    {
        [MaxLength(30)]
        [Required]
        public String Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public long TorrentNum { get; set; }
        public DateTime LastDownloadedFrom { get; set; }
        public UserDto Creator { get; set; }
    }
}
