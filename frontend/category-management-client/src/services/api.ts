// src/services/api.ts
import axios, { type AxiosInstance, type AxiosResponse, type AxiosError } from 'axios';

const apiClient: AxiosInstance = axios.create({
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
  (response: AxiosResponse) => response,
  (error: AxiosError) => {
    console.error('API Error:', error);
    // Try to extract a message from the error response, fallback to error.message
    const errorMessage =
      (error.response?.data as { message?: string })?.message ||
      error.message ||
      'Unknown error occurred';
    return Promise.reject(new Error(errorMessage));
  }
);

export default apiClient;
