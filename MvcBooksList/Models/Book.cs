using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBooksList.Models
{
    public class Book
    {
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Publisher { get; set; }
        public bool IsActive { get; set; }
        public double? Price { get; set; }
        public int Pages { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
