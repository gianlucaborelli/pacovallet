import { Routes, Route, Navigate } from 'react-router-dom';

import Auth from '../pages/auth/Auth';
import Home from '../pages/home/Home';
import Persons from '../pages/persons/Persons';
import Categories from '../pages/categories/Categories';

import ProtectedRoutes from './ProtectedRoutes';

export default function AppRoutes() {
  return (
    <Routes>
      {/* Rotas públicas */}
      <Route path="/login" element={<Auth />} />
      <Route path="/register" element={<Auth />} />

      {/* Rotas protegidas */}
      <Route element={<ProtectedRoutes />}>
        <Route path="/" element={<Home />} />
        <Route path="/persons" element={<Persons />} />
        <Route path="/categories" element={<Categories />} />
      </Route>

      {/* Fallback */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
