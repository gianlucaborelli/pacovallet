import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import './SideMenu.css';

interface SideMenuProps {
  isOpen: boolean;
  onClose: () => void;
}

const SideMenu: React.FC<SideMenuProps> = ({ isOpen, onClose }) => {
  const location = useLocation();

  const menuItems = [
    { path: '/', label: 'Dashboard', icon: '📊' },
    { path: '/persons', label: 'Pessoas', icon: '👥' },
    { path: '/categories', label: 'Categorias', icon: '📁' },
  ];

  return (
    <>
      {isOpen && <div className="side-menu-overlay" onClick={onClose} />}
      <div className={`side-menu ${isOpen ? 'open' : ''}`}>
        <div className="side-menu-header">
          <h2>Menu</h2>
          <button className="close-button" onClick={onClose}>
            ✕
          </button>
        </div>
        <nav className="side-menu-nav">
          {menuItems.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              className={`side-menu-item ${location.pathname === item.path ? 'active' : ''}`}
              onClick={onClose}
            >
              <span className="menu-icon">{item.icon}</span>
              <span className="menu-label">{item.label}</span>
            </Link>
          ))}
        </nav>
      </div>
    </>
  );
};

export default SideMenu;

