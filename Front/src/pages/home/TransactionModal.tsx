import React, { useState, useEffect, useMemo } from 'react';
import Select from 'react-select';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { transactionService } from '../../services/transactionService';
import { Transaction, Person, Category } from '../../types/transaction';
import './TransactionModal.css';

interface TransactionModalProps {
  transaction: Transaction | null;
  persons: Person[];
  categories: Category[];
  onClose: () => void;
  onSuccess: () => void;
}

interface SelectOption {
  value: string;
  label: string;
}

const typeOptions: SelectOption[] = [
  { value: 'Income', label: 'Receita' },
  { value: 'Expense', label: 'Despesa' },
];

const TransactionModal: React.FC<TransactionModalProps> = ({
  transaction,
  persons,
  categories,
  onClose,
  onSuccess,
}) => {
  const [description, setDescription] = useState('');
  const [amount, setAmount] = useState('');
  const [type, setType] = useState<SelectOption | null>(null);
  const [category, setCategory] = useState<SelectOption | null>(null);
  const [person, setPerson] = useState<SelectOption | null>(null);
  const [occurredAt, setOccurredAt] = useState<Date>(new Date());
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const isEditing = !!transaction;

  const personOptions: SelectOption[] = useMemo(
    () =>
      persons.map((p) => ({
        value: p.id,
        label: p.name,
      })),
    [persons]
  );

  const categoryOptions: SelectOption[] = useMemo(
    () =>
      categories.map((c) => ({
        value: c.id,
        label: c.description,
      })),
    [categories]
  );

  useEffect(() => {
    if (transaction) {
      setDescription(transaction.description);
      setAmount(transaction.amount.toString());
      setType(typeOptions.find((opt) => opt.value === transaction.type) || null);
      setCategory(
        categoryOptions.find((opt) => opt.value === transaction.categoryId) || null
      );
      setPerson(personOptions.find((opt) => opt.value === transaction.personId) || null);
      setOccurredAt(new Date(transaction.occurredAt));
    } else {
      setDescription('');
      setAmount('');
      setType(null);
      setCategory(null);
      setPerson(null);
      setOccurredAt(new Date());
    }
  }, [transaction, personOptions, categoryOptions]);

  const handleAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value.replace(/[^0-9.,]/g, '');
    const normalizedValue = value.replace(',', '.');
    const parts = normalizedValue.split('.');
    
    if (parts.length > 2) {
      return;
    }
    
    if (parts[1] && parts[1].length > 2) {
      return;
    }
    
    setAmount(normalizedValue);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!description.trim()) {
      setError('A descrição é obrigatória');
      return;
    }

    if (!amount || parseFloat(amount) <= 0) {
      setError('O valor deve ser maior que zero');
      return;
    }

    if (!type) {
      setError('O tipo é obrigatório');
      return;
    }

    if (!category) {
      setError('A categoria é obrigatória');
      return;
    }

    if (!person) {
      setError('A pessoa é obrigatória');
      return;
    }

    const amountValue = parseFloat(amount);
    if (isNaN(amountValue) || amountValue <= 0) {
      setError('O valor deve ser um número positivo');
      return;
    }

    try {
      setLoading(true);

      if (isEditing && transaction) {
        if (!window.confirm(`Tem certeza que deseja atualizar esta transação?`)) {
          return;
        }
        await transactionService.updateTransaction(
          transaction.id,
          description.trim(),
          amountValue,
          occurredAt,
          type.value as 'Income' | 'Expense',
          category.value,
          person.value
        );
        alert('Transação atualizada com sucesso!');
      } else {
        await transactionService.createTransaction(
          description.trim(),
          amountValue,
          occurredAt,
          type.value as 'Income' | 'Expense',
          category.value,
          person.value
        );
        alert('Transação criada com sucesso!');
      }

      onSuccess();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao salvar transação');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content transaction-modal" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEditing ? 'Editar Transação' : 'Criar Transação'}</h2>
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
              placeholder="Digite a descrição"
              required
              disabled={loading}
            />
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="amount">Valor *</label>
              <input
                type="text"
                id="amount"
                value={amount}
                onChange={handleAmountChange}
                placeholder="0.00"
                required
                disabled={loading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="type">Tipo *</label>
              <Select
                options={typeOptions}
                value={type}
                onChange={(selected) => setType(selected)}
                placeholder="Selecione o tipo"
                isClearable={false}
                className="react-select-container"
                classNamePrefix="react-select"
                isDisabled={loading}
              />
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="category">Categoria *</label>
              <Select
                options={categoryOptions}
                value={category}
                onChange={(selected) => setCategory(selected)}
                placeholder="Selecione a categoria"
                isClearable={false}
                className="react-select-container"
                classNamePrefix="react-select"
                isDisabled={loading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="person">Pessoa *</label>
              <Select
                options={personOptions}
                value={person}
                onChange={(selected) => setPerson(selected)}
                placeholder="Selecione a pessoa"
                isClearable={false}
                className="react-select-container"
                classNamePrefix="react-select"
                isDisabled={loading}
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="occurredAt">Data *</label>
            <DatePicker
              selected={occurredAt}
              onChange={(date: Date | null) => {
                if (date) setOccurredAt(date);
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

export default TransactionModal;

