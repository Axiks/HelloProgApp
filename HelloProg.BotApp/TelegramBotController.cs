using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace HelloProg.BotApp
{
    [Route("api/message")]
    [ApiController]
    public class TelegramBotController : ControllerBase
    {
        [HttpPost("update")]
        public IActionResult Update(Update update)
        {
            // /start => register user
            return Ok();
        }
    }
}
