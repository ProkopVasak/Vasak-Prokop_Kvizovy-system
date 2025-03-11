// src/pages/AuthPage.js
import { useState, useContext, useEffect } from 'react';
import { AuthContext } from '../context/AuthContext';
import { useLocation } from 'react-router-dom';
import styles from './AuthPage.module.css';

const AuthPage = () => {
    // Získání funkcí z AuthContext pro login a registraci
    const { login, register } = useContext(AuthContext);
    
    // Stav pro email a heslo uživatele
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    
    // Získání aktuální URL pro rozhodnutí, zda je stránka pro login nebo registraci
    const location = useLocation();

    // Určení, zda je aktuální stránka pro login
    const isLogin = location.pathname === '/login';

    // Funkce pro odeslání formuláře (login nebo registrace)
    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();  // Zabraňuje defaultnímu chování formuláře (obnovení stránky)
        if (isLogin) {
            login(email, password);  // Zavolání funkce login, pokud je stránka pro login
        } else {
            register(email, password);  // Zavolání funkce register, pokud je stránka pro registraci
        }
    };

    // Aktualizace titulku stránky podle toho, zda je to login nebo registrace
    useEffect(() => {
        document.title = isLogin ? "Login" : "Register"; // Nastavení textu záložky pro login nebo registraci
    }, [isLogin]);

    return (
        <div className={styles.authPage}>
            <h2 className={styles.title}>{isLogin ? "Login" : "Register"}</h2>  {/* Zobrazení titulu pro login nebo registraci */}
            
            <form className={styles.form} onSubmit={handleSubmit}> {/* Formulář pro přihlášení nebo registraci */}
                <div className={styles.formGroup}>
                    <label className={styles.label}>Email</label>
                    <input
                        className={styles.input}
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}  // Aktualizace hodnoty emailu
                        required
                    />
                </div>
                
                <div className={styles.formGroup}>
                    <label className={styles.label}>Password</label>
                    <input
                        className={styles.input}
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}  // Aktualizace hodnoty hesla
                        required
                    />
                </div>
                
                <button className={styles.button} type="submit">
                    {isLogin ? "Login" : "Register"}  {/* Text tlačítka se mění podle stránky */}
                </button>
            </form>
        </div>
    );
};

export default AuthPage;
