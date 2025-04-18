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
        :error="error"
        :pagination="pagination"
        :sortOptions="sortOptions"
        @pageChange="handlePageChange"
        @sortChange="handleSortChange"
      />
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, watch } from 'vue';
import SessionList from '@/components/sessions/SessionList.vue';
import { useSessions } from '@/composables/useSessions';

const props = defineProps({
  category: {
    type: Object,
    required: true
  }
});

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
const includeTags = computed(() => {
  return props.category?.conditions
    ?.filter(c => c.type === 'IncludeTag')
    ?.map(c => c.value) || [];
});

const excludeTags = computed(() => {
  return props.category?.conditions
    ?.filter(c => c.type === 'ExcludeTag')
    ?.map(c => c.value) || [];
});

const location = computed(() => {
  const locationCondition = props.category?.conditions?.find(c => c.type === 'Location');
  return locationCondition?.value || '';
});

const dateRange = computed(() => {
  const startCondition = props.category?.conditions?.find(c => c.type === 'StartDateMin');
  const endCondition = props.category?.conditions?.find(c => c.type === 'StartDateMax');

  if (startCondition?.value && endCondition?.value) {
    return `${formatDateShort(startCondition.value)} to ${formatDateShort(endCondition.value)}`;
  } else if (startCondition?.value) {
    return `From ${formatDateShort(startCondition.value)}`;
  } else if (endCondition?.value) {
    return `Until ${formatDateShort(endCondition.value)}`;
  }

  return '';
});

// Format date helper
const formatDateShort = (dateString) => {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  }).format(date);
};

// Move the loadSessions function declaration above the watch effect
const loadSessions = () => {
  if (!props.category?.id) return;

  fetchSessionsByCategory(
    props.category.id,
    pagination.value.currentPage,
    pagination.value.pageSize,
    sortOptions.value.sortBy,
    sortOptions.value.ascending
  );
};

// Now use it in the watch effect
watch(() => props.category?.id, (newCategoryId) => {
  if (newCategoryId) {
    loadSessions();
  }
}, { immediate: true });

// And in onMounted
onMounted(() => {
  if (props.category?.id) {
    loadSessions();
  }
});

// Load sessions when category changes
watch(() => props.category?.id, (newCategoryId) => {
  if (newCategoryId) {
    loadSessions();
  }
}, { immediate: true });

// Load initial sessions
onMounted(() => {
  if (props.category?.id) {
    loadSessions();
  }
});

// Event handlers
const handlePageChange = (page) => {
  fetchSessionsByCategory(
    props.category.id,
    page,
    pagination.value.pageSize,
    sortOptions.value.sortBy,
    sortOptions.value.ascending
  );
};

const handleSortChange = (newSortOptions) => {
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
