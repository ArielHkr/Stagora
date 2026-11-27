# Mise en ligne sur Render (gratuit)

Deux services, décrits dans `render.yaml` :

| Service | Contenu | Adresse (exemple) |
|---|---|---|
| `stagora-api` | API ASP.NET (Docker) | https://stagora-api.onrender.com |
| `stagora-site` | Front Vue (site statique) | https://stagora-site.onrender.com |

L'API gratuite **se met en veille après 15 min sans visite** : le premier message suivant prend 30 à 60 s.

## 1. Pousser le code sur GitHub
La branche doit être fusionnée dans `main` (Render déploie `main`).

## 2. Créer les services
1. Crée un compte sur https://render.com (connexion avec GitHub).
2. **New → Blueprint** → choisis le dépôt `Stagora` → **Apply**.
3. Render demande les variables marquées `sync: false`. Pour le premier déploiement :
   - `Groq__ApiKey` : ta clé Groq (`gsk_...`).
   - `Cors__AllowedOrigins__0` : mets `https://stagora-site.onrender.com` (à corriger à l'étape 3 si l'adresse réelle diffère).
   - `VITE_API_URL` : mets `https://stagora-api.onrender.com` (idem).

## 3. Corriger les adresses
Render ajoute parfois un suffixe si le nom est déjà pris (ex. `stagora-api-x1y2.onrender.com`).
Une fois les deux services créés, copie leurs vraies adresses (en haut de chaque service) :

- Service **stagora-api** → Environment → `Cors__AllowedOrigins__0` = adresse réelle du **site**, sans `/` final.
- Service **stagora-site** → Environment → `VITE_API_URL` = adresse réelle de l'**API**, sans `/` final.
  Puis **Manual Deploy → Deploy latest commit** : cette valeur est intégrée au moment du build.

## 4. Vérifier
- `https://<api>/health` doit afficher `Healthy`.
- Ouvre l'adresse du site et pose une question.

## En cas de problème
| Symptôme | Cause probable |
|---|---|
| L'API ne démarre pas, « Groq:ApiKey est manquante » | Variable `Groq__ApiKey` absente ou mal nommée (deux `_`) |
| Le site affiche « Impossible de joindre le serveur » | `VITE_API_URL` faux, ou API en veille (réessayer après 1 min) |
| Erreur CORS dans la console du navigateur | `Cors__AllowedOrigins__0` ne correspond pas exactement à l'adresse du site |
| « Beaucoup de demandes » | Limite de 10 messages/min par visiteur (`RateLimit` dans appsettings.json) |
