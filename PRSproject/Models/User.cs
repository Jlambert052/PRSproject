using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PRSproject.Models {

    [Index("Password", IsUnique = true, Name = "PasswordIndex")]
    public class User {

        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string Username { get; set; }

        [StringLength(30)]
        public string Password { get; set; }

        [StringLength(30)]
        public string Firstname { get; set; }

        [StringLength(30)]
        public string Lastname { get; set; }

        [StringLength(12)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        public bool IsReviewer { get; set; }

        public bool IsAdmin { get; set; }


    }
}
