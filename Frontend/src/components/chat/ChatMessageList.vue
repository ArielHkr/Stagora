<script setup lang="ts">
import { nextTick, onMounted, useTemplateRef, watch } from 'vue'
import type { MessageAffiche } from '../../types/chat'
import ChatMessageItem from './ChatMessageItem.vue'
import TypingIndicator from './TypingIndicator.vue'

const props = defineProps<{
  messages: MessageAffiche[]
  enCours: boolean
}>()

const liste = useTemplateRef<HTMLElement>('liste')

async function defilerEnBas() {
  await nextTick()
  liste.value?.scrollTo({ top: liste.value.scrollHeight, behavior: 'smooth' })
}

// Nouveau message ou indicateur « écrit… » : on descend en bas de la conversation.
watch(() => [props.messages.length, props.enCours], defilerEnBas)
onMounted(defilerEnBas)
</script>

<template>
  <div ref="liste" class="liste" role="log" aria-label="Conversation">
    <ChatMessageItem v-for="message in messages" :key="message.id" :message="message" />
    <TypingIndicator v-if="enCours" />
  </div>
</template>

<style scoped>
.liste {
  flex: 1;
  overflow-y: auto;
  padding: 16px;
  display: flex;
  flex-direction: column;
  gap: 10px;
}
</style>
