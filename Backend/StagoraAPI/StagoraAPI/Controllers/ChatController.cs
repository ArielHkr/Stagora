using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StagoraAPI.DTOs;
using StagoraAPI.Services;                                
namespace StagoraAPI.Controllers
{
    /// <summary>
    /// Point d'entrée HTTP du chatbot.                     
    /// Reçoit la conversation et la transmet à BotService.
    /// </summary>
    [Route("api/chat")]
    [ApiController]
    [EnableRateLimiting("Chat")] // Politique définie dans Program.cs (PolitiqueChat)
    public class ChatController : ControllerBase
    {
        private readonly BotService _botService;            

        public ChatController(BotService botService)        
        {
            _botService = botService;
        }

        /// <summary>
        /// Reçoit la conversation et renvoie la réponse du bot.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ChatReponse>> Post(ChatRequete requete, CancellationToken ct)
        {
            // 1. Valider : une conversation vide n'a pas de question à traiter.
            if (requete.Messages.Count == 0)
            {
                return BadRequest("La conversation est vide.");
            }

            // 2. Demander la réponse au bot.               
            string texte = await _botService.RepondreAsync(requete.Messages, ct);

            return Ok(new ChatReponse { Reponse = texte });
        }
    }
}