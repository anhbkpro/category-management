<!-- src/components/categories/CategoryForm.vue -->
<template>
  <div class="category-form">
    <form @submit.prevent="saveCategory">
      <div class="form-group">
        <label for="name">Category Name</label>
        <input
          type="text"
          id="name"
          v-model="form.name"
          class="form-control"
          required
        />
      </div>

      <div class="form-group">
        <label for="description">Description</label>
        <textarea
          id="description"
          v-model="form.description"
          class="form-control"
          rows="3"
        ></textarea>
      </div>

      <h3>Filter Conditions</h3>

      <!-- Include Tags section with autocomplete -->
      <div class="condition-section">
        <h4>Include Tags</h4>
        <p class="text-muted">Sessions must contain all these tags</p>

        <div class="tag-list">
          <div
            v-for="(tag, index) in includeTags"
            :key="`include-${index}`"
            class="tag-item"
          >
            <span>{{ tag }}</span>
            <button type="button" @click="removeIncludeTag(index)" class="btn-remove">×</button>
          </div>
        </div>

        <div class="tag-input">
          <div class="autocomplete-wrapper">
            <input
              type="text"
              v-model="newIncludeTag"
              placeholder="Add tag..."
              @input="filterIncludeTagSuggestions"
              @keyup.enter="addIncludeTag"
              @keydown.down="selectNextIncludeTag"
              @keydown.up="selectPrevIncludeTag"
              @blur="hideIncludeTagSuggestions"
            />
            <div v-if="showIncludeTagSuggestions && filteredIncludeTags.length > 0" class="tag-suggestions">
              <div
                v-for="(tag, index) in filteredIncludeTags"
                :key="index"
                :class="['tag-suggestion', { 'selected': selectedIncludeTagIndex === index }]"
                @mousedown="selectIncludeTag(tag.name)"
                @mouseover="selectedIncludeTagIndex = index"
              >
                {{ tag.name }}
              </div>
            </div>
          </div>
          <button type="button" @click="addIncludeTag" class="btn-add">Add</button>
        </div>
      </div>

      <!-- Exclude Tags section with autocomplete -->
      <div class="condition-section">
        <h4>Exclude Tags</h4>
        <p class="text-muted">Sessions must NOT contain any of these tags</p>

        <div class="tag-list">
          <div
            v-for="(tag, index) in excludeTags"
            :key="`exclude-${index}`"
            class="tag-item"
          >
            <span>{{ tag }}</span>
            <button type="button" @click="removeExcludeTag(index)" class="btn-remove">×</button>
          </div>
        </div>

        <div class="tag-input">
          <div class="autocomplete-wrapper">
            <input
              type="text"
              v-model="newExcludeTag"
              placeholder="Add tag..."
              @input="filterExcludeTagSuggestions"
              @keyup.enter="addExcludeTag"
              @keydown.down="selectNextExcludeTag"
              @keydown.up="selectPrevExcludeTag"
              @blur="hideExcludeTagSuggestions"
            />
            <div v-if="showExcludeTagSuggestions && filteredExcludeTags.length > 0" class="tag-suggestions">
              <div
                v-for="(tag, index) in filteredExcludeTags"
                :key="index"
                :class="['tag-suggestion', { 'selected': selectedExcludeTagIndex === index }]"
                @mousedown="selectExcludeTag(tag.name)"
                @mouseover="selectedExcludeTagIndex = index"
              >
                {{ tag.name }}
              </div>
            </div>
          </div>
          <button type="button" @click="addExcludeTag" class="btn-add">Add</button>
        </div>
      </div>

      <!-- Location -->
      <div class="form-group">
        <label for="location">Location</label>
        <select id="location" v-model="location" class="form-control">
          <option value="">Any Location</option>
          <option value="Online">Online</option>
          <option value="In-Person">In-Person</option>
        </select>
      </div>

      <!-- Date Range -->
      <div class="form-group date-range">
        <h4>Date Range</h4>
        <div class="date-inputs">
          <div>
            <label for="startDate">From</label>
            <input
              type="date"
              id="startDate"
              v-model="startDate"
              class="form-control"
            />
          </div>
          <div>
            <label for="endDate">To</label>
            <input
              type="date"
              id="endDate"
              v-model="endDate"
              class="form-control"
            />
          </div>
        </div>
      </div>

      <div class="form-actions">
        <button type="button" @click="$emit('cancel')" class="btn-secondary">Cancel</button>
        <button type="submit" class="btn-primary">Save Category</button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { tagService } from '@/services/tagService';

