using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPSS.BusinessObject.Models
{
    public class EmailVerification
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Required]
        [StringLength(320)]
        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string CodeHash { get; set; }        // hash của OTP

        [Required]
        [StringLength(64)]
        public string Salt { get; set; }            // salt (nếu dùng)

        public DateTimeOffset ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public int Attempts { get; set; }          // số lần thử mã sai

        public DateTimeOffset CreatedAt { get; set; }
    }
}
