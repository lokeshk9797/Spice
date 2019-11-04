using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModel;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public MenuItemViewModel  MenuItemVM { get; set; }

        public MenuItemController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
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
            var menuItems = await _db.MenuItem.Include(s=>s.Category).Include(s=>s.SubCategory).ToListAsync();
            return View(menuItems);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(MenuItemVM); 
        }
    }
}