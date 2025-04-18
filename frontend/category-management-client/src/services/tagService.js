// src/services/tagService.js
import apiClient from './api';

export const tagService = {
  getAll() {
    return apiClient.get('/tags').then(response => response.data);
  }
};
