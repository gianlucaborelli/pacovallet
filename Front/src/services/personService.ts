import { API_ENDPOINTS } from '../config/api';
import { getAuthHeaders, fetchWithAuthCheck } from '../utils/apiClient';
import { Person } from '../types/transaction';

export const personService = {
  async getPersons(): Promise<Person[]> {
    const response = await fetchWithAuthCheck(API_ENDPOINTS.PERSONS, {
      method: 'GET',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error('Erro ao buscar pessoas');
    }

    return response.json();
  },

  async createPerson(name: string, birthDate: Date): Promise<Person> {
    const response = await fetchWithAuthCheck(API_ENDPOINTS.PERSONS, {
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
    const response = await fetchWithAuthCheck(API_ENDPOINTS.PERSONS, {
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
    const response = await fetchWithAuthCheck(`${API_ENDPOINTS.PERSONS}/${personId}`, {
      method: 'DELETE',
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || 'Erro ao excluir pessoa');
    }
  },
};

