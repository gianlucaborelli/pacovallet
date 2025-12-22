import React, { useState, useEffect } from 'react';
import { personService } from '../../services/personService';
import { Person } from '../../types/transaction';
import PersonModal from './PersonModal';
import './Persons.css';

const Persons: React.FC = () => {
  const [persons, setPersons] = useState<Person[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingPerson, setEditingPerson] = useState<Person | null>(null);

  const loadPersons = async () => {
    try {
      setLoading(true);
      const data = await personService.getPersons();
      setPersons(data);
    } catch (error) {
      console.error('Erro ao carregar pessoas:', error);
      alert('Erro ao carregar pessoas');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPersons();
  }, []);

  const handleCreate = () => {
    setEditingPerson(null);
    setIsModalOpen(true);
  };

  const handleEdit = (person: Person) => {
    setEditingPerson(person);
    setIsModalOpen(true);
  };

  const handleDelete = async (person: Person) => {
    if (!window.confirm(`Tem certeza que deseja excluir a pessoa "${person.name}"?`)) {
      return;
    }

    try {
      await personService.deletePerson(person.id);
      await loadPersons();
      alert('Pessoa excluída com sucesso!');
    } catch (error) {
      console.error('Erro ao excluir pessoa:', error);
      alert(error instanceof Error ? error.message : 'Erro ao excluir pessoa');
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setEditingPerson(null);
  };

  const handleModalSuccess = async () => {
    await loadPersons();
    handleModalClose();
  };

  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleDateString('pt-BR');
  };

  const calculateAge = (birthDate: string): number => {
    const today = new Date();
    const birth = new Date(birthDate);
    let age = today.getFullYear() - birth.getFullYear();
    const monthDiff = today.getMonth() - birth.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
      age--;
    }
    return age;
  };

  return (
    <div className="persons-container">
      <div className="persons-header">
        <h1>Pessoas</h1>
        <button className="create-button" onClick={handleCreate}>
          + Criar
        </button>
      </div>

      {loading ? (
        <div className="loading">Carregando pessoas...</div>
      ) : persons.length === 0 ? (
        <div className="no-data">Nenhuma pessoa cadastrada</div>
      ) : (
        <div className="table-container">
          <table className="persons-table">
            <thead>
              <tr>
                <th>Nome</th>
                <th>Data de Nascimento</th>
                <th>Idade</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {persons.map((person) => (
                <tr key={person.id}>
                  <td>{person.name}</td>
                  <td>{formatDate(person.birthDate)}</td>
                  <td>{calculateAge(person.birthDate)} anos</td>
                  <td>
                    <div className="actions">
                      <button
                        className="action-button edit"
                        onClick={() => handleEdit(person)}
                        title="Editar"
                      >
                        ✏️
                      </button>
                      <button
                        className="action-button delete"
                        onClick={() => handleDelete(person)}
                        title="Excluir"
                      >
                        🗑️
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {isModalOpen && (
        <PersonModal
          person={editingPerson}
          onClose={handleModalClose}
          onSuccess={handleModalSuccess}
        />
      )}
    </div>
  );
};

export default Persons;

