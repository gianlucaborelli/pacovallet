import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import './Auth.css';

const Auth: React.FC = () => {
  const [activeTab, setActiveTab] = useState<'login' | 'register'>('login');
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    confirmPassword: '',
    name: ''
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login, register, isAuthenticated, loading: authLoading } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    // Determina a aba baseada na URL
    if (location.pathname.includes('register')) {
      setActiveTab('register');
    } else {
      setActiveTab('login');
    }
  }, [location.pathname]);

  useEffect(() => {
    if (!authLoading && isAuthenticated) {
      navigate('/');
    }
  }, [isAuthenticated, authLoading, navigate]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
    setError(''); // Limpa erro quando usuário digita
  };

  const handleTabChange = (tab: 'login' | 'register') => {
    setActiveTab(tab);
    setError('');
    // Atualiza a URL sem recarregar a página
    navigate(tab === 'login' ? '/login' : '/register', { replace: true });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      if (activeTab === 'login') {
        await login(formData.email, formData.password);
      } else {
        if (formData.password !== formData.confirmPassword) {
          setError('As senhas não coincidem');
          return;
        }
        await register(formData.name, formData.email, formData.password, formData.confirmPassword);
      }
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro na autenticação');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      {/* Lado Esquerdo - Landpage */}
      <div className="landing-side">
        <img 
          src="/logo192.png" 
          alt="PacoVallet Logo" 
          className="landing-logo"
        />
        <h1 className="landing-title">PacoVallet</h1>
        <p className="landing-subtitle">Sua carteira digital inteligente</p>
        
        <div className="landing-description">
          <h3>🌿 Controle que Fortalece</h3>
          <p>
            Como as folhas do pacová, que são resistentes e duradouras, 
            o PacoVallet fortalece sua saúde financeira com controle 
            inteligente de gastos e organização completa.
          </p>
          
          <h3>🏺 Segurança que Protege</h3>
          <p>
            Inspirado na solidez de uma carteira bem estruturada, 
            oferecemos proteção e confiabilidade para seus dados 
            financeiros e decisões importantes.
          </p>
          
          <div className="color-meaning">
            <div className="color-item">
              <div className="color-circle green"></div>
              <span className="color-text">
                <strong>Verde Pacová:</strong> Força, resistência e crescimento financeiro
              </span>
            </div>
            <div className="color-item">
              <div className="color-circle terracotta"></div>
              <span className="color-text">
                <strong>Terracota:</strong> Solidez, confiança e estabilidade
              </span>
            </div>
          </div>
        </div>
      </div>

      {/* Lado Direito - Autenticação */}
      <div className="auth-side">
        <div className="auth-card">
          <div className="auth-tabs">
            <button 
              className={`auth-tab ${activeTab === 'login' ? 'active' : ''}`}
              onClick={() => handleTabChange('login')}
            >
              Entrar
            </button>
            <button 
              className={`auth-tab ${activeTab === 'register' ? 'active' : ''}`}
              onClick={() => handleTabChange('register')}
            >
              Cadastrar
            </button>
          </div>

          <form onSubmit={handleSubmit} className="auth-form">
            <div className="auth-header">
              <h2>
                {activeTab === 'login' ? 'Bem-vindo de volta!' : 'Criar conta'}
              </h2>
              <p>
                {activeTab === 'login' 
                  ? 'Acesse sua carteira digital'
                  : 'Comece sua jornada financeira'
                }
              </p>
            </div>

            {error && <div className="error-message">{error}</div>}

            {activeTab === 'register' && (
              <div className="form-group">
                <label htmlFor="name">Nome completo</label>
                <input
                  type="text"
                  id="name"
                  name="name"
                  value={formData.name}
                  onChange={handleInputChange}
                  required={activeTab === 'register'}
                  placeholder="Seu nome completo"
                />
              </div>
            )}

            <div className="form-group">
              <label htmlFor="email">Email</label>
              <input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleInputChange}
                required
                placeholder="seu@email.com"
              />
            </div>

            <div className="form-group">
              <label htmlFor="password">Senha</label>
              <input
                type="password"
                id="password"
                name="password"
                value={formData.password}
                onChange={handleInputChange}
                required
                placeholder="••••••••"
                minLength={6}
              />
            </div>

            {activeTab === 'register' && (
              <div className="form-group">
                <label htmlFor="confirmPassword">Confirmar senha</label>
                <input
                  type="password"
                  id="confirmPassword"
                  name="confirmPassword"
                  value={formData.confirmPassword}
                  onChange={handleInputChange}
                  required={activeTab === 'register'}
                  placeholder="••••••••"
                  minLength={6}
                />
              </div>
            )}

            <button 
              type="submit" 
              className="auth-button"
              disabled={loading}
            >
              {loading 
                ? (activeTab === 'login' ? 'Entrando...' : 'Criando conta...') 
                : (activeTab === 'login' ? 'Entrar' : 'Criar conta')
              }
            </button>

            {activeTab === 'login' && (
              <div className="auth-footer">
                <button 
                  type="button" 
                  className="forgot-password"
                  onClick={() => alert('Funcionalidade em desenvolvimento')}
                >
                  Esqueceu sua senha?
                </button>
              </div>
            )}

            {activeTab === 'register' && (
              <div className="auth-footer">
                <p>
                  Ao criar uma conta, você concorda com nossos termos de uso e política de privacidade.
                </p>
              </div>
            )}
          </form>
        </div>
      </div>
    </div>
  );
};

export default Auth;