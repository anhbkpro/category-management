<!-- src/components/sessions/SessionList.vue -->
<template>
  <div class="session-list">
    <div class="session-list-header">
      <h2>Matching Sessions</h2>

      <div class="sort-options">
        <label for="sort-by">Sort by:</label>
        <select
          id="sort-by"
          v-model="localSortOptions.sortBy"
          @change="handleSortChange"
        >
          <option value="startDate">Start Date</option>
          <option value="title">Title</option>
          <option value="location">Location</option>
        </select>

        <button
          class="sort-direction"
          @click="toggleSortDirection"
          :title="localSortOptions.ascending ? 'Ascending' : 'Descending'"
        >
          <span v-if="localSortOptions.ascending">↑</span>
          <span v-else>↓</span>
        </button>
      </div>
    </div>

    <div v-if="loading" class="loading-spinner">
      <span>Loading sessions...</span>
    </div>

    <div v-else-if="error" class="error-message">
      {{ error }}
    </div>

    <div v-else-if="!sessions || sessions.length === 0" class="no-sessions">
      <p>No sessions match the selected category filters.</p>
    </div>

    <div v-else class="session-cards">
      <div v-for="session in sessions" :key="session.id" class="session-card">
        <div class="session-title">
          <h3>{{ session.title }}</h3>
        </div>

        <div class="session-details">
          <div class="session-date">
            <strong>When:</strong> {{ formatDate(session.startDate) }}
            <span v-if="formatDate(session.startDate) !== formatDate(session.endDate)">
              - {{ formatDate(session.endDate) }}
            </span>
          </div>

          <div class="session-location">
            <strong>Where:</strong> {{ session.location }}
            <span v-if="session.isOnline" class="online-badge">Online</span>
          </div>

          <div class="session-tags">
            <span v-for="(tag, index) in session.tags" :key="index" class="tag">
              {{ tag }}
            </span>
          </div>

          <div class="session-speakers" v-if="session.speakers && session.speakers.length > 0">
            <strong>Speakers: </strong>
            <span v-for="(speaker, index) in session.speakers" :key="speaker.id">
              {{ speaker.name }}{{ index < session.speakers.length - 1 ? ', ' : '' }}
            </span>
          </div>
        </div>

        <div class="session-description">
          <p>{{ truncateText(session.description, 150) }}</p>
        </div>
      </div>
    </div>

    <div v-if="sessions && sessions.length > 0" class="pagination">
      <button
        :disabled="pagination.currentPage <= 1"
        @click="changePage(pagination.currentPage - 1)"
        class="pagination-button"
      >
        Previous
      </button>

      <span class="pagination-info">
        Page {{ pagination.currentPage }} of {{ pagination.totalPages }}
      </span>

      <button
        :disabled="pagination.currentPage >= pagination.totalPages"
        @click="changePage(pagination.currentPage + 1)"
        class="pagination-button"
      >
        Next
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';

// Type definitions
interface Speaker {
  id: string;
  name: string;
}

interface Session {
  id: string;
  title: string;
  startDate: string;
  endDate: string;
  location: string;
  isOnline: boolean;
  tags: string[];
  speakers: Speaker[];
  description: string;
}

interface Pagination {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

interface SortOptions {
  sortBy: string;
  ascending: boolean;
}

// Props with types
const props = defineProps<{
  sessions: Session[];
  loading: boolean;
  error: string;
  pagination: Pagination;
  sortOptions: SortOptions;
}>();

// Emits with types
const emit = defineEmits<{
  (e: 'pageChange', page: number): void;
  (e: 'sortChange', options: SortOptions): void;
}>();

// Local copy of sort options to avoid direct mutation
const localSortOptions = ref<SortOptions>({
  sortBy: props.sortOptions.sortBy,
  ascending: props.sortOptions.ascending
});

// Update local sort options when props change
watch(
  () => props.sortOptions,
  (newOptions) => {
    localSortOptions.value = { ...newOptions };
  },
  { deep: true }
);

// Pagination methods
const changePage = (page: number) => {
  if (page < 1 || page > props.pagination.totalPages) return;
  emit('pageChange', page);
};

// Sorting methods
const handleSortChange = () => {
  emit('sortChange', localSortOptions.value);
};

const toggleSortDirection = () => {
  localSortOptions.value.ascending = !localSortOptions.value.ascending;
  handleSortChange();
};

// Helper methods
const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(date);
};

const truncateText = (text: string, maxLength: number) => {
  if (!text) return '';
  return text.length > maxLength ? `${text.substring(0, maxLength)}...` : text;
};
</script>

<style scoped>
.session-list {
  width: 100%;
}

.session-list-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.sort-options {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.sort-direction {
  background: none;
  border: 1px solid #ccc;
  border-radius: 4px;
  padding: 0.25rem 0.5rem;
  cursor: pointer;
}

.session-cards {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1rem;
}

.session-card {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 1rem;
  background-color: #fff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

.session-title h3 {
  margin-top: 0;
  margin-bottom: 0.5rem;
  color: #333;
}

.session-details {
  margin-bottom: 1rem;
  font-size: 0.9rem;
}

.session-details > div {
  margin-bottom: 0.25rem;
}

.session-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
  margin-top: 0.5rem;
}

.tag {
  background-color: #f0f0f0;
  border-radius: 4px;
  padding: 0.15rem 0.5rem;
  font-size: 0.8rem;
}

.online-badge {
  background-color: #e6f7ff;
  color: #0066cc;
  border-radius: 4px;
  padding: 0.15rem 0.5rem;
  font-size: 0.8rem;
  margin-left: 0.5rem;
}

.session-description {
  font-size: 0.9rem;
  color: #666;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 1.5rem;
  gap: 1rem;
}

.pagination-button {
  background-color: #f0f0f0;
  border: 1px solid #ccc;
  border-radius: 4px;
  padding: 0.5rem 0.75rem;
  cursor: pointer;
}

.pagination-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.loading-spinner {
  text-align: center;
  padding: 2rem;
  color: #666;
}

.error-message {
  color: #d32f2f;
  padding: 1rem;
  border: 1px solid #ffcdd2;
  border-radius: 4px;
  background-color: #ffebee;
}

.no-sessions {
  text-align: center;
  padding: 2rem;
  color: #666;
}

@media (min-width: 768px) {
  .session-cards {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (min-width: 1200px) {
  .session-cards {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>
