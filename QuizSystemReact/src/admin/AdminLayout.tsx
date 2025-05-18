import { useContext, useEffect, useState } from 'react';
import {
  Link,
  Route,
  Routes,
  useNavigate,
  Navigate
} from 'react-router-dom';
import AdminUsersPage from './AdminUsersPage';
import { AuthContext } from '../context/AuthContext';

function AdminLayout() {
  const { user } = useContext(AuthContext);
  const navigate = useNavigate();
  // null = loading, [] = načteno, ale bez rolí (nebo chyba)
  const [roles, setRoles] = useState<string[] | null>(null);

  useEffect(() => {
    // Pokud uživatel není přihlášený, přesměruj ihned
    


    // Načítáme role přímo z API
    fetch(`${import.meta.env.VITE_API_URL}/api/manage/roles`, {
      credentials: 'include', 
      headers: {  }
    })
      .then(res => {
        if (!res.ok) {
          throw new Error('Failed to load roles');
        }
        return res.json() as Promise<string[]>;
      })
      .then(rs => setRoles(rs))
      .catch(() => setRoles([]));
  }, [user, navigate]);

  // Zobrazíme loading dokud se role nenačtou
  if (roles === null) {
    return <div>Checking permissions…</div>;
  }
  // Vypiš role uživatele
  if (roles.length > 0) {
    console.log('Vaše role:', roles);
  }
  // Pokud uživatel nemá administratorskou roli, přesměruj na domovskou stránku
  if (!roles.includes('Admin')) {
    return <Navigate to="/" replace />;
  }

  // Uživatel má roli ADMIN – zobrazíme admin rozhraní
  return (
    <div>
      <h2>Admin Sekce</h2>
      <nav>
        <ul>
          <li>
            <Link to="/admin/users">Uživatelé</Link>
          </li>
        </ul>
      </nav>
      <Routes>
        <Route path="users" element={<AdminUsersPage />} />
      </Routes>
    </div>
  );
}

export default AdminLayout;
