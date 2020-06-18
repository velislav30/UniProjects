using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VT.Models.Entities
{
    public class SubType : BaseEntity
    {
        public String Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatorId { get; set; }
        public int CatalogId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }

        [ForeignKey("CatalogId")]
        public Catalog Catalog { get; set; }
        public virtual ICollection<Torrent> UploadedTorrents { get; set; }
    }
}
