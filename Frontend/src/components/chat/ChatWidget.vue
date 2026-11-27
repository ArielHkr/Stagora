<script setup lang="ts">
import { nextTick, useTemplateRef, watch } from 'vue'
import { useChat } from '../../composables/useChat'
import ChatInput from './ChatInput.vue'
import ChatMessageList from './ChatMessageList.vue'

// v-model:ouvert : le parent (App.vue) peut ouvrir ou fermer la fenêtre.
const ouvert = defineModel<boolean>('ouvert', { default: false })

const { messages, enCours, erreur, envoyer, reessayer, reinitialiser } = useChat()

const saisie = useTemplateRef<InstanceType<typeof ChatInput>>('saisie')
const lanceur = useTemplateRef<HTMLButtonElement>('lanceur')

// Accessibilité : le focus va dans le champ à l'ouverture, et revient sur le bouton à la fermeture.
watch(ouvert, async (estOuvert) => {
  await nextTick()
  if (estOuvert) saisie.value?.focus()
  else lanceur.value?.focus()
})

function fermer() {
  ouvert.value = false
}

/** Ouvre la fenêtre et envoie une question (utilisé par les suggestions de la page). */
function poserQuestion(question: string) {
  ouvert.value = true
  void envoyer(question)
}

defineExpose({ poserQuestion })
</script>

<template>
  <div class="chat">
    <Transition name="fenetre">
      <section
        v-if="ouvert"
        id="fenetre-chat"
        class="fenetre"
        role="dialog"
        aria-labelledby="titre-chat"
        @keydown.esc="fermer"
      >
        <header class="fenetre__entete">
          <div>
            <h2 id="titre-chat" class="fenetre__titre">Assistant d'accueil</h2>
            <p class="fenetre__sous-titre">Assistant automatisé</p>
          </div>
          <div class="fenetre__actions">
            <button
              type="button"
              class="icone"
              title="Nouvelle conversation"
              aria-label="Nouvelle conversation"
              @click="reinitialiser"
            >
              <svg viewBox="0 0 24 24" width="20" height="20" aria-hidden="true">
                <path
                  d="M4 12a8 8 0 1 0 2.3-5.6M4 4v4h4"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
              </svg>
            </button>
            <button type="button" class="icone" title="Fermer" aria-label="Fermer le chat" @click="fermer">
              <svg viewBox="0 0 24 24" width="20" height="20" aria-hidden="true">
                <path d="M6 6l12 12M18 6L6 18" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" />
              </svg>
            </button>
          </div>
        </header>

        <ChatMessageList :messages="messages" :en-cours="enCours" />

        <div v-if="erreur" class="erreur" role="alert">
          <span>{{ erreur }}</span>
          <button type="button" class="erreur__bouton" @click="reessayer">Réessayer</button>
        </div>

        <ChatInput ref="saisie" :desactive="enCours" @envoyer="envoyer" />
      </section>
    </Transition>

    <button
      ref="lanceur"
      type="button"
      class="lanceur"
      :class="{ 'lanceur--ouvert': ouvert }"
      :aria-expanded="ouvert"
      aria-controls="fenetre-chat"
      :aria-label="ouvert ? 'Fermer le chat' : 'Ouvrir le chat'"
      @click="ouvert = !ouvert"
    >
      <svg v-if="!ouvert" viewBox="0 0 24 24" width="26" height="26" aria-hidden="true">
        <path
          d="M5 6.5A2.5 2.5 0 0 1 7.5 4h9A2.5 2.5 0 0 1 19 6.5v7a2.5 2.5 0 0 1-2.5 2.5H11l-4 3.5V16h0.5A2.5 2.5 0 0 1 5 13.5z"
          fill="currentColor"
        />
      </svg>
      <svg v-else viewBox="0 0 24 24" width="24" height="24" aria-hidden="true">
        <path d="M6 6l12 12M18 6L6 18" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" />
      </svg>
    </button>
  </div>
</template>

<style scoped>
.chat {
  position: fixed;
  right: 24px;
  bottom: 24px;
  z-index: 100;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 16px;
}

.fenetre {
  width: min(400px, calc(100vw - 48px));
  height: min(600px, calc(100vh - 120px));
  display: flex;
  flex-direction: column;
  background: var(--couleur-surface);
  border-radius: var(--rayon);
  box-shadow: var(--ombre);
  overflow: hidden;
}

.fenetre__entete {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 14px 16px;
  background: var(--couleur-primaire);
  color: #fff;
}

.fenetre__titre {
  margin: 0;
  font-size: 1rem;
}

.fenetre__sous-titre {
  margin: 0;
  font-size: 0.8rem;
  opacity: 0.8;
}

.fenetre__actions {
  display: flex;
  gap: 4px;
}

.icone {
  display: grid;
  place-items: center;
  width: 36px;
  height: 36px;
  border: none;
  border-radius: 8px;
  background: transparent;
  color: #fff;
}

.icone:hover {
  background: rgba(255, 255, 255, 0.15);
}

.erreur {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  margin: 0 12px 8px;
  padding: 10px 12px;
  border-radius: 10px;
  background: var(--couleur-erreur-fond);
  color: var(--couleur-erreur);
  font-size: 0.875rem;
}

.erreur__bouton {
  flex-shrink: 0;
  padding: 6px 12px;
  border: 1px solid currentColor;
  border-radius: 999px;
  background: transparent;
  color: inherit;
  font-weight: 600;
}

.lanceur {
  display: grid;
  place-items: center;
  width: 60px;
  height: 60px;
  border: none;
  border-radius: 50%;
  background: var(--couleur-primaire);
  color: #fff;
  box-shadow: var(--ombre);
  transition: background 0.15s ease, transform 0.15s ease;
}

.lanceur:hover {
  background: var(--couleur-primaire-fonce);
  transform: scale(1.05);
}

.fenetre-enter-active,
.fenetre-leave-active {
  transition: opacity 0.18s ease, transform 0.18s ease;
  transform-origin: bottom right;
}

.fenetre-enter-from,
.fenetre-leave-to {
  opacity: 0;
  transform: translateY(12px) scale(0.97);
}

/* Petits écrans : la fenêtre occupe tout l'écran. */
@media (max-width: 520px) {
  .chat {
    right: 16px;
    bottom: 16px;
  }

  .fenetre {
    position: fixed;
    inset: 0;
    width: 100%;
    height: 100%;
    border-radius: 0;
  }

  .lanceur--ouvert {
    display: none;
  }
}
</style>
