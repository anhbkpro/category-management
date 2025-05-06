// src/router/index.ts
import { createRouter, createWebHistory, type RouteRecordRaw, type NavigationGuardNext, type RouteLocationNormalized } from 'vue-router';
import HomeView from '@/views/HomeView.vue';
import CategoriesView from '@/views/CategoriesView.vue';
import CategoryPreviewView from '@/views/CategoryPreviewView.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
    meta: {
      title: 'Dashboard - Learning Session Management',
      description: 'Manage and organize your learning sessions with dynamic categories'
    }
  },
  {
    path: '/categories',
    name: 'categories',
    component: CategoriesView,
    meta: {
      title: 'Categories - Learning Session Management',
      description: 'Create and manage dynamic categories for your learning sessions'
    }
  },
  {
    path: '/categories/:id/preview',
    name: 'category-preview',
    component: CategoryPreviewView,
    meta: {
      title: 'Session Preview - Learning Session Management',
      description: 'Preview sessions matching your category criteria'
    }
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

// Update document title for SEO
router.beforeEach(
  (
    to: RouteLocationNormalized,
    from: RouteLocationNormalized,
    next: NavigationGuardNext
  ) => {
    document.title = (to.meta.title as string) || 'Learning Session Management';

    // Update meta description for SEO
    const metaDescription = document.querySelector('meta[name="description"]');
    if (metaDescription) {
      metaDescription.setAttribute(
        'content',
        (to.meta.description as string) ||
          'Manage learning sessions with dynamic categories'
      );
    }

    next();
  }
);

export default router;