// --- Type Definitions ---
interface CategoryCondition {
  type: number;
  value: string;
}

interface Category {
  id: string | null;
  name: string;
  description: string;
  conditions: CategoryCondition[];
}

interface Tag {
  name: string;
}

// --- Props ---
const props = defineProps<{
  category: Category;
}>();

// --- Emits ---
const emit = defineEmits<{
  (e: 'save', category: Category): void;
  (e: 'cancel'): void;
}>();

// --- Form fields ---
const form = ref({
  id: props.category.id,
  name: props.category.name,
  description: props.category.description
});

// --- Tag management ---
const includeTags = ref<string[]>([]);
const excludeTags = ref<string[]>([]);
const newIncludeTag = ref('');
const newExcludeTag = ref('');

// --- Location and Date Range ---
const location = ref('');
const startDate = ref('');
const endDate = ref('');

// --- Tags from database ---
const availableTags = ref<Tag[]>([]);
const loadingTags = ref(false);

// --- Include tags autocomplete ---
const filteredIncludeTags = ref<Tag[]>([]);
const showIncludeTagSuggestions = ref(false);
const selectedIncludeTagIndex = ref(-1);

// --- Exclude tags autocomplete ---
const filteredExcludeTags = ref<Tag[]>([]);
const showExcludeTagSuggestions = ref(false);
const selectedExcludeTagIndex = ref(-1);

// --- Fetch available tags ---
const fetchTags = async () => {
  loadingTags.value = true;
  try {
    availableTags.value = await tagService.getAll();
  } catch (error) {
    console.error('Error fetching tags:', error);
  } finally {
    loadingTags.value = false;
  }
};

// --- Include tags filtering and selection ---
const filterIncludeTagSuggestions = () => {
  if (!newIncludeTag.value.trim()) {
    filteredIncludeTags.value = [];
    showIncludeTagSuggestions.value = false;
    return;
  }

  const search = newIncludeTag.value.toLowerCase().trim();
  filteredIncludeTags.value = availableTags.value
    .filter(tag => tag.name.toLowerCase().includes(search))
    .filter(tag => !includeTags.value.includes(tag.name))
    .slice(0, 5);

  showIncludeTagSuggestions.value = filteredIncludeTags.value.length > 0;
  selectedIncludeTagIndex.value = -1;
};

const selectIncludeTag = (tagName: string) => {
  newIncludeTag.value = tagName;
  addIncludeTag();
  showIncludeTagSuggestions.value = false;
};

const selectNextIncludeTag = () => {
  if (filteredIncludeTags.value.length === 0) return;
  selectedIncludeTagIndex.value = (selectedIncludeTagIndex.value + 1) % filteredIncludeTags.value.length;
};

const selectPrevIncludeTag = () => {
  if (filteredIncludeTags.value.length === 0) return;
  selectedIncludeTagIndex.value = selectedIncludeTagIndex.value <= 0
    ? filteredIncludeTags.value.length - 1
    : selectedIncludeTagIndex.value - 1;
};

const hideIncludeTagSuggestions = () => {
  setTimeout(() => {
    showIncludeTagSuggestions.value = false;
  }, 200);
};

// --- Exclude tags filtering and selection ---
const filterExcludeTagSuggestions = () => {
  if (!newExcludeTag.value.trim()) {
    filteredExcludeTags.value = [];
    showExcludeTagSuggestions.value = false;
    return;
  }

  const search = newExcludeTag.value.toLowerCase().trim();
  filteredExcludeTags.value = availableTags.value
    .filter(tag => tag.name.toLowerCase().includes(search))
    .filter(tag => !excludeTags.value.includes(tag.name))
    .slice(0, 5);

  showExcludeTagSuggestions.value = filteredExcludeTags.value.length > 0;
  selectedExcludeTagIndex.value = -1;
};

