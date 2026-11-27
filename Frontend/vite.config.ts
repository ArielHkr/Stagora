import vue from '@vitejs/plugin-vue'
import { defineConfig, loadEnv } from 'vite'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  return {
    plugins: [vue()],
    server: {
      port: 5173,
      // En développement, les appels à /api sont relayés vers le backend ASP.NET.
      // Le navigateur ne parle qu'à Vite (même origine) : pas besoin de CORS en local.
      proxy: {
        '/api': {
          target: env.VITE_API_PROXY_TARGET || 'http://localhost:5226',
          changeOrigin: true,
        },
      },
    },
  }
})
