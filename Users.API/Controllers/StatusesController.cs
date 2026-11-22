#nullable disable
using CORE.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.APP.Features.Statuses;

//Generated from Custom Microservices Template.
namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusesController : ControllerBase
    {
        private readonly ILogger<StatusesController> _logger;
        private readonly IMediator _mediator;

        public StatusesController(ILogger<StatusesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Statuses
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new StatusQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("StatusesGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during StatusesGet."));
            }
        }

        // GET: api/Statuses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new StatusQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("StatusesGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during StatusesGetById."));
            }
        }

		// POST: api/Statuses
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(StatusAddRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    ModelState.AddModelError("StatusesPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("StatusesPost Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during StatusesPost."));
            }
        }

        // PUT: api/Statuses
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(StatusUpdateRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                    {
                        return Ok(response);
                    }
                    ModelState.AddModelError("StatusesPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("StatusesPut Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during StatusesPut."));
            }
        }

        // DELETE: api/Statuses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new StatusRemoveRequest() { Id = id });
                if (response.IsSuccessful)
                {
                    return Ok(response);
                }
                ModelState.AddModelError("StatusesDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception exception)
            {
                _logger.LogError("StatusesDelete Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during StatusesDelete."));
            }
        }
	}
}
