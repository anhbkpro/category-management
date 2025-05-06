// src/composables/useSessions.ts
import { ref, type Ref } from 'vue';
import { sessionService, type Session, type PaginatedSessions } from '@/services/sessionService';

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

export function useSessions() {
  const sessions: Ref<Session[]> = ref([]);
  const loading = ref(false);
  const error: Ref<string | null> = ref(null);
  const pagination: Ref<Pagination> = ref({
    currentPage: 1,
    pageSize: 9,
    totalCount: 0,
    totalPages: 0
  });
  const sortOptions: Ref<SortOptions> = ref({
    sortBy: 'startDate',
    ascending: true
  });

  /**
   * Fetch sessions that match a category's criteria
   * @param {string} categoryId - The ID of the category to filter by
   * @param {number} page - The page number (1-based)
   * @param {number} pageSize - Number of items per page
   * @param {string} sortBy - Field to sort by (startDate, title, location)
   * @param {boolean} ascending - True for ascending order, false for descending
   */
  const fetchSessionsByCategory = async (
    categoryId: string,
    page = 1,
    pageSize = 9,
    sortBy = 'startDate',
    ascending = true
  ): Promise<void> => {
    loading.value = true;
    error.value = null;
    try {
      const response: PaginatedSessions = await sessionService.getByCategory(
        categoryId,
        page,
        pageSize,
        sortBy,
        ascending
      );

      sessions.value = response.sessions;
      pagination.value = {
        currentPage: response.currentPage,
        pageSize: response.pageSize,
        totalCount: response.totalCount,
        totalPages: response.totalPages
      };
      sortOptions.value = { sortBy, ascending };
    } catch (err) {
      if (err instanceof Error) {
        error.value = err.message;
      } else {
        error.value = 'Failed to fetch sessions';
      }
      console.error('Error fetching sessions:', err);
      sessions.value = [];
    } finally {
      loading.value = false;
    }
  };

  /**
   * Reload the current page of sessions with the same parameters
   * @param {string} categoryId - The category ID
   */
  const refreshSessions = async (categoryId: string): Promise<void> => {
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
   * @param {string} categoryId - The category ID
   * @param {string} sortBy - Field to sort by
   * @param {boolean} ascending - Sort direction
   */
  const changeSorting = async (
    categoryId: string,
    sortBy: string,
    ascending: boolean
  ): Promise<void> => {
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
   * @param {string} categoryId - The category ID
   * @param {number} newPageSize - New page size
   */
  const changePageSize = async (
    categoryId: string,
    newPageSize: number
  ): Promise<void> => {
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
