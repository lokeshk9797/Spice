using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViewModel
{
    public class SubCategoryAndCategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public SubCategory NewSubCategory { get; set; }
        public List<string> SubCategories { get; set; }
        public string StatusMessage { get; set; }
    }
}
