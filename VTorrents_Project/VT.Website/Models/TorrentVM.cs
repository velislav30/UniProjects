using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VT.Website.Models
{
    public class TorrentVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public int TimesDownloaded { get; set; }
        public DateTime UploadedOn { get; set; }

        [DisplayName("Uploader")]
        public int UploaderId { get; set; }
        public UserVM Uploader { get; set; }

        [DisplayName("Catalog")]
        public int CatalogId { get; set; }
        public CatalogVM Catalog { get; set; }

        [DisplayName("Subtype")]
        public int SubtypeId { get; set; }
        public SubtypeVM SybType { get; set; }
    }
}
