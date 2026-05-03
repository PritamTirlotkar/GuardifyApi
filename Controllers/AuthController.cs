using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuardifyApi.DTOs;
using GuardifyApi.Data;
using GuardifyApi.Models;
using GuardifyApi.Helpers;
using System;
using System.Security.Claims;

namespace GuardifyApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto request)
        {
            // 1. FIND USER (email OR mobile)
            var user = _context.Users.FirstOrDefault(x =>
                x.Email == request.Identifier ||
                x.Mobile == request.Identifier
            );

            if (user == null)
                return Unauthorized("Invalid credentials");

            // 2. VERIFY PASSWORD
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash
            );

            if (!isValidPassword)
                return Unauthorized("Invalid credentials");

            // 3. GENERATE JWT TOKEN
            var token = JwtHelper.GenerateToken(user, _config);

            // 4. RESPONSE TO FLUTTER
            return Ok(new
            {
                token,
                role = user.Role,
                userId = user.Id,
                societyId = user.SocietyId
            });
        }


        [HttpPost("create-user")]
        public IActionResult CreateUser([FromBody] CreateUserDto request)
        {
            // 1. GET LOGGED-IN USER ROLE FROM JWT
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleClaim))
                return Unauthorized("Invalid token");

            // 2. ROLE-BASED RULES
            if (roleClaim == "SuperAdmin")
            {
                // SuperAdmin can only create Admin
                //if (request.Role != "Admin")
                //    return Forbid("SuperAdmin can only create Admin");
            }
            else if (roleClaim == "Admin")
            {
                // Admin can only create Resident or Guard
                if (request.Role != "Resident" && request.Role != "Guard")
                    return Forbid("Admin can only create Resident or Guard");
            }
            else
            {
                return Forbid("You are not allowed to create users");
            }

            // 3. CHECK EXISTING USER
            var existingUser = _context.Users.FirstOrDefault(x =>
                x.Email == request.Email ||
                x.Mobile == request.Mobile
            );

            if (existingUser != null)
                return BadRequest("User already exists");

            // 4. HASH PASSWORD
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var inviteToken = Guid.NewGuid().ToString();

            // 5. CREATE USER
            var user = new AppUser
            {
                Name = request.Name,
                Email = request.Email,
                Mobile = request.Mobile,
                Role = request.Role,
                SocietyId = request.SocietyId,

                PasswordHash = null,
                IsActive = false,

                InviteToken = inviteToken,
                InviteExpiry = DateTime.UtcNow.AddDays(2)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                message = "User invited successfully",
                userId = user.Id,
                inviteLink = $"http://localhost:4200/set-password?token={inviteToken}"
            });
        }


        [HttpPost("set-password")]
        public IActionResult SetPassword([FromBody] SetPasswordDto request)
        {
            // 1. FIND USER BY INVITE TOKEN
            var user = _context.Users.FirstOrDefault(x =>
                x.InviteToken == request.Token &&
                x.InviteExpiry > DateTime.UtcNow
            );

            if (user == null)
                return BadRequest("Invalid or expired invite link");

            // 2. SET PASSWORD (HASH IT)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. ACTIVATE USER
            user.IsActive = true;

            // 4. CLEAR INVITE DATA (IMPORTANT SECURITY STEP)
            user.InviteToken = null;
            user.InviteExpiry = null;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Password set successfully. You can now login."
            });
        }
    }
}
