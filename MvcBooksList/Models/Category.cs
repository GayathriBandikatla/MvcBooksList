using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBooksList.Models
{
    public class Category
    {
        public string  CategoryName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
