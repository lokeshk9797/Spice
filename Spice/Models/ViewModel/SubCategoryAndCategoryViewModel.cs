using System.Collections.Generic;

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
