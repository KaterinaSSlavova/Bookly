using AutoMapper;
using Bookly.ViewModels;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Business_logic.DTOs;

namespace Bookly.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class APIBookPlannerController : ControllerBase
    {
        private readonly IShelfServices _shelfServices;
        private readonly IMapper _mapper;
        public APIBookPlannerController(IShelfServices shelfServices, IMapper mapper)
        {
            _shelfServices = shelfServices; 
            _mapper = mapper;
        }

        [HttpPost("mark-as-completed")]
        public IActionResult MarkBookAsRead([FromBody] PlannerBookDTO dto)
        {
            try
            {
                PlannerBook plannerBook = _mapper.Map<PlannerBook>(dto);
                _shelfServices.MoveBookToHaveRead(plannerBook);
                return Ok("Book moved to 'Have Read' shelf.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
