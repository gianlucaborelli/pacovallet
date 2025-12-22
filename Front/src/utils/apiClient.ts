import { API_URL } from '../config/api';

export const getAuthHeaders = (): HeadersInit => {
  const token = localStorage.getItem('token');
  return {
    'Content-Type': 'application/json',
    ...(token && { Authorization: `Bearer ${token}` }),
  };
};

// Função para lidar com 401 e redirecionar para login
const handleUnauthorized = () => {
  localStorage.removeItem('token');
  window.location.href = '/login';
};

// Função wrapper para fetch que verifica 401
const fetchWithAuth = async (
  url: string,
  options: RequestInit = {}
): Promise<Response> => {
  const response = await fetch(url, options);

  if (response.status === 401) {
    handleUnauthorized();
    throw new Error('Não autorizado');
  }

  return response;
};

export const apiClient = {
  get: async (endpoint: string) => {
    return fetchWithAuth(`${API_URL}${endpoint}`, {
      method: 'GET',
      headers: getAuthHeaders(),
    });
  },

  post: async (endpoint: string, data?: any) => {
    return fetchWithAuth(`${API_URL}${endpoint}`, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: data ? JSON.stringify(data) : undefined,
    });
  },

  put: async (endpoint: string, data?: any) => {
    return fetchWithAuth(`${API_URL}${endpoint}`, {
      method: 'PUT',
      headers: getAuthHeaders(),
      body: data ? JSON.stringify(data) : undefined,
    });
  },

  delete: async (endpoint: string) => {
    return fetchWithAuth(`${API_URL}${endpoint}`, {
      method: 'DELETE',
      headers: getAuthHeaders(),
    });
  },
};

// Função auxiliar para usar em fetch direto (usado nos serviços)
export const fetchWithAuthCheck = async (
  url: string,
  options: RequestInit = {}
): Promise<Response> => {
  return fetchWithAuth(url, options);
};

