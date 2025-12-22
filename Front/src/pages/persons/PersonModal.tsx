import React, { useState, useEffect } from 'react';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { transactionService } from '../../services/transactionService';
import { Person } from '../../types/transaction';
import './PersonModal.css';

interface PersonModalProps {
  person: Person | null;
  onClose: () => void;
  onSuccess: () => void;
}

const PersonModal: React.FC<PersonModalProps> = ({ person, onClose, onSuccess }) => {
  const [name, setName] = useState('');
  const [birthDate, setBirthDate] = useState<Date>(new Date());
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const isEditing = !!person;

  useEffect(() => {
    if (person) {
      setName(person.name);
      setBirthDate(new Date(person.birthDate));
    } else {
      setName('');
      setBirthDate(new Date());
    }
  }, [person]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!name.trim()) {
      setError('O nome é obrigatório');
      return;
    }

    if (!birthDate) {
      setError('A data de nascimento é obrigatória');
      return;
    }

    try {
      setLoading(true);

      if (isEditing && person) {
        if (!window.confirm(`Tem certeza que deseja atualizar a pessoa "${person.name}"?`)) {
          return;
        }
        await transactionService.updatePerson(person.id, name.trim(), birthDate);
        alert('Pessoa atualizada com sucesso!');
      } else {
        await transactionService.createPerson(name.trim(), birthDate);
        alert('Pessoa criada com sucesso!');
      }

      onSuccess();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao salvar pessoa');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEditing ? 'Editar Pessoa' : 'Criar Pessoa'}</h2>
          <button className="close-button" onClick={onClose}>
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit} className="modal-form">
          {error && <div className="error-message">{error}</div>}

          <div className="form-group">
            <label htmlFor="name">Nome *</label>
            <input
              type="text"
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Digite o nome"
              required
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="birthDate">Data de Nascimento *</label>
            <DatePicker
              selected={birthDate}
              onChange={(date: Date | null) => {
                if (date) setBirthDate(date);
              }}
              dateFormat="dd/MM/yyyy"
              maxDate={new Date()}
              showYearDropdown
              showMonthDropdown
              dropdownMode="select"
            />
          </div>

          <div className="modal-actions">
            <button type="button" className="cancel-button" onClick={onClose} disabled={loading}>
              Cancelar
            </button>
            <button type="submit" className="submit-button" disabled={loading}>
              {loading ? 'Salvando...' : isEditing ? 'Atualizar' : 'Criar'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default PersonModal;

