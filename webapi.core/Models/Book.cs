using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public class Book : BaseEntity
    {
        public String Title { get; set; }
        public DateTime Year { get; set; }

        public Book() { }
        public Book(String title) { Title = title; }
    }
}
