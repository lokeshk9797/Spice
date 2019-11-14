﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CouponController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task< IActionResult> Index()
        {
            return View(await _db.Coupon.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Coupon coupon)
        {
            if(!ModelState.IsValid)
            {

            }
            var files = HttpContext.Request.Form.Files;
            if(files.Count>0)
            {
                byte[] picture = null;
                using (var fs1= files[0].OpenReadStream())
                {
                    using (var ms1= new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        picture = ms1.ToArray();
                    }
                }

                coupon.Picture = picture;
            }

            _db.Coupon.Add(coupon);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}