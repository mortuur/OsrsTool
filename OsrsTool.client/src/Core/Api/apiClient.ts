import axios from 'axios';
import type {
  AxiosInstance,
  AxiosRequestConfig,
  InternalAxiosRequestConfig,
  AxiosResponse,
} from 'axios';

const baseURL = (import.meta.env.VITE_API_BASE_URL as string) ?? '';

if (!baseURL) {
  console.warn('VITE_API_BASE_URL is niet ingesteld. Zet deze in je .env bestand.');
}

const apiClient: AxiosInstance = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor — bv. auth token toevoegen
apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  // zorg dat headers object bestaat (compatibiliteit met axios types)
  config.headers = config.headers ?? {};

  const token = localStorage.getItem('token'); // of andere token-store
  if (token) {
    // cast headers naar een plain record zodat we dynamisch velden kunnen zetten
    const headers = config.headers as Record<string, string>;
    headers['Authorization'] = `Bearer ${token}`;
    // bewaar terug op config.headers met juiste shape
    config.headers = headers as InternalAxiosRequestConfig['headers'];
  }

  return config;
});

// Response interceptor — centrale foutafhandeling / refresh-token etc.
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // centrale foutafhandeling kan hier
    return Promise.reject(error);
  }
);

// Helper factory voor toekomstige services
export const createService = (prefix = '') => {
  const url = (path = '') => `${prefix}${path}`;

  return {
    get: <T = unknown>(path: string, config?: AxiosRequestConfig) =>
      apiClient.get<T>(url(path), config) as Promise<AxiosResponse<T>>,
    post: <T = unknown>(path: string, data?: unknown, config?: AxiosRequestConfig) =>
      apiClient.post<T>(url(path), data, config) as Promise<AxiosResponse<T>>,
    put: <T = unknown>(path: string, data?: unknown, config?: AxiosRequestConfig) =>
      apiClient.put<T>(url(path), data, config) as Promise<AxiosResponse<T>>,
    delete: <T = unknown>(path: string, config?: AxiosRequestConfig) =>
      apiClient.delete<T>(url(path), config) as Promise<AxiosResponse<T>>,
    rawClient: apiClient,
  };
};

export default apiClient;
