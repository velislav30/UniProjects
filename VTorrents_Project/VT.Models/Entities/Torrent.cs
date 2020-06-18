using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VT.Models.Entities
{
    public class Torrent : BaseEntity
    {
        public int UploaderId { get; set; }
        public int CatalogId { get; set; }
        public int SubTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimesDownloaded { get; set; }
        public DateTime UploadedOn { get; set; }

        [ForeignKey("UploaderId")]
        public User Uploader { get; set; }

        [ForeignKey("CatalogId")]
        public Catalog Catalog { get; set; }

        [ForeignKey("SubTypeId")]
        public SubType SybType { get; set; }
        public virtual ICollection<UserToTorrent> Downloads { get; set; }
    }
}
