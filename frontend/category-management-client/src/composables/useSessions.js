// src/composables/useSessions.js
import { ref, computed } from 'vue';
import { sessionService } from '@/services/sessionService';

export function useSessions() {
  const sessions = ref([]);
  const loading = ref(false);
  const error = ref(null);
  const pagination = ref({
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0
  });
  const sortOptions = ref({
    sortBy: 'startDate',
    ascending: true
  });

  /**
   * Fetch sessions that match a category's criteria
   * @param {number} categoryId - The ID of the category to filter by
   * @param {number} page - The page number (1-based)
   * @param {number} pageSize - Number of items per page
   * @param {string} sortBy - Field to sort by (startDate, title, location)
   * @param {boolean} ascending - True for ascending order, false for descending
   */
  const fetchSessionsByCategory = async (
    categoryId,
    page = 1,
    pageSize = 10,
    sortBy = 'startDate',
    ascending = true
  ) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await sessionService.getByCategory(
        categoryId,
        page,
        pageSize,
        sortBy,
        ascending
      );

      sessions.value = response.items;
      pagination.value = {
        currentPage: response.currentPage,
        pageSize: response.pageSize,
        totalCount: response.totalCount,
        totalPages: response.totalPages
      };
      sortOptions.value = { sortBy, ascending };
    } catch (err) {
      error.value = err.message || 'Failed to fetch sessions';
      console.error('Error fetching sessions:', err);
      sessions.value = [];
    } finally {
      loading.value = false;
    }
  };

  /**
   * Reload the current page of sessions with the same parameters
   * @param {number} categoryId - The category ID
   */
  const refreshSessions = async (categoryId) => {
    await fetchSessionsByCategory(
      categoryId,
      pagination.value.currentPage,
      pagination.value.pageSize,
      sortOptions.value.sortBy,
      sortOptions.value.ascending
    );
  };

  /**
   * Change the sort options and reload sessions
   * @param {number} categoryId - The category ID
   * @param {string} sortBy - Field to sort by
   * @param {boolean} ascending - Sort direction
   */
  const changeSorting = async (categoryId, sortBy, ascending) => {
    await fetchSessionsByCategory(
      categoryId,
      1, // Reset to first page when changing sort
      pagination.value.pageSize,
      sortBy,
      ascending
    );
  };

  /**
   * Change the page size and reload sessions
   * @param {number} categoryId - The category ID
   * @param {number} newPageSize - New page size
   */
  const changePageSize = async (categoryId, newPageSize) => {
    await fetchSessionsByCategory(
      categoryId,
      1, // Reset to first page when changing page size
      newPageSize,
      sortOptions.value.sortBy,
      sortOptions.value.ascending
    );
  };

  return {
    sessions,
    loading,
    error,
    pagination,
    sortOptions,
    fetchSessionsByCategory,
    refreshSessions,
    changeSorting,
    changePageSize
  };
}
