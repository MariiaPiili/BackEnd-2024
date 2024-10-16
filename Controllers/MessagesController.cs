using BackEnd_2024_Project.Middleware;
using BackEnd_2024_Project.Models;
using BackEnd_2024_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd_2024_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserAuthenticationService _userAuthenticationService;

        public MessagesController(IMessageService service, IUserAuthenticationService authenticationService)
        {
            _messageService = service;
            _userAuthenticationService = authenticationService;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages()
        {
            return Ok(await _messageService.GetMessagesAsync());
        }
                
        [Authorize]
        [HttpGet("Received/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMyReceivedMessages(string username)
        {
            if (this.User.FindFirst(ClaimTypes.Name).Value == username)
            {
                return Ok(await _messageService.GetMyReceivedMessagesAsync(username));
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("Sent/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMySentMessages(string username)
        {
            if (this.User.FindFirst(ClaimTypes.Name).Value == username)
            {
                return Ok(await _messageService.GetMySentMessagesAsync(username));
            }
            return Unauthorized();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDTO>> GetMessage(long id)
        {
            MessageDTO? message = await _messageService.GetMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(long id, MessageDTO message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }
            bool result = await _messageService.UpdateMessageAsync(message);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(MessageDTO message)
        {
            MessageDTO? newMessage = await _messageService.NewMessageAsync(message);

            if (newMessage == null)
            {
                return Problem();
            }
            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            string username = this.User.FindFirst(ClaimTypes.Name).Value;
            if (!await _userAuthenticationService.isMyMessage(username, id))
            {
                return BadRequest();
            }
            bool result = await _messageService.DeleteMessageAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
