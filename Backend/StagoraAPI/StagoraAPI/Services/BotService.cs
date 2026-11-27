using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using StagoraAPI.DTOs;
using StagoraAPI.Models;

namespace StagoraAPI.Services
{
    public class BotService
    {
        private readonly IChatClient _chatClient;
        private readonly BotOptions _options;
        private readonly string _fiche;

        public BotService(IChatClient chatClient, IOptions<BotOptions> options, IWebHostEnvironment environment)
        {
            _chatClient = chatClient;
            _options = options.Value;
            _fiche = File.ReadAllText(Path.Combine(environment.ContentRootPath, "Data", "fiche_eglise.md"));
        }

        /// <summary>
        /// Envoie la conversation au modèle, avec le prompt système et la fiche,
        /// puis renvoie le texte de la réponse.
        /// </summary>
        public async Task<string> RepondreAsync(List<MessageDto> messages, CancellationToken ct = default)
        {
            var conversation = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System,
                    $"{Prompts.SystemPrompt}\n\n<fiche>\n{_fiche}\n</fiche>")
            };

            IEnumerable<MessageDto> recents = messages.TakeLast(_options.MaxHistory);

         
            foreach (MessageDto dto in recents)
            {
                ChatRole role = dto.Role == "assistant" ? ChatRole.Assistant : ChatRole.User;
                conversation.Add(new ChatMessage(role, dto.Contenu));
            }

            var chatOptions = new ChatOptions { MaxOutputTokens = _options.MaxTokens };
            ChatResponse reponse = await _chatClient.GetResponseAsync(conversation, chatOptions, ct);

            return reponse.Text;
        }
    }
}
