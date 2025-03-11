import { Link } from 'react-router-dom';
import {  Route, Routes } from 'react-router-dom';
import AdminUsersPage from './AdminUsersPage'; 

function AdminLayout() {

  return (
    <div>
      <h2>Admin Sekce</h2>
      <nav>
        <ul>
          <li><Link to="/admin/users">Uživatelé</Link></li>
        </ul>
      </nav>
      <Routes>
        <Route path="users" element={<AdminUsersPage></AdminUsersPage>} />
      </Routes>
    </div>
  );
}

export default AdminLayout;
