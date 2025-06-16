using EventService.Models;
using EventService.Producer;
using EventService.Repositorty;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly EventMessageProducer _eventMessageProducer;


        public EventController(IEventRepository eventRepository, EventMessageProducer eventMessageProducer)
        {
            _eventRepository = eventRepository;
            _eventMessageProducer = eventMessageProducer;

        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEvent([FromBody] Event concertEvent)
        {
            var result = await _eventRepository.UpdateEvent(concertEvent);
            return result != null ? Ok(result) : NotFound("Event not found");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _eventRepository.GetAllEvents();
            return Ok(result);
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventById(int eventId)
        {
            var result = await _eventRepository.GetEventById(eventId);
            return result != null ? Ok(result) : NotFound("Event not found");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] Event concertEvent)
        {
            var result = await _eventRepository.CreateEvent(concertEvent);
            _eventMessageProducer.PublishMessage($"Event '{concertEvent.Name}' scheduled at {concertEvent.Venue} on {concertEvent.Date}");
            return Ok(result);
        }


    }
}
