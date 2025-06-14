using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VenueService.Models;
using VenueService.Repository;

namespace VenueService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueRepository _venueRepository;

        public VenueController(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateVenue([FromBody] Venue venue)
        {
            var result = await _venueRepository.CreateVenue(venue);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateVenue([FromBody] Venue venue)
        {
            var result = await _venueRepository.UpdateVenue(venue);
            return result != null ? Ok(result) : NotFound("Venue not found");
        }

        [HttpGet("{venueId}")]
        public async Task<IActionResult> GetVenueById(int venueId)
        {
            var result = await _venueRepository.GetVenueById(venueId);
            return result != null ? Ok(result) : NotFound("Venue not found");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllVenues()
        {
            var result = await _venueRepository.GetAllVenues();
            return Ok(result);
        }
    }
}