using Microsoft.AspNetCore.Mvc;
using GuardifyApi.Data;
using GuardifyApi.Models;
using GuardifyApi.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GuardifyApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/society")]
    public class SocietyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SocietyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateSociety([FromBody] CreateSocietyDto request)
        {

            // 1. GET ROLE FROM JWT
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "SuperAdmin")
                return Forbid("Only SuperAdmin can create society");

            // 2. CREATE SOCIETY
            var society = new Society
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City
            };

            _context.Societies.Add(society);
            _context.SaveChanges();

            // 3. RETURN SOCIETY ID
            return Ok(new
            {
                message = "Society created successfully",
                societyId = society.Id
            });
        }
    }
}