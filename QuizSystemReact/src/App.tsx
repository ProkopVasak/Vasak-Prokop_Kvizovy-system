import './App.css'; // Import CSS souboru pro stylování aplikace
import Navbar from './components/Navbar'; // Komponenta pro navigační panel
import { AuthProvider } from './context/AuthContext'; // Context provider pro autentizaci uživatele
import { QuizProvider } from './context/QuizContext'; // Context provider pro quizy
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'; // Router pro správu trasování
import AuthPage from './pages/AuthPage'; // Stránka pro přihlášení/registraci
import HomePage from './pages/HomePage'; // Úvodní stránka
import QuizPage from './pages/QuizPage'; // Stránka pro zobrazení kvízu
import Create from './pages/CreateQuizPage'; // Stránka pro vytvoření nového kvízu
import UserPage from './pages/UserPage'; // Stránka pro zobrazení kvízů uživatele
import EditQuizPage from './pages/EditQuizPage'; // Stránka pro úpravu kvízu
import AdminLayout from './admin/AdminLayout'; // Administrátorská sekce aplikace
import ViewPage from './pages/ViewPage'; // Stránka pro zobrazení kvízu v režimu pouze pro čtení

function App() {
  return (
    <Router>
      <AuthProvider>
        <QuizProvider>
          <Navbar />
          
          <Routes>
            <Route path="/admin/*" element={<AdminLayout />} />
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<AuthPage />} />
            <Route path="/register" element={<AuthPage />} />
            <Route path="/quiz/:id" element={<QuizPage />} />
            <Route path="/create" element={<Create />} />
            <Route path="/user" element={<UserPage />} />
            <Route path="/edit/:id" element={<EditQuizPage />} />
            <Route path="/view/:id" element={<ViewPage />} />
          </Routes>
        </QuizProvider>
      </AuthProvider>
    </Router>
  );
}

export default App;
