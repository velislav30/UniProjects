using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VT.Website.Models
{
    public class CatalogVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public String Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public long TorrentNum { get; set; }
        public DateTime LastDownloadedFrom { get; set; }

        [DisplayName("Creator")]
        public int CreatorId { get; set; }
        public UserVM Creator { get; set; }
    }
}
