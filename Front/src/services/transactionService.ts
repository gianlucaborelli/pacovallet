import { API_ENDPOINTS } from '../config/api';
import { getAuthHeaders } from '../utils/apiClient';
import { Person, Category, Transaction, TransactionFilters } from '../types/transaction';

export const transactionService = {
  async getPersons(): Promise<Person[]> {
    const response = await fetch(API_ENDPOINTS.PERSONS, {
      method: 'GET',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error('Erro ao buscar pessoas');
    }

    return response.json();
  },

  async getCategories(): Promise<Category[]> {
    const response = await fetch(API_ENDPOINTS.CATEGORIES, {
      method: 'GET',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error('Erro ao buscar categorias');
    }

    return response.json();
  },

  async getTransactions(filters: TransactionFilters): Promise<Transaction[]> {
    const params = new URLSearchParams();

    if (filters.initialDate) {
      const dateStr = filters.initialDate.toISOString().split('T')[0];
      params.append('InitialDate', dateStr);
    }

    if (filters.finalDate) {
      const dateStr = filters.finalDate.toISOString().split('T')[0];
      params.append('FinalDate', dateStr);
    }

    if (filters.type) {
      params.append('Type', filters.type);
    }

    if (filters.personsId && filters.personsId.length > 0) {
      filters.personsId.forEach((id) => {
        params.append('PersonsId', id);
      });
    }

    if (filters.category && filters.category.length > 0) {
      filters.category.forEach((id) => {
        params.append('CategoryId', id);
      });
    }

    const url = `${API_ENDPOINTS.TRANSACTIONS}${params.toString() ? `?${params.toString()}` : ''}`;

    const response = await fetch(url, {
      method: 'GET',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error('Erro ao buscar transações');
    }

    return response.json();
  },
};

