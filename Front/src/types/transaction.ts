export interface Person {
  id: string;
  name: string;
  birthDate: string;
  age: number;
}

export interface Category {
  id: string;
  description: string;
  purpose: 'Both' | 'Income' | 'Expense';
}

export interface Transaction {
  id: string;
  description: string;
  amount: number | string;
  occurredAt: string;
  type: 'Income' | 'Expense';
  categoryId: string;
  personId: string;
}

export interface TransactionFilters {
  initialDate?: Date;
  finalDate?: Date;
  type?: 'Income' | 'Expense' | '';
  personsId?: string[];
  category?: string[];
}

