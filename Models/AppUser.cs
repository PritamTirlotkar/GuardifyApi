using System;
using System.ComponentModel.DataAnnotations;

namespace GuardifyApi.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        // 👤 BASIC INFO
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? Mobile { get; set; }

        // 🔐 PASSWORD (set via invite flow, NOT by admin directly)
        public string? PasswordHash { get; set; }

        // 🎭 ROLE SYSTEM
        [Required]
        [MaxLength(20)]
        public string Role { get; set; }
        // SuperAdmin, Admin, Resident, Guard

        // 🏢 MULTI-SOCIETY SUPPORT
        public int? SocietyId { get; set; }

        // 🏠 RESIDENCE / FLAT MAPPING
        public int? ResidenceId { get; set; }

        // 👮 GUARD SHIFT (optional)
        public string? shift { get; set; }

        // =========================
        // 🔥 INVITE FLOW (ONBOARDING)
        // =========================
        public string? InviteToken { get; set; }
        public DateTime? InviteExpiry { get; set; }
        public bool IsActive { get; set; } = false;

        // =========================
        // 📲 OTP LOGIN (USED ONLY FOR LOGIN FLOW)
        // =========================
        public string? LastOtp { get; set; }
        public DateTime? OtpExpiry { get; set; }
    }
}