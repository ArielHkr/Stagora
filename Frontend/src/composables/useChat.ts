// L'état de la conversation et les actions possibles.
// Le backend ne mémorise rien : c'est ici que l'historique est conservé,
// puis renvoyé en entier à chaque question.

import { ref } from 'vue'
import { NOM_EGLISE } from '../config'
import { ChatApiError, envoyerConversation } from '../services/chatApi'
import type { MessageAffiche, MessageDto } from '../types/chat'

const MESSAGE_ACCUEIL =
  `Bonjour ! Je suis l'assistant de l'${NOM_EGLISE}. ` +
  "Posez-moi vos questions sur les horaires, l'accès ou nos activités."

const DELAI_AVANT_MESSAGE_REVEIL_MS = 5000

function creerAccueil(): MessageAffiche {
  return { id: 0, role: 'assistant', contenu: MESSAGE_ACCUEIL, local: true }
}

export function useChat() {
  const messages = ref<MessageAffiche[]>([creerAccueil()])
  const enCours = ref(false)
  const erreur = ref<string | null>(null)
  /** true si la réponse tarde : l'API est sans doute en train de se réveiller. */
  const lent = ref(false)

  let prochainId = 1
  let controleur: AbortController | null = null

  /** L'historique à envoyer : sans le message d'accueil, qui n'existe que dans le navigateur. */
  function historiquePourApi(): MessageDto[] {
    return messages.value
      .filter((m) => !m.local)
      .map((m) => ({ role: m.role, contenu: m.contenu }))
  }

  async function demanderReponse(): Promise<void> {
    enCours.value = true
    erreur.value = null
    controleur = new AbortController()
    // Après 5 s sans réponse, on prévient le visiteur que l'API se réveille.
    const minuterie = setTimeout(() => (lent.value = true), DELAI_AVANT_MESSAGE_REVEIL_MS)

    try {
      const texte = await envoyerConversation({ messages: historiquePourApi() }, controleur.signal)
      messages.value.push({ id: prochainId++, role: 'assistant', contenu: texte })
    } catch (e) {
      if (e instanceof DOMException && e.name === 'AbortError') return
      erreur.value = e instanceof ChatApiError ? e.message : 'Une erreur inattendue est survenue.'
    } finally {
      clearTimeout(minuterie)
      lent.value = false
      enCours.value = false
      controleur = null
    }
  }

  /** Ajoute la question du visiteur et demande la réponse. */
  async function envoyer(texte: string): Promise<void> {
    const contenu = texte.trim()
    if (!contenu || enCours.value) return

    messages.value.push({ id: prochainId++, role: 'user', contenu })
    await demanderReponse()
  }

  /** Redemande une réponse à la dernière question, après une erreur. */
  async function reessayer(): Promise<void> {
    const dernier = messages.value[messages.value.length - 1]
    if (enCours.value || dernier?.role !== 'user') return
    await demanderReponse()
  }

  /** Efface la conversation et annule l'appel en cours. */
  function reinitialiser(): void {
    controleur?.abort()
    messages.value = [creerAccueil()]
    erreur.value = null
    enCours.value = false
  }

  return { messages, enCours, lent, erreur, envoyer, reessayer, reinitialiser }
}
