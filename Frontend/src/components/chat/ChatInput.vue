<script setup lang="ts">
import { computed, nextTick, ref, useTemplateRef } from 'vue'
import { LONGUEUR_MAX_MESSAGE } from '../../config'

const props = defineProps<{ desactive: boolean }>()
const emit = defineEmits<{ envoyer: [texte: string] }>()

const texte = ref('')
const zone = useTemplateRef<HTMLTextAreaElement>('zone')

// On peut continuer à écrire pendant que l'assistant répond, mais pas envoyer.
const peutEnvoyer = computed(() => texte.value.trim().length > 0 && !props.desactive)

function ajusterHauteur() {
  const el = zone.value
  if (!el) return
  el.style.height = 'auto'
  el.style.height = `${Math.min(el.scrollHeight, 140)}px`
}

function envoyer() {
  if (!peutEnvoyer.value) return
  emit('envoyer', texte.value)
  texte.value = ''
  void nextTick(ajusterHauteur)
}

// Entrée envoie, Maj + Entrée passe à la ligne.
function surTouche(evenement: KeyboardEvent) {
  if (evenement.key === 'Enter' && !evenement.shiftKey && !evenement.isComposing) {
    evenement.preventDefault()
    envoyer()
  }
}

defineExpose({ focus: () => zone.value?.focus() })
</script>

<template>
  <form class="saisie" @submit.prevent="envoyer">
    <label for="message-chat" class="visuellement-cache">Votre message</label>
    <textarea
      id="message-chat"
      ref="zone"
      v-model="texte"
      class="saisie__zone"
      rows="1"
      :maxlength="LONGUEUR_MAX_MESSAGE"
      placeholder="Écrivez votre question…"
      @keydown="surTouche"
      @input="ajusterHauteur"
    />
    <button type="submit" class="saisie__bouton" :disabled="!peutEnvoyer" aria-label="Envoyer">
      <svg viewBox="0 0 24 24" width="20" height="20" aria-hidden="true">
        <path d="M4 12l16-8-6 16-2.5-6.5z" fill="currentColor" />
      </svg>
    </button>
  </form>
</template>

<style scoped>
.saisie {
  display: flex;
  align-items: flex-end;
  gap: 8px;
  padding: 12px;
  border-top: 1px solid var(--couleur-bordure);
}

.saisie__zone {
  flex: 1;
  resize: none;
  max-height: 140px;
  padding: 10px 12px;
  border: 1px solid var(--couleur-bordure);
  border-radius: 12px;
  background: var(--couleur-fond);
  line-height: 1.4;
}

.saisie__zone:focus-visible {
  outline: 2px solid var(--couleur-primaire);
  outline-offset: 0;
}

.saisie__bouton {
  display: grid;
  place-items: center;
  width: 42px;
  height: 42px;
  flex-shrink: 0;
  border: none;
  border-radius: 50%;
  background: var(--couleur-primaire);
  color: #fff;
}

.saisie__bouton:disabled {
  background: var(--couleur-bordure);
  color: var(--couleur-texte-doux);
  cursor: not-allowed;
}
</style>
