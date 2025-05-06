// src/main.ts
import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import "./assets/styles/main.css";

// Create and mount the Vue application
const app = createApp(App);

// Register global plugins
app.use(router);

// Mount the app
app.mount("#app");
