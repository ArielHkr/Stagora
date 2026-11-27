// Réglages de l'application, regroupés à un seul endroit.

/** Adresse de l'API. Vide en développement : les appels passent par le proxy de Vite. */
export const API_BASE_URL = (import.meta.env.VITE_API_URL ?? '').replace(/\/$/, '')

/** Nom affiché dans l'interface. */
export const NOM_EGLISE = 'Église Nouvelle Espérance'

/** Longueur maximale d'un message saisi par le visiteur. */
export const LONGUEUR_MAX_MESSAGE = 1000
