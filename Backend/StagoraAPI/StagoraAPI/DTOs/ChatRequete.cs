namespace StagoraAPI.DTOs
{
    /// <summary>
    /// Corps de la requête POST /api/chat.
    /// Le client envoie toute la conversation, avec la nouvelle question en dernier.
    /// </summary>
    public class ChatRequete
    {
        public List<MessageDto> Messages { get; set; } = new();
    }
}
