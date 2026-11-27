# Stagora

« Accueil », l'assistant IA d'une église, développé en C# / .NET et Vue.

**🌐 Démo en ligne : https://stagora-site.onrender.com/**

> L'église présentée est **fictive** (données de démonstration).
> L'API est hébergée sur l'offre gratuite de Render : après 15 min sans visite, elle se met en veille
> et la première réponse peut prendre jusqu'à une minute.

## Ce que fait le bot

- Répond aux questions des visiteurs (horaires, accès, activités) **uniquement** à partir de la fiche de l'église.
- Dit clairement quand il ne sait pas et redirige vers le secrétariat, **sans inventer**.
- Oriente vers le pasteur pour les sujets personnels ou délicats (deuil, prière, détresse).
- Refuse poliment les questions hors sujet.

## Stack

| Partie | Technologies |
|---|---|
| Backend | .NET 8, ASP.NET Core (contrôleurs), `Microsoft.Extensions.AI` (`IChatClient`) |
| Modèle d'IA | Groq (`openai/gpt-oss-120b`), via son API compatible OpenAI |
| Front-end | Vue 3, TypeScript, Vite |
| Hébergement | Render : API en Docker, front en site statique (`render.yaml`) |

## Structure

```
Backend/StagoraAPI/StagoraAPI/
├── Controllers/ChatController.cs   POST /api/chat
├── Services/BotService.cs          Assemble prompt + fiche + historique, appelle le modèle
├── Services/Prompts.cs             Règles de comportement du bot
├── Data/fiche_eglise.md            Seule source d'information du bot
├── DTOs/, Models/BotOptions.cs     Format de l'API et réglages
└── Dockerfile
Frontend/                           Page d'accueil + bulle de chat (voir Frontend/README.md)
render.yaml, DEPLOIEMENT.md         Mise en ligne sur Render
```

## Lancer en local

1. Ajoute ta clé Groq dans les secrets utilisateur du projet `StagoraAPI` :
   ```json
   { "Groq": { "ApiKey": "gsk_..." } }
   ```
2. Lance l'API (F5 dans Visual Studio, ou `dotnet run`) : http://localhost:5226/swagger
3. Lance le front :
   ```powershell
   cd Frontend
   npm install
   npm run dev
   ```

## Sécurité

- Les clés ne sont jamais dans le dépôt : secrets utilisateur en local, variables d'environnement en production.
- Limite de 10 messages par minute et par visiteur (protection du quota).
- CORS limité à l'adresse du site.
- Les réponses sont affichées sans `v-html` (pas d'injection HTML).

## Feuille de route

- [x] Bot qui répond à partir d'une fiche
- [x] Interface web et mise en ligne
- [ ] Base de connaissances (RAG avec PostgreSQL + pgvector)
- [ ] Outils : lister les événements, inscrire une personne, transférer à un humain
- [ ] Garde-fous et alertes aux responsables
- [ ] Évaluation automatique (xUnit)
