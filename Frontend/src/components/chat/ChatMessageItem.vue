<script setup lang="ts">
import { computed } from 'vue'
import type { MessageAffiche } from '../../types/chat'
import { decouperGras } from '../../utils/formatTexte'

const props = defineProps<{ message: MessageAffiche }>()

const segments = computed(() => decouperGras(props.message.contenu))
const auteur = computed(() => (props.message.role === 'user' ? 'Vous' : 'Assistant'))
</script>

<template>
  <div class="message" :class="`message--${message.role}`">
    <span class="visuellement-cache">{{ auteur }} :</span>
    <!-- Pas de v-html : Vue échappe chaque morceau, aucun HTML de la réponse ne peut s'exécuter. -->
    <p class="message__bulle"><template v-for="(segment, index) in segments" :key="index"><strong v-if="segment.gras">{{ segment.texte }}</strong><template v-else>{{ segment.texte }}</template></template></p>
  </div>
</template>

<style scoped>
.message {
  display: flex;
}

.message--user {
  justify-content: flex-end;
}

.message__bulle {
  margin: 0;
  max-width: 85%;
  padding: 10px 14px;
  border-radius: 16px;
  font-size: 0.95rem;
  white-space: pre-wrap;
  overflow-wrap: anywhere;
}

.message--assistant .message__bulle {
  background: var(--couleur-bulle-assistant);
  border-bottom-left-radius: 4px;
}

.message--user .message__bulle {
  background: var(--couleur-bulle-visiteur);
  color: #fff;
  border-bottom-right-radius: 4px;
}
</style>
