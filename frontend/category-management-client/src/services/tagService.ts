// src/services/tagService.ts
import apiClient from './api';

// Define a Tag interface
export interface Tag {
  id: string;
  name: string;
  // Add other fields if your tag object has more properties
}

export const tagService = {
  getAll(): Promise<Tag[]> {
    return apiClient.get('/tags').then(response => response.data as Tag[]);
  }
};
