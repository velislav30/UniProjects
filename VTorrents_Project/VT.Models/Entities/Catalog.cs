using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VT.Models.Entities
{
    public class Catalog : BaseEntity
    {
        public String Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatorId { get; set; }
        public long TorrentNum { get; set; }
        public DateTime LastDownloadedFrom { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }

        public virtual ICollection<Torrent> Torrents { get; set; }
        public virtual ICollection<SubType> UploadedTorrents { get; set; }
    }
}
