# Stagora — Front-end (Vue 3 + TypeScript + Vite)

Page d'accueil de l'église avec une bulle de chat qui appelle l'API `StagoraAPI`.

## Lancer en développement

1. Démarre le backend (`StagoraAPI`, F5 dans Visual Studio). Il écoute sur `http://localhost:5226`.
2. Dans ce dossier :

```powershell
npm install
npm run dev
```

3. Ouvre http://localhost:5173.

Les appels à `/api` sont relayés vers le backend par le proxy de Vite (voir `vite.config.ts`).

## Structure

```
src/
├── main.ts                  Point d'entrée : crée l'application Vue
├── App.vue                  Assemble la page : en-tête, accueil, pied de page, chat
├── config.ts                Réglages (adresse de l'API, nom de l'église, limites)
├── env.d.ts                 Types des variables d'environnement
├── types/chat.ts            Types qui reflètent les DTOs du backend
├── services/chatApi.ts      Seul endroit qui appelle l'API (fetch + gestion des erreurs)
├── composables/useChat.ts   État de la conversation : messages, envoi, erreurs
├── utils/formatTexte.ts     Affiche le **gras** sans v-html
├── styles/main.css          Couleurs et styles globaux
└── components/
    ├── layout/              AppHeader, AppFooter
    ├── home/                HeroSection (accueil + questions suggérées)
    └── chat/                ChatWidget, ChatMessageList, ChatMessageItem, ChatInput, TypingIndicator
```

## Production

```powershell
npm run build
```

Le résultat est dans `dist/`. Définis `VITE_API_URL` (voir `.env.example`) avec l'adresse de l'API,
et ajoute l'adresse du site dans `Cors:AllowedOrigins` du backend.
