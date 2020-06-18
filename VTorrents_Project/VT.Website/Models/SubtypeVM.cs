using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VT.Website.Models
{
    public class SubtypeVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public String Title { get; set; }
        public DateTime CreatedOn { get; set; }

        [DisplayName("Creator")]
        public int CreatorId { get; set; }
        public UserVM Creator { get; set; }

        [DisplayName("Catalog")]
        public int CatalogId { get; set; }
        public CatalogVM Catalog { get; set; }
    }
}
