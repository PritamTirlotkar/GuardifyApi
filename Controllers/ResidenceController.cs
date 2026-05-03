using Microsoft.AspNetCore.Mvc;
using GuardifyApi.Data;
using GuardifyApi.Models;
using GuardifyApi.DTOs;
using System.Linq;

namespace GuardifyApi.Controllers
{
    [ApiController]
    [Route("api/residence")]
    public class ResidenceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResidenceController(AppDbContext context)
        {
            _context = context;
        }

        // 🏗️ CREATE RESIDENCE
        [HttpPost("create")]
        public IActionResult CreateResidence([FromBody] CreateResidenceDto request)
        {
            var residence = new Residence
            {
                SocietyId = request.SocietyId,
                BuildingName = request.BuildingName,
                Wing = request.Wing,
                FlatNumber = request.FlatNumber,
                Floor = request.Floor,
                AddressLine = request.AddressLine
            };

            _context.Residences.Add(residence);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Residence created successfully",
                residenceId = residence.Id
            });
        }

        // 🏢 GET ALL RESIDENCES BY SOCIETY
        [HttpGet("by-society/{societyId}")]
        public IActionResult GetBySociety(int societyId)
        {
            var residences = _context.Residences
                .Where(x => x.SocietyId == societyId)
                .ToList();

            return Ok(residences);
        }

        // 🏠 GET SINGLE RESIDENCE
        [HttpGet("{id}")]
        public IActionResult GetResidence(int id)
        {
            var residence = _context.Residences
                .FirstOrDefault(x => x.Id == id);

            if (residence == null)
                return NotFound("Residence not found");

            return Ok(residence);
        }

        // 👤 ASSIGN USER TO RESIDENCE
        [HttpPost("assign-user")]
        public IActionResult AssignUser(int userId, int residenceId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return NotFound("User not found");

            var residence = _context.Residences.FirstOrDefault(x => x.Id == residenceId);
            if (residence == null)
                return NotFound("Residence not found");

            user.ResidenceId = residenceId;

            _context.SaveChanges();

            return Ok(new
            {
                message = "User assigned to residence successfully"
            });
        }
    }
}