module.exports = {
  // other configuration...

  // Add Vue plugin
  plugins: [
    'vue',
    // other plugins...
  ],

  // Extend Vue recommended configuration
  extends: [
    'plugin:vue/vue3-recommended',
    // other configs...
  ],

  // Configure globals for Composition API
  globals: {
    defineProps: 'readonly',
    defineEmits: 'readonly',
    defineExpose: 'readonly',
    withDefaults: 'readonly'
  }
};
