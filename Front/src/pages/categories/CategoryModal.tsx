import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import { categoryService } from '../../services/categoryService';
import { Category } from '../../types/transaction';
import './CategoryModal.css';

interface CategoryModalProps {
  category: Category | null;
  onClose: () => void;
  onSuccess: () => void;
}

interface PurposeOption {
  value: 'Income' | 'Expense' | 'Both';
  label: string;
}

const purposeOptions: PurposeOption[] = [
  { value: 'Income', label: 'Receita' },
  { value: 'Expense', label: 'Despesa' },
  { value: 'Both', label: 'Ambos' },
];

const CategoryModal: React.FC<CategoryModalProps> = ({ category, onClose, onSuccess }) => {
  const [description, setDescription] = useState('');
  const [purpose, setPurpose] = useState<PurposeOption | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const isEditing = !!category;

  useEffect(() => {
    if (category) {
      setDescription(category.description);
      const selectedPurpose = purposeOptions.find((opt) => opt.value === category.purpose);
      setPurpose(selectedPurpose || null);
    } else {
      setDescription('');
      setPurpose(null);
    }
  }, [category]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!description.trim()) {
      setError('A descrição é obrigatória');
      return;
    }

    if (!purpose) {
      setError('O propósito é obrigatório');
      return;
    }

    try {
      setLoading(true);

      if (isEditing && category) {
        if (!window.confirm(`Tem certeza que deseja atualizar a categoria "${category.description}"?`)) {
          return;
        }
        await categoryService.updateCategory(category.id, description.trim(), purpose.value);
        alert('Categoria atualizada com sucesso!');
      } else {
        await categoryService.createCategory(description.trim(), purpose.value);
        alert('Categoria criada com sucesso!');
      }

      onSuccess();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao salvar categoria');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEditing ? 'Editar Categoria' : 'Criar Categoria'}</h2>
          <button className="close-button" onClick={onClose}>
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit} className="modal-form">
          {error && <div className="error-message">{error}</div>}

          <div className="form-group">
            <label htmlFor="description">Descrição *</label>
            <input
              type="text"
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Digite a descrição da categoria"
              required
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="purpose">Propósito *</label>
            <Select
              options={purposeOptions}
              value={purpose}
              onChange={(selected) => setPurpose(selected)}
              placeholder="Selecione o propósito"
              isClearable={false}
              className="react-select-container"
              classNamePrefix="react-select"
              isDisabled={loading}
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

export default CategoryModal;

