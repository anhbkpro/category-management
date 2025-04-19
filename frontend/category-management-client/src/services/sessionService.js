// src/services/sessionService.js
import apiClient from './api';

export const sessionService = {
  getByCategory(categoryId, page = 1, pageSize = 9, sortBy = 'startDate', ascending = true) {
    return apiClient.get('/sessions/category/' + categoryId, {
      params: {
        page,
        pageSize,
        sortBy,
        ascending
      }
    }).then(response => response.data);
  }
};
