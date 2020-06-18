using System;
using System.Collections.Generic;
using System.Text;

namespace VT.Business.DTOs
{
    public class BaseDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
