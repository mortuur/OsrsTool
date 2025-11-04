import axios from 'axios';
import type { AxiosInstance, AxiosRequestConfig, InternalAxiosRequestConfig } from 'axios';

const baseURL = (import.meta.env.VITE_API_BASE_URL as string) ?? '';

if (!baseURL) {
  // korte waarschuwing tijdens development als de env ontbreekt
  // verwijder of vervang door een fallback in productie
  // (console.warn mag verwijderd worden later)
  console.warn('VITE_API_BASE_URL is niet ingesteld. Zet deze in je .env bestand.');
}

const apiClient: AxiosInstance = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
  // withCredentials: true, // enable indien je cookies/auth nodig hebt
});

// Request interceptor — bv. auth token toevoegen
apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  // zorg dat headers object bestaat (compatibiliteit met axios types)
  config.headers = config.headers ?? {};
  const token = localStorage.getItem('token'); // of andere token-store
  if (token) {
    // Authorization toevoegen
    // @ts-ignore intent: sommige axios-versies hebben andere header types
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor — centrale foutafhandeling / refresh-token etc.
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // hier kun je globale foutafhandeling doen (401 redirect, notificaties, ...)
    return Promise.reject(error);
  }
);

// Helper factory voor toekomstige services
export const createService = (prefix = '') => {
  const url = (path = '') => `${prefix}${path}`;
  return {
    get: <T = any>(path: string, config?: AxiosRequestConfig) => apiClient.get<T>(url(path), config),
    post: <T = any>(path: string, data?: any, config?: AxiosRequestConfig) =>
      apiClient.post<T>(url(path), data, config),
    put: <T = any>(path: string, data?: any, config?: AxiosRequestConfig) =>
      apiClient.put<T>(url(path), data, config),
    delete: <T = any>(path: string, config?: AxiosRequestConfig) => apiClient.delete<T>(url(path), config),
    rawClient: apiClient,
  };
};

export default apiClient;