using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModel;
using Spice.Utility;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Categories = _db.Category,
                MenuItem = new MenuItem()
            };
        }

        //GET - INDEX
        public async Task<IActionResult> Index()
        {
            var menuItems = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).ToListAsync();
            return View(menuItems);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(MenuItemVM);
        }

        //POST - CREATE
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }
            _db.MenuItem.Add(MenuItemVM.MenuItem);
            await _db.SaveChangesAsync();

            //saving the Image and its path in database

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDB = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count > 0)
            {
                //file has been uploaded by user
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                menuItemFromDB.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension;
            }
            else
            {
                //No file has been Uploaded
                var uploads = Path.Combine(webRootPath, @"images\" + StaticDetail.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + MenuItemVM.MenuItem.Id + ".png");
                menuItemFromDB.Image = @"\images\" + MenuItemVM.MenuItem.Id + ".png";
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).SingleOrDefaultAsync(s => s.Id == id);
            MenuItemVM.SubCategories = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }
            return View(MenuItemVM);
        }

        //POST - EDIT
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                MenuItemVM.SubCategories = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();
                return View(MenuItemVM);
            }
            
            //saving the Image and its path in database

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDB = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagepath = Path.Combine(webRootPath, menuItemFromDB.Image.TrimStart('\\'));
                if(System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }

                //We will upload the new file
                using (var fileStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                menuItemFromDB.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension_new;
            }

            menuItemFromDB.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDB.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDB.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDB.Spicyness= MenuItemVM.MenuItem.Spicyness;
            menuItemFromDB.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;
            menuItemFromDB.CategoryId = MenuItemVM.MenuItem.CategoryId;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = await _db.MenuItem.Include(s => s.Category).Include(s => s.SubCategory).SingleOrDefaultAsync(s => s.Id == id);
            MenuItemVM.SubCategories = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }
            return View(MenuItemVM);
        }
    }
}