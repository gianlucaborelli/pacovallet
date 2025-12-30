import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import SideMenu from './SideMenu';
import './Layout.css';

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { user, logout } = useAuth();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  return (
    <div className="layout-container">
      <header className="app-header">
        <div className="header-left">
          <button className="menu-button" onClick={() => setIsMenuOpen(true)}>
            ☰
          </button>
          <img src="/logo192.png" alt="App Logo" className="app-logo" />
          <h1>PacoVallet</h1>
        </div>
        <div className="user-info">
          <span>Olá, {user?.name}!</span>
          <button onClick={logout} className="logout-button">
            Sair
          </button>
        </div>
      </header>

      <SideMenu isOpen={isMenuOpen} onClose={() => setIsMenuOpen(false)} />

      <main className="layout-content">
        {children}
      </main>
    </div>
  );
};

export default Layout;