const selectExcludeTag = (tagName: string) => {
  newExcludeTag.value = tagName;
  addExcludeTag();
  showExcludeTagSuggestions.value = false;
};

const selectNextExcludeTag = () => {
  if (filteredExcludeTags.value.length === 0) return;
  selectedExcludeTagIndex.value = (selectedExcludeTagIndex.value + 1) % filteredExcludeTags.value.length;
};

const selectPrevExcludeTag = () => {
  if (filteredExcludeTags.value.length === 0) return;
  selectedExcludeTagIndex.value = selectedExcludeTagIndex.value <= 0
    ? filteredExcludeTags.value.length - 1
    : selectedExcludeTagIndex.value - 1;
};

const hideExcludeTagSuggestions = () => {
  setTimeout(() => {
    showExcludeTagSuggestions.value = false;
  }, 200);
};

// --- Load tags and initialize form on mount ---
onMounted(() => {
  fetchTags();

  if (props.category.id) {
    form.value = { ...props.category };

    // Extract include tags (Type = 0)
    const includeTagConditions = props.category.conditions?.filter(c => c.type === 0) || [];
    includeTags.value = includeTagConditions.map(c => c.value);

    // Extract exclude tags (Type = 1)
    const excludeTagConditions = props.category.conditions?.filter(c => c.type === 1) || [];
    excludeTags.value = excludeTagConditions.map(c => c.value);

    // Extract location (Type = 2)
    const locationCondition = props.category.conditions?.find(c => c.type === 2);
    location.value = locationCondition?.value || '';

    // Extract date range (Type = 3 for StartDateMin, Type = 4 for StartDateMax)
    const startDateCondition = props.category.conditions?.find(c => c.type === 3);
    const endDateCondition = props.category.conditions?.find(c => c.type === 4);

    startDate.value = startDateCondition?.value || '';
    endDate.value = endDateCondition?.value || '';
  }
});

// --- Tag management methods ---
const addIncludeTag = () => {
  const tag = newIncludeTag.value.trim();
  if (tag && !includeTags.value.includes(tag)) {
    includeTags.value.push(tag);
    newIncludeTag.value = '';
    showIncludeTagSuggestions.value = false;
  }
};

const removeIncludeTag = (index: number) => {
  includeTags.value.splice(index, 1);
};

const addExcludeTag = () => {
  const tag = newExcludeTag.value.trim();
  if (tag && !excludeTags.value.includes(tag)) {
    excludeTags.value.push(tag);
    newExcludeTag.value = '';
    showExcludeTagSuggestions.value = false;
  }
};

const removeExcludeTag = (index: number) => {
  excludeTags.value.splice(index, 1);
};

// --- Save category with all conditions ---
const saveCategory = () => {
  const categoryData: Category = {
    ...form.value,
    conditions: []
  };

  // Add include tag conditions (Type = 0)
  for (const tag of includeTags.value) {
    categoryData.conditions.push({
      type: 0,
      value: tag
    });
  }

  // Add exclude tag conditions (Type = 1)
  for (const tag of excludeTags.value) {
    categoryData.conditions.push({
      type: 1,
      value: tag
    });
  }

  // Add location condition (Type = 2)
  if (location.value) {
    categoryData.conditions.push({
      type: 2,
      value: location.value
    });
  }

  // Add date range conditions (Type = 3 for StartDateMin, Type = 4 for StartDateMax)
  if (startDate.value) {
    categoryData.conditions.push({
      type: 3,
      value: startDate.value
    });
  }

  if (endDate.value) {
    categoryData.conditions.push({
      type: 4,
      value: endDate.value
    });
  }

  emit('save', categoryData);
};
</script>

<style scoped>
/* Existing styles */

.autocomplete-wrapper {
  position: relative;
  flex: 1;
}

.tag-suggestions {
  position: absolute;
  top: 100%;
  left: 0;
  width: 100%;
  max-height: 200px;
  overflow-y: auto;
  background-color: white;
  border: 1px solid #ccc;
  border-top: none;
  border-radius: 0 0 4px 4px;
  z-index: 10;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.tag-suggestion {
  padding: 8px 12px;
  cursor: pointer;
}

.tag-suggestion:hover, .tag-suggestion.selected {
  background-color: #f5f5f5;
}
</style>
