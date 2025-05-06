// src/composables/useCategories.ts
import { ref, type Ref } from 'vue';
import { categoryService, type Category } from '@/services/categoryService';

export function useCategories() {
  const categories: Ref<Category[]> = ref([]);
  const currentCategory: Ref<Category | null> = ref(null);
  const loading = ref(false);
  const error: Ref<string | null> = ref(null);

  const fetchCategories = async (): Promise<void> => {
    loading.value = true;
    error.value = null;
    try {
      categories.value = await categoryService.getAll();
    } catch (err) {
      if (err instanceof Error) {
        error.value = err.message;
      } else {
        error.value = 'Failed to fetch categories';
      }
      console.error('Error fetching categories:', err);
    } finally {
      loading.value = false;
    }
  };

  const fetchCategoryById = async (id: string): Promise<void> => {
    loading.value = true;
    error.value = null;
    try {
      currentCategory.value = await categoryService.getById(id);
    } catch (err) {
      if (err instanceof Error) {
        error.value = err.message;
      } else {
        error.value = 'Failed to fetch category';
      }
      console.error('Error fetching category:', err);
    } finally {
      loading.value = false;
    }
  };

  const saveCategory = async (category: Category): Promise<boolean> => {
    loading.value = true;
    error.value = null;
    try {
      if (category.id) {
        await categoryService.update(category);
        // Update the local list if the category already exists
        const index = categories.value.findIndex(c => c.id === category.id);
        if (index !== -1) {
          categories.value[index] = { ...category };
        }
      } else {
        const newCategory = await categoryService.create(category);
        categories.value.push(newCategory);
      }
      return true;
    } catch (err) {
      if (err instanceof Error) {
        error.value = err.message;
      } else {
        error.value = 'Failed to save category';
      }
      console.error('Error saving category:', err);
      return false;
    } finally {
      loading.value = false;
    }
  };

  const deleteCategory = async (id: string): Promise<boolean> => {
    loading.value = true;
    error.value = null;
    try {
      await categoryService.delete(id);
      categories.value = categories.value.filter(c => c.id !== id);
      return true;
    } catch (err) {
      if (err instanceof Error) {
        error.value = err.message;
      } else {
        error.value = 'Failed to delete category';
      }
      console.error('Error deleting category:', err);
      return false;
    } finally {
      loading.value = false;
    }
  };

  return {
    categories,
    currentCategory,
    loading,
    error,
    fetchCategories,
    fetchCategoryById,
    saveCategory,
    deleteCategory
  };
}
