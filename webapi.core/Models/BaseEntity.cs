using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
    }
}
