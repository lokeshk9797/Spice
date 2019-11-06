using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        //GET - INDEX
        public async Task<IActionResult> Index()
        {
            var subcategories = await _db.SubCategory.Include(s => s.Category).OrderBy(s => s.Category.Name).ToListAsync();
            return View(subcategories);
        }

        //GET - CREATE 
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                Categories = await _db.Category.ToListAsync(),
                NewSubCategory = new SubCategory(),
                SubCategories = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };
            return View(model);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
                {
                    Categories = await _db.Category.ToListAsync(),
                    NewSubCategory = model.NewSubCategory,
                    SubCategories = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync()

                };
                return View(modelVM);
            }
            var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).
                                                        Where(s => s.Name == model.NewSubCategory.Name && s.Category.Id == model.NewSubCategory.CategoryId);
            if (doesSubCategoryExists.Count() > 0)
            {
                SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
                {
                    Categories = await _db.Category.ToListAsync(),
                    NewSubCategory = model.NewSubCategory,
                    SubCategories = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                    StatusMessage = "Error : Sub Category Exists Under " + doesSubCategoryExists.First().Category.Name + " Please use another Name"

                };
                return View(modelVM);
            }
            else
            {
                _db.SubCategory.Add(model.NewSubCategory);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }


        //API to GET SubCategories of Selected Category
        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            subCategories = await (from subcategory in _db.SubCategory
                                   where subcategory.CategoryId == id
                                   select subcategory).ToListAsync();

            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                Categories = await _db.Category.ToListAsync(),
                NewSubCategory = subCategory,
                SubCategories = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };
            return View(model);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryAndCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
                {
                    Categories = await _db.Category.ToListAsync(),
                    NewSubCategory = model.NewSubCategory,
                    SubCategories = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync()

                };
                return View(modelVM);
            }
            var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).
                                                        Where(s => s.Name == model.NewSubCategory.Name && s.Category.Id == model.NewSubCategory.CategoryId);
            if (doesSubCategoryExists.Count() > 0)
            {
                SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
                {
                    Categories = await _db.Category.ToListAsync(),
                    NewSubCategory = model.NewSubCategory,
                    SubCategories = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                    StatusMessage = "Error : Sub Category Exists Under " + doesSubCategoryExists.First().Category.Name + " Please use another Name"

                };
                modelVM.NewSubCategory.Id = id;
                return View(modelVM);
            }
            else
            {
                var SubCategoryFromDb = await _db.SubCategory.FindAsync(id);
                if (SubCategoryFromDb == null)
                {
                    return NotFound();
                }
                SubCategoryFromDb.Name = model.NewSubCategory.Name;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(s => s.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            return View(subCategory);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(s => s.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            return View(subCategory);

        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(s => s.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            _db.SubCategory.Remove(subCategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}