<!-- src/views/CategoriesView.vue -->
<template>
  <div class="categories-container">
    <div class="categories-header">
      <h1>Category Management</h1>
      <button @click="showAddForm = true" class="btn-primary">
        <span class="icon">+</span> Create New Category
      </button>
    </div>

    <div v-if="loading" class="loading-spinner">
      Loading categories...
    </div>

    <div v-else-if="error" class="error-message">
      {{ error }}
    </div>

    <div v-else-if="!categories || categories.length === 0" class="no-categories">
      <p>No categories found. Create your first category to get started!</p>
    </div>

    <div v-else class="categories-list">
      <div v-for="category in categories" :key="category.id" class="category-card">
        <div class="category-card-header">
          <h3>{{ category.name }}</h3>
          <div class="category-actions">
            <button @click="previewCategory(category)" class="btn-secondary">
              Preview Sessions
            </button>
            <button @click="editCategory(category)" class="btn-outline">
              Edit
            </button>
            <button @click="confirmDeleteCategory(category)" class="btn-danger">
              Delete
            </button>
          </div>
        </div>

        <div class="category-description">
          {{ category.description || 'No description provided.' }}
        </div>

        <div class="category-meta">
          <div class="filter-summary">
            <div v-if="getIncludeTags(category).length > 0" class="filter-item">
              <span class="filter-label">Include:</span>
              <span class="filter-value">{{ getIncludeTags(category).join(', ') }}</span>
            </div>

            <div v-if="getExcludeTags(category).length > 0" class="filter-item">
              <span class="filter-label">Exclude:</span>
              <span class="filter-value">{{ getExcludeTags(category).join(', ') }}</span>
            </div>

            <div v-if="getLocation(category)" class="filter-item">
              <span class="filter-label">Location:</span>
              <span class="filter-value">{{ getLocation(category) }}</span>
            </div>

            <div v-if="getDateRange(category)" class="filter-item">
              <span class="filter-label">Dates:</span>
              <span class="filter-value">{{ getDateRange(category) }}</span>
            </div>
          </div>

          <div class="category-timestamps">
            <span>Created: {{ formatDate(category.createdAt) }}</span>
            <span v-if="category.updatedAt">
              · Updated: {{ formatDate(category.updatedAt) }}
            </span>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Category Modal -->
    <div v-if="showAddForm || editingCategory" class="modal-overlay">
      <div class="modal-content">
        <div class="modal-header">
          <h2>{{ editingCategory ? 'Edit Category' : 'Create New Category' }}</h2>
          <button @click="closeForm" class="close-button">&times;</button>
        </div>

        <CategoryForm
          :category="editingCategory || {}"
          @save="saveCategory"
          @cancel="closeForm"
        />
      </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div v-if="showDeleteConfirm" class="modal-overlay">
      <div class="modal-content delete-confirm">
        <div class="modal-header">
          <h2>Confirm Delete</h2>
          <button @click="showDeleteConfirm = false" class="close-button">&times;</button>
        </div>

        <div class="delete-message">
          <p>Are you sure you want to delete the category <strong>{{ categoryToDelete?.name }}</strong>?</p>
          <p>This action cannot be undone.</p>
        </div>

        <div class="delete-actions">
          <button @click="showDeleteConfirm = false" class="btn-secondary">Cancel</button>
          <button @click="deleteCategory" class="btn-danger">Delete Category</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import CategoryForm from '@/components/categories/CategoryForm.vue';
import { useCategories } from '@/composables/useCategories';

const router = useRouter();

// Get categories functionality from composable
const {
  categories,
  loading,
  error,
  fetchCategories,
  saveCategory: saveToApi,
  deleteCategory: deleteFromApi
} = useCategories();

// UI state
const showAddForm = ref(false);
const editingCategory = ref(null);
const showDeleteConfirm = ref(false);
const categoryToDelete = ref(null);

// Load categories on component mount
onMounted(() => {
  fetchCategories();
});

// Form actions
const closeForm = () => {
  showAddForm.value = false;
  editingCategory.value = null;
};

