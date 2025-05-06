// src/services/categoryService.ts
import apiClient from './api';

// Define types for category and response
export interface CategoryCondition {
  type: string;
  value: string;
}

export interface Category {
  id: string;
  name: string;
  description: string;
  conditions: CategoryCondition[];
  // Add other fields as needed
}

export const categoryService = {
  getAll(): Promise<Category[]> {
    return apiClient.get('/categories').then(response => response.data as Category[]);
  },

  getById(id: string): Promise<Category> {
    return apiClient.get(`/categories/${id}`).then(response => response.data as Category);
  },

  create(category: Omit<Category, 'id'>): Promise<Category> {
    return apiClient.post('/categories', category).then(response => response.data as Category);
  },

  update(category: Category): Promise<Category> {
    return apiClient.put(`/categories/${category.id}`, category).then(response => response.data as Category);
  },

  delete(id: string): Promise<{ success: boolean }> {
    return apiClient.delete(`/categories/${id}`).then(response => response.data as { success: boolean });
  }
};
