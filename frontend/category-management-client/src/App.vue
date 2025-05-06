<!-- src/App.vue -->
<template>
  <div class="app-container">
    <nav class="navbar">
      <div class="navbar-container">
        <router-link :to="{ name: 'home' }" class="logo">
          Learning Sessions
        </router-link>

        <div class="nav-links">
          <router-link :to="{ name: 'home' }" class="nav-link">
            Home
          </router-link>
          <router-link :to="{ name: 'categories' }" class="nav-link">
            Categories
          </router-link>
        </div>
      </div>
    </nav>

    <main class="main-content">
      <router-view />
    </main>

    <footer class="footer">
      <div class="footer-container">
        <p>&copy; {{ currentYear }} Learning Session Management</p>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const currentYear = computed(() => new Date().getFullYear());
</script>

<style>
/* Global Styles */
:root {
  --primary-color: #1976d2;
  --primary-dark: #1565c0;
  --secondary-color: #f5f5f5;
  --text-color: #333;
  --text-light: #666;
  --border-color: #e0e0e0;
  --danger-color: #d32f2f;
  --success-color: #2e7d32;
  --background-color: #f9f9f9;
}

* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen,
    Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
  font-size: 16px;
  line-height: 1.5;
  color: var(--text-color);
  background-color: var(--background-color);
}

/* Form Styles */
.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 600;
}

.form-control {
  display: block;
  width: 100%;
  padding: 0.75rem;
  font-size: 1rem;
  line-height: 1.5;
  color: var(--text-color);
  background-color: #fff;
  border: 1px solid var(--border-color);
  border-radius: 4px;
  transition: border-color 0.15s ease-in-out;
}

.form-control:focus {
  border-color: var(--primary-color);
  outline: 0;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 2rem;
}

/* Button Styles */
button {
  cursor: pointer;
}

.btn-primary {
  background-color: var(--primary-color);
  color: white;
  border: none;
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-primary:hover {
  background-color: var(--primary-dark);
}

.btn-secondary {
  background-color: var(--secondary-color);
  color: var(--text-color);
  border: 1px solid var(--border-color);
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-outline {
  background-color: transparent;
  color: var(--primary-color);
  border: 1px solid var(--primary-color);
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-danger {
  background-color: transparent;
  color: var(--danger-color);
  border: 1px solid var(--danger-color);
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-danger:hover {
  background-color: var(--danger-color);
  color: white;
}

/* App Layout */
.app-container {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.navbar {
  background-color: white;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  padding: 0.75rem 0;
}

.navbar-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.logo {
  color: var(--primary-color);
  font-size: 1.25rem;
  font-weight: 700;
  text-decoration: none;
}

.nav-links {
  display: flex;
  gap: 1.5rem;
}

.nav-link {
  color: var(--text-color);
  text-decoration: none;
  font-weight: 600;
  padding: 0.5rem 0;
  position: relative;
}

.nav-link::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 0;
  height: 2px;
  background-color: var(--primary-color);
  transition: width 0.2s;
}

.nav-link:hover::after, .router-link-active::after {
  width: 100%;
}

.main-content {
  flex: 1;
}

.footer {
  background-color: white;
  border-top: 1px solid var(--border-color);
  padding: 1.5rem 0;
  margin-top: 2rem;
}

.footer-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
  text-align: center;
  color: var(--text-light);
}

/* Add these styles to remove the underline from navigation links */
.nav-link {
  text-decoration: none;
}

.nav-link:hover {
  text-decoration: none;
}

/* If you're using router-link, you might need to target it specifically */
.router-link-active,
.router-link-exact-active {
  text-decoration: none;
}

/* You can also add a subtle effect on hover instead of an underline */
.nav-link:hover {
  opacity: 0.8;
  /* Or you could use a different effect like: */
  /* border-bottom: 2px solid #42b983; */
}
</style>
