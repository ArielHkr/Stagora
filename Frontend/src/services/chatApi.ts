// Seul fichier qui sait comment parler au backend.
// Les composants n'appellent jamais fetch directement : ils passent par ici.

import { API_BASE_URL } from '../config'
import type { ChatReponse, ChatRequete } from '../types/chat'

/** Erreur d'appel à l'API, avec un message déjà prêt à afficher au visiteur. */
export class ChatApiError extends Error {
  readonly status: number | null

  constructor(message: string, status: number | null) {
    super(message)
    this.name = 'ChatApiError'
    this.status = status
  }
}

function messagePourStatut(status: number): string {
  if (status === 400) return "Votre message n'a pas pu être traité. Reformulez-le et réessayez."
  if (status === 429) return 'Beaucoup de demandes en ce moment. Patientez quelques secondes puis réessayez.'
  if (status >= 500) return "L'assistant rencontre un problème. Réessayez dans un instant."
  return `Erreur inattendue (code ${status}). Réessayez dans un instant.`
}

/**
 * Envoie la conversation à POST /api/chat et renvoie le texte de la réponse.
 * Lève ChatApiError en cas d'échec, ou une AbortError si l'appel est annulé.
 */
export async function envoyerConversation(requete: ChatRequete, signal?: AbortSignal): Promise<string> {
  let reponse: Response

  try {
    reponse = await fetch(`${API_BASE_URL}/api/chat`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(requete),
      signal,
    })
  } catch (erreur) {
    if (erreur instanceof DOMException && erreur.name === 'AbortError') throw erreur
    throw new ChatApiError('Impossible de joindre le serveur. Vérifiez votre connexion et réessayez.', null)
  }

  if (!reponse.ok) {
    throw new ChatApiError(messagePourStatut(reponse.status), reponse.status)
  }

  const donnees = (await reponse.json()) as ChatReponse
  return donnees.reponse
}
