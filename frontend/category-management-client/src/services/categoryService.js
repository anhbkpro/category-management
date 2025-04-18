// src/services/categoryService.js
import apiClient from './api';

export const categoryService = {
  getAll() {
    return apiClient.get('/categories').then(response => response.data);
  },

  getById(id) {
    return apiClient.get(`/categories/${id}`).then(response => response.data);
  },

  create(category) {
    return apiClient.post('/categories', category).then(response => response.data);
  },

  update(category) {
    return apiClient.put(`/categories/${category.id}`, category).then(response => response.data);
  },

  delete(id) {
    return apiClient.delete(`/categories/${id}`).then(response => response.data);
  }
};
