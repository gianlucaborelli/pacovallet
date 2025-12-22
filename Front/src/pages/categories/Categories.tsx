import React, { useState, useEffect } from 'react';
import { categoryService } from '../../services/categoryService';
import { Category } from '../../types/transaction';
import CategoryModal from './CategoryModal';
import './Categories.css';

const Categories: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);

  const loadCategories = async () => {
    try {
      setLoading(true);
      const data = await categoryService.getCategories();
      setCategories(data);
    } catch (error) {
      console.error('Erro ao carregar categorias:', error);
      alert('Erro ao carregar categorias');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCategories();
  }, []);

  const handleCreate = () => {
    setEditingCategory(null);
    setIsModalOpen(true);
  };

  const handleEdit = (category: Category) => {
    setEditingCategory(category);
    setIsModalOpen(true);
  };

  const handleDelete = async (category: Category) => {
    if (!window.confirm(`Tem certeza que deseja excluir a categoria "${category.description}"?`)) {
      return;
    }

    try {
      await categoryService.deleteCategory(category.id);
      await loadCategories();
      alert('Categoria excluída com sucesso!');
    } catch (error) {
      console.error('Erro ao excluir categoria:', error);
      alert(error instanceof Error ? error.message : 'Erro ao excluir categoria');
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setEditingCategory(null);
  };

  const handleModalSuccess = async () => {
    await loadCategories();
    handleModalClose();
  };

  const getPurposeLabel = (purpose: string): string => {
    switch (purpose) {
      case 'Income':
        return 'Receita';
      case 'Expense':
        return 'Despesa';
      case 'Both':
        return 'Ambos';
      default:
        return purpose;
    }
  };

  return (
    <div className="categories-container">
      <div className="categories-header">
        <h1>Categorias</h1>
        <button className="create-button" onClick={handleCreate}>
          + Criar
        </button>
      </div>

      {loading ? (
        <div className="loading">Carregando categorias...</div>
      ) : categories.length === 0 ? (
        <div className="no-data">Nenhuma categoria cadastrada</div>
      ) : (
        <div className="table-container">
          <table className="categories-table">
            <thead>
              <tr>
                <th>Descrição</th>
                <th>Propósito</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {categories.map((category) => (
                <tr key={category.id}>
                  <td>{category.description}</td>
                  <td>
                    <span className={`purpose-badge ${category.purpose.toLowerCase()}`}>
                      {getPurposeLabel(category.purpose)}
                    </span>
                  </td>
                  <td>
                    <div className="actions">
                      <button
                        className="action-button edit"
                        onClick={() => handleEdit(category)}
                        title="Editar"
                      >
                        ✏️
                      </button>
                      <button
                        className="action-button delete"
                        onClick={() => handleDelete(category)}
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
        <CategoryModal
          category={editingCategory}
          onClose={handleModalClose}
          onSuccess={handleModalSuccess}
        />
      )}
    </div>
  );
};

export default Categories;
