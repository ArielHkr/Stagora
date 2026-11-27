// Types qui reflètent les DTOs du backend (StagoraAPI/DTOs).
// ASP.NET convertit les noms en camelCase dans le JSON : Contenu → contenu.

export type Role = 'user' | 'assistant'

/** MessageDto côté backend. */
export interface MessageDto {
  role: Role
  contenu: string
}

/** ChatRequete côté backend : toute la conversation, la nouvelle question en dernier. */
export interface ChatRequete {
  messages: MessageDto[]
}

/** ChatReponse côté backend. */
export interface ChatReponse {
  reponse: string
}

/** Un message tel qu'affiché dans l'interface. */
export interface MessageAffiche extends MessageDto {
  id: number
  /** true pour les messages créés dans le navigateur (accueil) : jamais envoyés à l'API. */
  local?: boolean
}
