import { API_ENDPOINTS } from '../config/api';
import { getAuthHeaders, fetchWithAuthCheck } from '../utils/apiClient';
import { Category } from '../types/transaction';

export const categoryService = {
  async getCategories(): Promise<Category[]> {
    const response = await fetchWithAuthCheck(API_ENDPOINTS.CATEGORIES, {
      method: 'GET',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error('Erro ao buscar categorias');
    }

    return response.json();
  },

  async createCategory(description: string, purpose: 'Income' | 'Expense' | 'Both'): Promise<Category> {
    const response = await fetchWithAuthCheck(API_ENDPOINTS.CATEGORIES, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify({
        description,
        purpose,
      }),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao criar categoria');
    }

    return response.json();
  },

  async updateCategory(id: string, description: string, purpose: 'Income' | 'Expense' | 'Both'): Promise<void> {
    const response = await fetchWithAuthCheck(API_ENDPOINTS.CATEGORIES, {
      method: 'PUT',
      headers: getAuthHeaders(),
      body: JSON.stringify({
        id,
        description,
        purpose,
      }),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao atualizar categoria');
    }
  },

  async deleteCategory(categoryId: string): Promise<void> {
    const response = await fetchWithAuthCheck(`${API_ENDPOINTS.CATEGORIES}/${categoryId}`, {
      method: 'DELETE',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao excluir categoria');
    }
  },
};

