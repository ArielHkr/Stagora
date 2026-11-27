namespace StagoraAPI.DTOs
{
    /// <summary>
    /// Un message de la conversation, envoyé par le client.
    /// </summary>
    public class MessageDto
    {
        /// <summary>Qui parle : "user" (le visiteur) ou "assistant" (le bot).</summary>
        public string Role { get; set; } = "";

        /// <summary>Le texte du message.</summary>
        public string Contenu { get; set; } = "";
    }
}
