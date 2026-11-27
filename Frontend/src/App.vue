<script setup lang="ts">
import { onMounted, ref, useTemplateRef } from 'vue'
import AppFooter from './components/layout/AppFooter.vue'
import AppHeader from './components/layout/AppHeader.vue'
import HeroSection from './components/home/HeroSection.vue'
import ChatWidget from './components/chat/ChatWidget.vue'
import { reveillerApi } from './services/chatApi'

// Dès l'ouverture de la page, on réveille l'API (en veille sur l'offre gratuite).
onMounted(reveillerApi)

const chatOuvert = ref(false)
const chat = useTemplateRef<InstanceType<typeof ChatWidget>>('chat')

function poserQuestion(question: string) {
  chat.value?.poserQuestion(question)
}
</script>

<template>
  <AppHeader />

  <main class="contenu">
    <HeroSection @ouvrir-chat="chatOuvert = true" @poser-question="poserQuestion" />
  </main>

  <AppFooter />

  <ChatWidget ref="chat" v-model:ouvert="chatOuvert" />
</template>

<style scoped>
.contenu {
  flex: 1;
}
</style>
