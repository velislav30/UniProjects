using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VT.Business.DTOs
{
    public class SubTypeDto : BaseDto
    {
        [MaxLength(30)]
        [Required]
        public String Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserDto Creator { get; set; }
        public CatalogDto Catalog { get; set; }
    }
}
