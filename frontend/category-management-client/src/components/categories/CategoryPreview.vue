<!-- src/components/categories/CategoryPreview.vue -->
<template>
  <div class="category-preview">
    <div class="preview-header">
      <h2>{{ category ? category.name : 'Category Preview' }}</h2>
      <div class="filter-summary" v-if="category">
        <div class="filter-badge" v-if="includeTags.length > 0">
          <span class="badge-label">Include Tags:</span>
          <span class="badge-value">{{ includeTags.join(', ') }}</span>
        </div>

        <div class="filter-badge" v-if="excludeTags.length > 0">
          <span class="badge-label">Exclude Tags:</span>
          <span class="badge-value">{{ excludeTags.join(', ') }}</span>
        </div>

        <div class="filter-badge" v-if="location">
          <span class="badge-label">Location:</span>
          <span class="badge-value">{{ location }}</span>
        </div>

        <div class="filter-badge" v-if="dateRange">
          <span class="badge-label">Date Range:</span>
          <span class="badge-value">{{ dateRange }}</span>
        </div>
      </div>
    </div>

    <div class="preview-content">
      <SessionList
        :sessions="sessions"
        :loading="loading"
        :error="error || 'Failed to load sessions'"
        :pagination="pagination"
        :sortOptions="sortOptions"
        @pageChange="handlePageChange"
        @sortChange="handleSortChange"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue';
import SessionList from '@/components/sessions/SessionList.vue';
import { useSessions } from '@/composables/useSessions';

// --- Type Definitions ---
interface CategoryCondition {
  type: string;
  value: string;
}

interface Category {
  id: string;
  name: string;
  conditions: CategoryCondition[];
}

// --- Props ---
const props = defineProps<{
  category: Category;
}>();

// Extract the session service functionality
const {
  sessions,
  loading,
  error,
  pagination,
  sortOptions,
  fetchSessionsByCategory
} = useSessions();

// Helper computed properties for displaying filter conditions
const includeTags = computed<string[]>(() => {
  return props.category?.conditions
    ?.filter((c: CategoryCondition) => c.type === 'IncludeTag')
    ?.map((c: CategoryCondition) => c.value) || [];
});

const excludeTags = computed<string[]>(() => {
  return props.category?.conditions
    ?.filter((c: CategoryCondition) => c.type === 'ExcludeTag')
    ?.map((c: CategoryCondition) => c.value) || [];
});

const location = computed<string>(() => {
  const locationCondition = props.category?.conditions?.find((c: CategoryCondition) => c.type === 'Location');
  return locationCondition?.value || '';
});

const dateRange = computed<string>(() => {
  const startCondition = props.category?.conditions?.find((c: CategoryCondition) => c.type === 'StartDateMin');
  const endCondition = props.category?.conditions?.find((c: CategoryCondition) => c.type === 'StartDateMax');

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
});

// Format date helper
const formatDateShort = (dateString: string): string => {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  }).format(date);
};

// Function to load sessions
const loadSessions = (): void => {
  if (!props.category?.id) return;

  fetchSessionsByCategory(
    props.category.id,
    pagination.value.currentPage,
    pagination.value.pageSize,
    sortOptions.value.sortBy,
    sortOptions.value.ascending
  );
};

// Single watch effect to load sessions when category changes
watch(() => props.category?.id, (newCategoryId: string) => {
  if (newCategoryId) {
    loadSessions();
  }
}, { immediate: true });

// Event handlers
const handlePageChange = (page: number): void => {
  fetchSessionsByCategory(
    props.category.id,
    page,
    pagination.value.pageSize,
    sortOptions.value.sortBy,
    sortOptions.value.ascending
  );
};

const handleSortChange = (newSortOptions: { sortBy: string; ascending: boolean }): void => {
  fetchSessionsByCategory(
    props.category.id,
    1, // Reset to first page when sorting changes
    pagination.value.pageSize,
    newSortOptions.sortBy,
    newSortOptions.ascending
  );
};
</script>

<style scoped>
.category-preview {
  width: 100%;
}

.preview-header {
  margin-bottom: 1.5rem;
}

.filter-summary {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-top: 0.5rem;
}

.filter-badge {
  display: inline-flex;
  align-items: center;
  background-color: #f5f5f5;
  border-radius: 4px;
  padding: 0.35rem 0.75rem;
  font-size: 0.85rem;
}

.badge-label {
  font-weight: 600;
  margin-right: 0.25rem;
}

.badge-value {
  color: #666;
}

.preview-content {
  margin-top: 1rem;
}
</style>
