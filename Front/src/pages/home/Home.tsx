import React from 'react';
import { useAuth } from '../../contexts/AuthContext';
import './Home.css';

const Home: React.FC = () => {
  const { user, logout } = useAuth();

  return (
    <div className="home-container">
      <header className="home-header">
        <h1>Bem-vindo ao PacoVallet</h1>
        <div className="user-info">
          <span>Olá, {user?.name}!</span>
          <button onClick={logout} className="logout-button">
            Sair
          </button>
        </div>
      </header>
      <main className="home-content">
        <p>Você está autenticado e pode acessar o conteúdo protegido.</p>
      </main>
    </div>
  );
};

export default Home;

