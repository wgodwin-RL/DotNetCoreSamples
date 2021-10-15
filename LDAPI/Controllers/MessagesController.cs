using LD_Models.Interfaces;
using LD_Models.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LD_API.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly IEventMessageData _eventMessageData;
        private readonly ILogger<MessagesController> _logger;
        public MessagesController(IEventMessageData eventMessageData, ILogger<MessagesController> logger) 
        {
            _eventMessageData = eventMessageData;
            _logger = logger;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventMessage(string id, StudentExamEventMessage message)
        {
            try
            {

                if (message.Event.ToLower() != Constants.Score.ToLower())
                    return BadRequest("invalid event type submitted.");

                await _eventMessageData.UpsertEventMessage(message);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e);
            }
        }
    }
}
