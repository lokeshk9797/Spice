using System;
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

        //GET - INDEX
        public async Task< IActionResult> Index()
        {
            return View(await _db.Coupon.ToListAsync());
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Coupon coupon)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
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

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var coupon = await _db.Coupon.FindAsync(id);
            if(coupon==null)
            {
                return NotFound();
            }
            return View(coupon);

        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Coupon coupon)
        {
            if(!ModelState.IsValid)
            {
                return View(coupon);
            }
            var couponFromDB = await _db.Coupon.FindAsync(id);
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                byte[] picture = null;
                using (var fs1 = files[0].OpenReadStream())
                {
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        picture = ms1.ToArray();
                    }
                }

                coupon.Picture = picture;
            }

            couponFromDB.Name = coupon.Name;
            couponFromDB.Discount = coupon.Discount;
            couponFromDB.CouponType = coupon.CouponType;
            couponFromDB.MinimumAmount = coupon.MinimumAmount;
            couponFromDB.IsActive = coupon.IsActive;
            couponFromDB.Picture = coupon.Picture;
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

            var coupon = await _db.Coupon.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);

        }
        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _db.Coupon.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);

        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupon = await _db.Coupon.SingleOrDefaultAsync(s => s.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            _db.Coupon.Remove(coupon);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}