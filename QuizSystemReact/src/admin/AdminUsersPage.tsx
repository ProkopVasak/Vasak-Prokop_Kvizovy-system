import React, { useEffect, useState } from 'react';
import styles from './AdminUsersPage.module.css';

// Rozhraní pro uživatele
interface User {
  id: string; // GUID
  email: string;
  roles: string[];
}

const AdminUsersPage: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const token = localStorage.getItem('accessToken');

  // Funkce pro načtení uživatelů
  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/admin/users`, {
          headers: {
            'Authorization': `Bearer ${token}`,
          },
        });

        if (response.ok) {
          const data = await response.json();
          const usersWithRoles = data.map((item: any) => ({
            id: item.user.id,
            email: item.user.email,
            roles: item.roles.length > 0 ? item.roles : ['User'],
          }));
          setUsers(usersWithRoles);
        } else {
          console.error('Chyba při načítání uživatelů');
        }
      } catch (error) {
        console.error('Chyba:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, [token]);

  useEffect(() => {
    document.title = "Admin Users"; 
}, []);

  // Funkce pro změnu role uživatele
  const changeRole = async (userId: string, newRole: string) => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/api/admin/change-role`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify({ userId, newRole }),
      });

      if (response.ok) {
        setUsers((prevUsers) =>
          prevUsers.map((user) =>
            user.id === userId ? { ...user, roles: [newRole] } : user
          )
        );
      } else {
        console.error('Chyba při změně role');
      }
    } catch (error) {
      console.error('Chyba:', error);
    }
  };

  // Funkce pro mazání uživatele
  const deleteUser = async (userId: string) => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/api/admin/delete-user/${userId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setUsers((prevUsers) => prevUsers.filter((user) => user.id !== userId));
      } else {
        console.error('Chyba při mazání uživatele');
      }
    } catch (error) {
      console.error('Chyba:', error);
    }
  };

  if (loading) {
    return <p>Načítání...</p>;
  }

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Správa uživatelů</h1>
      <table className={styles.table}>
        <thead>
          <tr>
            <th className={styles.cell}>Email</th>
            <th className={styles.cell}>Role</th>
            <th className={styles.cell}>Změna Role</th>
            <th className={styles.cell}>Smazat</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id} className={styles.row}>
              <td className={styles.cell}>{user.email}</td>
              <td className={styles.cell}>{user.roles.join(', ')}</td>
              <td className={styles.cell}>
                <select
                  value={user.roles[0]}
                  onChange={(e) => changeRole(user.id, e.target.value)}
                  className={styles.select}
                >
                  <option value="User">User</option>
                  <option value="Admin">Admin</option>
                </select>
              </td>
              <td className={styles.cell}>
                <button
                  onClick={() => deleteUser(user.id)}
                  className={styles.deleteButton}
                >
                  Smazat
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default AdminUsersPage;
