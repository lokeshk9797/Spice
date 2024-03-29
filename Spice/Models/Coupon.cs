﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string CouponType { get; set; }

        public enum ECouponType { Percent = 0, Rupees = 1 }

        [Required]
        public double Discount { get; set; }

        [Display(Name ="Minimum Amount")]
        [Required]
        public double MinimumAmount { get; set; }

        [Required]
        public byte[] Picture { get; set; }
        public bool IsActive { get; set; }
    }
}
