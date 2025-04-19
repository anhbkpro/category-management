// src/services/api.js
import axios from 'axios';

const apiClient = axios.create({
  baseURL: process.env.VUE_APP_API_URL || 'http://localhost:5001/api',
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  timeout: 10000
});

// Add request interceptor for handling errors
apiClient.interceptors.response.use(
  response => response,
  error => {
    console.error('API Error:', error);
    const errorMessage = error.response?.data?.message || error.message || 'Unknown error occurred';
    return Promise.reject(new Error(errorMessage));
  }
);

export default apiClient;
