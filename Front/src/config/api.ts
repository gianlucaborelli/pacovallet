export const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:7220';

export const API_ENDPOINTS = {
  LOGIN: `${API_URL}/api/identity/login`,
  REGISTER: `${API_URL}/api/identity`,
};
