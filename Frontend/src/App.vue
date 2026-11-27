<script setup lang="ts">
import { ref, useTemplateRef } from 'vue'
import AppFooter from './components/layout/AppFooter.vue'
import AppHeader from './components/layout/AppHeader.vue'
import HeroSection from './components/home/HeroSection.vue'
import ChatWidget from './components/chat/ChatWidget.vue'

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
