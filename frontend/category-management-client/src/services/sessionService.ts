// src/services/sessionService.ts
import apiClient from './api';

// Define types for session and response
export interface Session {
  id: string;
  title: string;
  startDate: string;
  endDate: string;
  location: string;
  isOnline: boolean;
  tags: string[];
  speakers: { id: string; name: string }[];
  description: string;
  // Add other fields as needed
}

export interface PaginatedSessions {
  sessions: Session[];
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export const sessionService = {
  getByCategory(
    categoryId: string,
    page = 1,
    pageSize = 9,
    sortBy = 'startDate',
    ascending = true
  ): Promise<PaginatedSessions> {
    return apiClient
      .get(`/sessions/category/${categoryId}`, {
        params: {
          page,
          pageSize,
          sortBy,
          ascending
        }
      })
      .then(response => response.data as PaginatedSessions);
  }
};
