<script setup lang="ts">
// lent = true : la réponse tarde, l'API est sans doute en train de se réveiller.
defineProps<{ lent?: boolean }>()
</script>

<template>
  <div class="attente" role="status">
    <div class="ecrit">
      <span class="visuellement-cache">L'assistant écrit…</span>
      <span class="ecrit__point" aria-hidden="true" />
      <span class="ecrit__point" aria-hidden="true" />
      <span class="ecrit__point" aria-hidden="true" />
    </div>
    <p v-if="lent" class="attente__texte">
      L'assistant se réveille, la première réponse peut prendre jusqu'à une minute…
    </p>
  </div>
</template>

<style scoped>
.attente {
  align-self: flex-start;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.attente__texte {
  margin: 0;
  max-width: 260px;
  font-size: 0.8rem;
  color: var(--couleur-texte-doux);
}

.ecrit {
  align-self: flex-start;
  display: flex;
  gap: 5px;
  padding: 14px 16px;
  border-radius: 16px;
  border-bottom-left-radius: 4px;
  background: var(--couleur-bulle-assistant);
}

.ecrit__point {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background: var(--couleur-texte-doux);
  animation: rebond 1.2s infinite ease-in-out;
}

.ecrit__point:nth-child(3) {
  animation-delay: 0.15s;
}

.ecrit__point:nth-child(4) {
  animation-delay: 0.3s;
}

@keyframes rebond {
  0%,
  60%,
  100% {
    opacity: 0.35;
    transform: translateY(0);
  }
  30% {
    opacity: 1;
    transform: translateY(-4px);
  }
}
</style>
