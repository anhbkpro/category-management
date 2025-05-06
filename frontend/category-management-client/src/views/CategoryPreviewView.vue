<!-- src/views/CategoryPreviewView.vue -->
<template>
  <div class="category-preview-container">
    <div class="preview-header">
      <div class="preview-navigation">
        <button @click="goBack" class="btn-back">
          ← Back to Categories
        </button>
      </div>

      <div v-if="loading" class="loading-indicator">
        Loading category...
      </div>

      <div v-else-if="error" class="error-message">
        {{ error }}
      </div>
    </div>

    <div v-if="category" class="preview-content">
      <CategoryPreview :category="category" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import CategoryPreview from '@/components/categories/CategoryPreview.vue';
import { useCategories } from '@/composables/useCategories';
import type { Category } from '@/services/categoryService';

const route = useRoute();
const router = useRouter();

// Extract category ID from route params
const categoryId = computed<number | undefined>(() => {
  const id = route.params.id;
  return typeof id === 'string' ? Number.parseInt(id) : undefined;
});

// Get category data using the categories composable
const { currentCategory: category, loading, error, fetchCategoryById } = useCategories();

// Load category data when component mounts
onMounted(async () => {
  if (categoryId.value) {
    await fetchCategoryById(categoryId.value.toString());
  }
});

// Navigation
const goBack = (): void => {
  router.push({ name: 'categories' });
};
</script>

<style scoped>
.category-preview-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.preview-header {
  margin-bottom: 2rem;
}

.preview-navigation {
  margin-bottom: 1rem;
}

.btn-back {
  background: none;
  border: none;
  cursor: pointer;
  color: #1976d2;
  padding: 0.5rem 0;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.loading-indicator {
  padding: 1rem 0;
  color: #666;
}

.error-message {
  color: #d32f2f;
  padding: 1rem;
  border: 1px solid #ffcdd2;
  border-radius: 4px;
  background-color: #ffebee;
}

.preview-content {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
  padding: 1.5rem;
}
</style>