const saveCategory = async (categoryData) => {
  const success = await saveToApi(categoryData);
  if (success) {
    closeForm();
    // Refresh categories list
    fetchCategories();
  }
};

const editCategory = (category) => {
  editingCategory.value = { ...category };
  showAddForm.value = false;
};

const confirmDeleteCategory = (category) => {
  categoryToDelete.value = category;
  showDeleteConfirm.value = true;
};

const deleteCategory = async () => {
  if (!categoryToDelete.value) return;

  const success = await deleteFromApi(categoryToDelete.value.id);
  if (success) {
    showDeleteConfirm.value = false;
    categoryToDelete.value = null;
  }
};

const previewCategory = (category) => {
  router.push({ name: 'category-preview', params: { id: category.id } });
};

// Helper methods
const formatDate = (dateString) => {
  if (!dateString) return '';
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(date);
};

const getIncludeTags = (category) => {
  return category.conditions
    ?.filter(c => c.type === 'IncludeTag')
    ?.map(c => c.value) || [];
};

const getExcludeTags = (category) => {
  return category.conditions
    ?.filter(c => c.type === 'ExcludeTag')
    ?.map(c => c.value) || [];
};

const getLocation = (category) => {
  const locationCondition = category.conditions?.find(c => c.type === 'Location');
  return locationCondition?.value || '';
};

const getDateRange = (category) => {
  const startCondition = category.conditions?.find(c => c.type === 'StartDateMin');
  const endCondition = category.conditions?.find(c => c.type === 'StartDateMax');

  if (startCondition?.value && endCondition?.value) {
    return `${formatDateShort(startCondition.value)} to ${formatDateShort(endCondition.value)}`;
  }
  if (startCondition?.value) {
    return `From ${formatDateShort(startCondition.value)}`;
  }
  if (endCondition?.value) {
    return `Until ${formatDateShort(endCondition.value)}`;
  }

  return '';
};

const formatDateShort = (dateString) => {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  }).format(date);
};
</script>

<style scoped>
.categories-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.categories-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.categories-list {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
}

.category-card {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 1.5rem;
  background-color: #fff;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
}

.category-card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.category-card-header h3 {
  margin: 0;
  font-size: 1.25rem;
  color: #333;
}

.category-actions {
  display: flex;
  gap: 0.5rem;
}

.category-description {
  margin-bottom: 1rem;
  color: #666;
}

.category-meta {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  font-size: 0.9rem;
}

.filter-summary {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.filter-item {
  background-color: #f5f5f5;
  border-radius: 4px;
  padding: 0.25rem 0.5rem;
  font-size: 0.85rem;
}

.filter-label {
  font-weight: 600;
  margin-right: 0.25rem;
}

.filter-value {
  color: #666;
}

.category-timestamps {
  color: #888;
  font-size: 0.8rem;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.modal-content {
  background-color: #fff;
  border-radius: 8px;
  padding: 1.5rem;
  width: 90%;
  max-width: 800px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.modal-header h2 {
  margin: 0;
}

.close-button {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #666;
}

.delete-confirm {
  max-width: 500px;
}

.delete-message {
  margin-bottom: 1.5rem;
}

.delete-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
}

.loading-spinner, .no-categories, .error-message {
  padding: 2rem;
  text-align: center;
  color: #666;
}

.error-message {
  color: #d32f2f;
  background-color: #ffebee;
  border: 1px solid #ffcdd2;
  border-radius: 8px;
}

.btn-primary {
  background-color: #1976d2;
  color: white;
  border: none;
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-secondary {
  background-color: #f5f5f5;
  color: #333;
  border: 1px solid #e0e0e0;
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-outline {
  background-color: transparent;
  color: #1976d2;
  border: 1px solid #1976d2;
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-danger {
  background-color: transparent;
  color: #d32f2f;
  border: 1px solid #d32f2f;
  border-radius: 4px;
  padding: 0.5rem 1rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-danger:hover {
  background-color: #d32f2f;
  color: white;
}

.icon {
  font-size: 1.2rem;
  line-height: 1;
}

@media (min-width: 768px) {
  .categories-list {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (min-width: 1200px) {
  .categories-list {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>
