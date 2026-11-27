<script setup lang="ts">
import { NOM_EGLISE } from '../../config'

const emit = defineEmits<{
  ouvrirChat: []
  poserQuestion: [question: string]
}>()

// Questions proposées : un clic ouvre le chat et envoie la question.
const suggestions = [
  { titre: 'Horaires', description: 'Cultes, prière, étude biblique', question: 'Quels sont les horaires des activités ?' },
  { titre: 'Accès', description: 'Adresse, stationnement, accessibilité', question: 'Comment me rendre à l’église et où me stationner ?' },
  { titre: 'Activités', description: 'Enfants, jeunes, aide alimentaire', question: 'Quelles activités proposez-vous pour les enfants et les jeunes ?' },
]
</script>

<template>
  <section class="accueil">
    <p class="accueil__surtitre">Bienvenue</p>
    <h1 class="accueil__titre">Une question sur l'{{ NOM_EGLISE }} ?</h1>
    <p class="accueil__texte">
      Horaires des cultes, accès, activités pour toute la famille : notre assistant
      vous répond en quelques secondes, à toute heure.
    </p>

    <button type="button" class="bouton-principal" @click="emit('ouvrirChat')">
      Poser une question
    </button>

    <ul class="suggestions" aria-label="Questions fréquentes">
      <li v-for="suggestion in suggestions" :key="suggestion.titre">
        <button type="button" class="suggestion" @click="emit('poserQuestion', suggestion.question)">
          <span class="suggestion__titre">{{ suggestion.titre }}</span>
          <span class="suggestion__description">{{ suggestion.description }}</span>
        </button>
      </li>
    </ul>
  </section>
</template>

<style scoped>
.accueil {
  max-width: 1080px;
  margin: 0 auto;
  padding: 72px 24px 64px;
}

.accueil__surtitre {
  margin: 0 0 8px;
  font-size: 0.85rem;
  font-weight: 650;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--couleur-accent);
}

.accueil__titre {
  margin: 0 0 16px;
  max-width: 18ch;
  font-size: clamp(2rem, 4.5vw, 3rem);
  line-height: 1.15;
  color: var(--couleur-primaire-fonce);
}

.accueil__texte {
  margin: 0 0 28px;
  max-width: 52ch;
  font-size: 1.1rem;
  color: var(--couleur-texte-doux);
}

.bouton-principal {
  padding: 13px 26px;
  border: none;
  border-radius: 999px;
  background: var(--couleur-primaire);
  color: #fff;
  font-weight: 600;
  transition: background 0.15s ease;
}

.bouton-principal:hover {
  background: var(--couleur-primaire-fonce);
}

.suggestions {
  list-style: none;
  margin: 48px 0 0;
  padding: 0;
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 16px;
}

.suggestion {
  width: 100%;
  height: 100%;
  padding: 20px;
  text-align: left;
  border: 1px solid var(--couleur-bordure);
  border-radius: var(--rayon);
  background: var(--couleur-surface);
  display: flex;
  flex-direction: column;
  gap: 4px;
  transition: border-color 0.15s ease, transform 0.15s ease;
}

.suggestion:hover {
  border-color: var(--couleur-accent);
  transform: translateY(-2px);
}

.suggestion__titre {
  font-weight: 650;
  color: var(--couleur-primaire);
}

.suggestion__description {
  font-size: 0.9rem;
  color: var(--couleur-texte-doux);
}
</style>
