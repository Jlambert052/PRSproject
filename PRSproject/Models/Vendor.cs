﻿using Microsoft.EntityFrameworkCore;
using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace PRSproject.Models {

    [Index("Code", IsUnique = true, Name = "CodeIndex")]
    public class Vendor {

        public int Id { get; set; }

        [StringLength(30)]
        public string Code { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Address { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(5)]
        public string Zip { get; set; }

        [StringLength(12)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }
    }
}
