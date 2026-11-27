<script setup lang="ts">
import { NOM_EGLISE } from '../../config'
</script>

<template>
  <header class="entete">
    <div class="entete__interieur">
      <img class="entete__logo" src="/favicon.svg" alt="" width="36" height="36" />
      <span class="entete__nom">{{ NOM_EGLISE }}</span>
    </div>
  </header>
</template>

<style scoped>
.entete {
  background: var(--couleur-surface);
  border-bottom: 1px solid var(--couleur-bordure);
}

.entete__interieur {
  max-width: 1080px;
  margin: 0 auto;
  padding: 14px 24px;
  display: flex;
  align-items: center;
  gap: 12px;
}

.entete__nom {
  font-weight: 650;
  font-size: 1.05rem;
  color: var(--couleur-primaire);
}
</style>
