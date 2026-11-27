using Microsoft.AspNetCore.Mvc;
using StagoraAPI.DTOs;

namespace StagoraAPI.Controllers
{
    /// <summary>
    /// Point d'entrée HTTP du chatbot.
    /// Version « écho » : renvoie le dernier message reçu, sans appeler l'IA.
    /// Elle sera reliée à BotService au point 1.5.
    /// </summary>
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        /// <summary>
        /// Reçoit la conversation et renvoie la réponse du bot.
        /// </summary>
        [HttpPost]
        public ActionResult<ChatReponse> Post(ChatRequete requete)
        {
            // 1. Valider : une conversation vide n'a pas de question à traiter.
            if (requete.Messages.Count == 0)
            {
                return BadRequest("La conversation est vide.");
            }

            // 2. Récupérer le dernier message (^1 = le premier en partant de la fin).
            MessageDto dernierMessage = requete.Messages[^1];

            // 3. Renvoyer la réponse (200 OK + JSON).
            //    TODO (1.5) : remplacer l'écho par l'appel à BotService.
            return Ok(new ChatReponse { Reponse = $"Écho : {dernierMessage.Contenu}" });
        }
    }
}
