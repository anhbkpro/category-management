<!-- src/views/HomeView.vue -->
<template>
  <div class="home-container">
    <div class="welcome-section">
      <h1>Learning Session Management</h1>
      <p class="subtitle">Organize and filter your learning sessions with dynamic categories</p>

      <div class="dashboard-stats">
        <div class="stat-card">
          <div class="stat-value">{{ categoriesCount }}</div>
          <div class="stat-label">Categories</div>
        </div>
      </div>

      <div class="actions">
        <router-link :to="{ name: 'categories' }" class="btn-primary">
          Manage Categories
        </router-link>
      </div>
    </div>

    <div class="features-section">
      <h2>System Features</h2>

      <div class="feature-cards">
        <div class="feature-card">
          <div class="feature-icon">🗂️</div>
          <h3>Create Dynamic Categories</h3>
          <p>Group sessions based on customizable filter conditions like tags, locations, and date ranges.</p>
        </div>

        <div class="feature-card">
          <div class="feature-icon">🔍</div>
          <h3>Preview Matching Sessions</h3>
          <p>See which sessions match your category criteria in real-time with the session preview feature.</p>
        </div>

        <div class="feature-card">
          <div class="feature-icon">📊</div>
          <h3>Flexible Filtering</h3>
          <p>Include or exclude specific tags, filter by location, and set date ranges for precise control.</p>
        </div>
      </div>
    </div>

    <div class="quick-start-section">
      <h2>Quick Start Guide</h2>

      <div class="steps">
        <div class="step">
          <div class="step-number">1</div>
          <div class="step-content">
            <h3>Create a Category</h3>
            <p>Start by creating a new category with a descriptive name and set of filter conditions.</p>
          </div>
        </div>

        <div class="step">
          <div class="step-number">2</div>
          <div class="step-content">
            <h3>Add Filter Conditions</h3>
            <p>Specify which tags should be included or excluded, set location requirements, and define date ranges.</p>
          </div>
        </div>

        <div class="step">
          <div class="step-number">3</div>
          <div class="step-content">
            <h3>Preview Sessions</h3>
            <p>Click "Preview Sessions" to see which learning sessions match your category criteria.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useCategories } from '@/composables/useCategories';

// Get categories functionality
const { categories, fetchCategories } = useCategories();

// Computed property for dashboard stats
const categoriesCount = computed(() => categories.value.length);

// Load categories on component mount
onMounted(() => {
  fetchCategories();
});
</script>

<style scoped>
.home-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.welcome-section {
  text-align: center;
  margin-bottom: 3rem;
}

h1 {
  font-size: 2.5rem;
  color: #333;
  margin-bottom: 0.5rem;
}

.subtitle {
  font-size: 1.2rem;
  color: #666;
  margin-bottom: 2rem;
}

.dashboard-stats {
  display: flex;
  justify-content: center;
  gap: 2rem;
  margin-bottom: 2rem;
}

.stat-card {
  background-color: #fff;
  border-radius: 8px;
  padding: 1.5rem 2rem;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.08);
  min-width: 150px;
}

.stat-value {
  font-size: 2.5rem;
  font-weight: 700;
  color: #1976d2;
  margin-bottom: 0.5rem;
}

.stat-label {
  font-size: 1rem;
  color: #666;
}

.actions {
  margin-top: 1.5rem;
}

.btn-primary {
  display: inline-block;
  background-color: #1976d2;
  color: white;
  border: none;
  border-radius: 4px;
  padding: 0.75rem 1.5rem;
  font-size: 1rem;
  font-weight: 600;
  text-decoration: none;
  transition: background-color 0.2s;
}

.btn-primary:hover {
  background-color: #1565c0;
}

.features-section, .quick-start-section {
  margin-bottom: 3rem;
}

h2 {
  text-align: center;
  font-size: 1.8rem;
  color: #333;
  margin-bottom: 2rem;
}

.feature-cards {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
}

.feature-card {
  background-color: #fff;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
  text-align: center;
}

.feature-icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.feature-card h3 {
  font-size: 1.2rem;
  color: #333;
  margin-bottom: 0.75rem;
}

.feature-card p {
  color: #666;
}

.steps {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.step {
  display: flex;
  gap: 1.5rem;
  align-items: flex-start;
}

.step-number {
  background-color: #1976d2;
  color: white;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2rem;
  font-weight: 700;
  flex-shrink: 0;
}

.step-content h3 {
  font-size: 1.2rem;
  color: #333;
  margin: 0 0 0.5rem;
}

.step-content p {
  color: #666;
  margin: 0;
}

@media (min-width: 768px) {
  .feature-cards {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>
