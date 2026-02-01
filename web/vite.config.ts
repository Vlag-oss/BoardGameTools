import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { VitePWA } from "vite-plugin-pwa";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    VitePWA({
      registerType: "autoUpdate",
       devOptions: {
        enabled: true,
      },
      includeAssets: ["favicon.ico", "robots.txt", "apple-touch-icon.png"],
      manifest: {
        name: "BoardGameTools",
        short_name: "BGT",
        description: "Board game library",
        theme_color: "#0f172a",
        background_color: "#0f172a",
        display: "standalone",
        scope: "/",
        start_url: "/",
        icons: [
          { src: "pwa-192x192.png", sizes: "192x192", type: "image/png" },
          { src: "pwa-512x512.png", sizes: "512x512", type: "image/png" },
          { src: "pwa-512x512.png", sizes: "512x512", type: "image/png", purpose: "any maskable" }
        ],
        screenshots: [
        // {
        //   src: "screenshots/desktop-wide.png",
        //   sizes: "1280x720",
        //   type: "image/png",
        //   form_factor: "wide"
        // },
        // {
        //   src: "screenshots/mobile.png",
        //   sizes: "750x1334",
        //   type: "image/png"
        // }
      ]
      }
    })
  ],
})
