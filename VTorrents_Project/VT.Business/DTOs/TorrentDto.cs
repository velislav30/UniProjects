using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VT.Business.DTOs
{
    public class TorrentDto : BaseDto
    {
        [MaxLength(30)]
        [Required]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public int TimesDownloaded { get; set; }
        public DateTime UploadedOn { get; set; }
        public UserDto Uploader { get; set; }
        public CatalogDto Catalog { get; set; }
        public SubTypeDto SybType { get; set; }
    }
}
