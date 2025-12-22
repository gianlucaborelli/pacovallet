import React, { useState, useEffect, useCallback } from 'react';
import Select from 'react-select';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { transactionService } from '../../services/transactionService';
import { Person, Category, Transaction, TransactionFilters } from '../../types/transaction';
import TransactionModal from './TransactionModal';
import './Home.css';

interface SelectOption {
  value: string;
  label: string;
}

const Home: React.FC = () => {

  const [persons, setPersons] = useState<Person[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [loading, setLoading] = useState(false);
  const [loadingData, setLoadingData] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingTransaction, setEditingTransaction] = useState<Transaction | null>(null);

  // Filtros
  const [selectedPersons, setSelectedPersons] = useState<SelectOption[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<SelectOption[]>([]);
  const [selectedType, setSelectedType] = useState<SelectOption | null>(null);
  const [initialDate, setInitialDate] = useState<Date>(() => {
    const date = new Date();
    date.setFullYear(date.getFullYear() - 1);
    return date;
  });
  const [finalDate, setFinalDate] = useState<Date>(new Date());

  // Carregar Persons e Categories ao montar
  useEffect(() => {
    const loadInitialData = async () => {
      try {
        setLoadingData(true);
        const [personsData, categoriesData] = await Promise.all([
          transactionService.getPersons(),
          transactionService.getCategories(),
        ]);
        setPersons(personsData);
        setCategories(categoriesData);
      } catch (error) {
        console.error('Erro ao carregar dados iniciais:', error);
      } finally {
        setLoadingData(false);
      }
    };

    loadInitialData();
  }, []);

  // Função para buscar transações
  const fetchTransactions = useCallback(async () => {
    try {
      setLoading(true);
      const filters: TransactionFilters = {
        initialDate,
        finalDate,
        type: selectedType?.value as 'Income' | 'Expense' | '' | undefined,
        personsId: selectedPersons.map((p) => p.value),
        category: selectedCategories.map((c) => c.value),
      };

      // Remover propriedades vazias
      if (!filters.type) delete filters.type;
      if (filters.personsId?.length === 0) delete filters.personsId;
      if (filters.category?.length === 0) delete filters.category;

      const data = await transactionService.getTransactions(filters);
      setTransactions(data);
    } catch (error) {
      console.error('Erro ao buscar transações:', error);
      setTransactions([]);
    } finally {
      setLoading(false);
    }
  }, [initialDate, finalDate, selectedPersons, selectedCategories, selectedType]);

  // Buscar transações quando os filtros mudarem
  useEffect(() => {
    if (!loadingData) {
      fetchTransactions();
    }
  }, [initialDate, finalDate, selectedPersons, selectedCategories, selectedType, loadingData, fetchTransactions]);

  // Opções para o SelectBox de tipo
  const typeOptions: SelectOption[] = [
    { value: 'Income', label: 'Receita' },
    { value: 'Expense', label: 'Despesa' },
  ];

  // Opções para Persons
  const personOptions: SelectOption[] = persons.map((person) => ({
    value: person.id,
    label: person.name,
  }));

  // Opções para Categories
  const categoryOptions: SelectOption[] = categories.map((category) => ({
    value: category.id,
    label: category.description,
  }));

  // Formatar valor monetário
  const formatCurrency = (value: number | string): string => {
    const numValue = typeof value === 'string' ? parseFloat(value) : value;
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(numValue);
  };

  // Formatar data
  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleDateString('pt-BR');
  };

  // Calcular totais
  const calculateTotals = () => {
    let income = 0;
    let expense = 0;

    transactions.forEach((transaction) => {
      const amount = typeof transaction.amount === 'string' 
        ? parseFloat(transaction.amount) 
        : transaction.amount;

      if (transaction.type === 'Income') {
        income += amount;
      } else {
        expense += amount;
      }
    });

    const balance = income - expense;

    return { income, expense, balance };
  };

  const { income, expense, balance } = calculateTotals();

  const handleCreate = () => {
    setEditingTransaction(null);
    setIsModalOpen(true);
  };

  const handleEdit = (transaction: Transaction) => {
    setEditingTransaction(transaction);
    setIsModalOpen(true);
  };

  const handleDelete = async (transaction: Transaction) => {
    if (!window.confirm(`Tem certeza que deseja excluir a transação "${transaction.description}"?`)) {
      return;
    }

    try {
      await transactionService.deleteTransaction(transaction.id);
      await fetchTransactions();
      alert('Transação excluída com sucesso!');
    } catch (error) {
      console.error('Erro ao excluir transação:', error);
      alert(error instanceof Error ? error.message : 'Erro ao excluir transação');
    }
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setEditingTransaction(null);
  };

  const handleModalSuccess = async () => {
    await fetchTransactions();
    handleModalClose();
  };

  return (
    <div className="home-container">
      <div className="home-content">
        <div className="filters-section">
          <h2>Filtros</h2>
          <div className="filters-grid">
            <div className="filter-group">
              <label>Pessoas</label>
              <Select
                isMulti
                options={personOptions}
                value={selectedPersons}
                onChange={(selected) => setSelectedPersons(selected as SelectOption[])}
                placeholder="Selecione pessoas..."
                isLoading={loadingData}
                className="react-select-container"
                classNamePrefix="react-select"
              />
            </div>

            <div className="filter-group">
              <label>Categorias</label>
              <Select
                isMulti
                options={categoryOptions}
                value={selectedCategories}
                onChange={(selected) => setSelectedCategories(selected as SelectOption[])}
                placeholder="Selecione categorias..."
                isLoading={loadingData}
                className="react-select-container"
                classNamePrefix="react-select"
              />
            </div>

            <div className="filter-group">
              <label>Tipo de Transação</label>
              <Select
                options={typeOptions}
                value={selectedType}
                onChange={(selected) => setSelectedType(selected)}
                placeholder="Todos os tipos"
                isClearable
                className="react-select-container"
                classNamePrefix="react-select"
              />
            </div>

            <div className="filter-group">
              <label>Data Inicial</label>
              <DatePicker
                selected={initialDate}
                onChange={(date: Date | null) => {
                  if (date) setInitialDate(date);
                }}
                dateFormat="dd/MM/yyyy"
              />
            </div>

            <div className="filter-group">
              <label>Data Final</label>
              <DatePicker
                selected={finalDate}
                onChange={(date: Date | null) => {
                  if (date) setFinalDate(date);
                }}
                dateFormat="dd/MM/yyyy"
                maxDate={new Date()}
              />
            </div>
          </div>
        </div>

        <div className="balance-section">
          <div className="balance-card income">
            <div className="balance-label">Receita</div>
            <div className="balance-value">{formatCurrency(income)}</div>
          </div>
          <div className="balance-card expense">
            <div className="balance-label">Despesas</div>
            <div className="balance-value">{formatCurrency(expense)}</div>
          </div>
          <div className={`balance-card balance ${balance >= 0 ? 'positive' : 'negative'}`}>
            <div className="balance-label">Balanço</div>
            <div className="balance-value">{formatCurrency(balance)}</div>
          </div>
        </div>

        <div className="transactions-section">
          <div className="transactions-header">
            <h2>Transações</h2>
            <button className="create-button" onClick={handleCreate}>
              + Criar
            </button>
          </div>
          {loading ? (
            <div className="loading">Carregando transações...</div>
          ) : transactions.length === 0 ? (
            <div className="no-data">Nenhuma transação encontrada</div>
          ) : (
            <div className="table-container">
              <table className="transactions-table">
                <thead>
                  <tr>
                    <th>Descrição</th>
                    <th>Valor</th>
                    <th>Tipo</th>
                    <th>Data</th>
                    <th>Categoria</th>
                    <th>Pessoa</th>
                    <th>Ações</th>
                  </tr>
                </thead>
                <tbody>
                  {transactions.map((transaction) => {
                    const person = persons.find((p) => p.id === transaction.personId);
                    const category = categories.find((c) => c.id === transaction.categoryId);
                    return (
                      <tr key={transaction.id}>
                        <td>{transaction.description}</td>
                        <td className={`amount ${transaction.type.toLowerCase()}`}>
                          {formatCurrency(transaction.amount)}
                        </td>
                        <td>
                          <span className={`type-badge ${transaction.type.toLowerCase()}`}>
                            {transaction.type === 'Income' ? 'Receita' : 'Despesa'}
                          </span>
                        </td>
                        <td>{formatDate(transaction.occurredAt)}</td>
                        <td>{category?.description || '-'}</td>
                        <td>{person?.name || '-'}</td>
                        <td>
                          <div className="actions">
                            <button
                              className="action-button edit"
                              onClick={() => handleEdit(transaction)}
                              title="Editar"
                            >
                              ✏️
                            </button>
                            <button
                              className="action-button delete"
                              onClick={() => handleDelete(transaction)}
                              title="Excluir"
                            >
                              🗑️
                            </button>
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}
        </div>

        {isModalOpen && (
          <TransactionModal
            transaction={editingTransaction}
            persons={persons}
            categories={categories}
            onClose={handleModalClose}
            onSuccess={handleModalSuccess}
          />
        )}
      </div>
    </div>
  );
};

export default Home;
