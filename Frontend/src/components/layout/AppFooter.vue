<script setup lang="ts">
import { NOM_EGLISE } from '../../config'

const annee = new Date().getFullYear()
</script>

<template>
  <footer class="pied">
    <p>
      L'assistant est automatisé : ses réponses peuvent contenir des erreurs.
      Pour une situation personnelle, contactez directement l'église.
    </p>
    <p class="pied__droits">© {{ annee }} {{ NOM_EGLISE }}</p>
  </footer>
</template>

<style scoped>
.pied {
  padding: 24px;
  text-align: center;
  font-size: 0.875rem;
  color: var(--couleur-texte-doux);
  border-top: 1px solid var(--couleur-bordure);
}

.pied p {
  margin: 0 auto 6px;
  max-width: 640px;
}

.pied__droits {
  font-size: 0.8rem;
}
</style>
