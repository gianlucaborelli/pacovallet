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

  async createPerson(name: string, birthDate: Date): Promise<Person> {
    const response = await fetch(API_ENDPOINTS.PERSONS, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify({
        name,
        birthDate: birthDate.toISOString(),
      }),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao criar pessoa');
    }

    return response.json();
  },

  async updatePerson(id: string, name: string, birthDate: Date): Promise<void> {
    const response = await fetch(API_ENDPOINTS.PERSONS, {
      method: 'PUT',
      headers: getAuthHeaders(),
      body: JSON.stringify({
        id,
        name,
        birthDate: birthDate.toISOString(),
      }),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao atualizar pessoa');
    }
  },

  async deletePerson(personId: string): Promise<void> {
    const response = await fetch(`${API_ENDPOINTS.PERSONS}/${personId}`, {
      method: 'DELETE',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao excluir pessoa');
    }
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

  async createCategory(description: string, purpose: 'Income' | 'Expense' | 'Both'): Promise<Category> {
    const response = await fetch(API_ENDPOINTS.CATEGORIES, {
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
    const response = await fetch(API_ENDPOINTS.CATEGORIES, {
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
    const response = await fetch(`${API_ENDPOINTS.CATEGORIES}/${categoryId}`, {
      method: 'DELETE',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao excluir categoria');
    }
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

  async createTransaction(
    description: string,
    amount: number,
    occurredAt: Date,
    type: 'Income' | 'Expense',
    categoryId: string,
    personId: string
  ): Promise<Transaction> {
    const response = await fetch(API_ENDPOINTS.TRANSACTIONS, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify({
        description,
        amount,
        occurredAt: occurredAt.toISOString(),
        type,
        categoryId,
        personId,
      }),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao criar transação');
    }

    return response.json();
  },

  async updateTransaction(
    id: string,
    description: string,
    amount: number,
    occurredAt: Date,
    type: 'Income' | 'Expense',
    categoryId: string,
    personId: string
  ): Promise<void> {
    const response = await fetch(API_ENDPOINTS.TRANSACTIONS, {
      method: 'PUT',
      headers: getAuthHeaders(),
      body: JSON.stringify({
        id,
        description,
        amount,
        occurredAt: occurredAt.toISOString(),
        type,
        categoryId,
        personId,
      }),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao atualizar transação');
    }
  },

  async deleteTransaction(transactionId: string): Promise<void> {
    const response = await fetch(`${API_ENDPOINTS.TRANSACTIONS}/${transactionId}`, {
      method: 'DELETE',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao excluir transação');
    }
  },
};

